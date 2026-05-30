using UnityEngine;
using Meta.XR;
using UnityEngine.Android;

public class RaycastPlacement : MonoBehaviour
{
    [SerializeField] private GameObject taulaPrefab;
    [SerializeField] private GameObject previewPrefab;

    [SerializeField] private Transform rightController;
    [SerializeField] private Transform leftController;

    [SerializeField] private Transform playerCamera;
    [SerializeField] private EnvironmentRaycastManager raycastManager;

    private GameObject previewObject;
    private GameObject placedObject;

    private const string SCENE_PERMISSION = "com.oculus.permission.USE_SCENE";

    private void Awake()
    {
        Permission.RequestUserPermission(SCENE_PERMISSION);
    }

    void Start()
    {
        previewObject = Instantiate(previewPrefab);
        previewObject.SetActive(false);
    }

    void Update()
    {
        if (placedObject != null)
            return; // SOLO UNO

        TryPreviewAndPlace(rightController, OVRInput.RawButton.RIndexTrigger);
        TryPreviewAndPlace(leftController, OVRInput.RawButton.LIndexTrigger);
    }

    void TryPreviewAndPlace(Transform controller, OVRInput.RawButton button)
    {
        Ray ray = new Ray(controller.position, controller.forward);

        if (raycastManager.Raycast(ray, out EnvironmentRaycastHit hit))
        {
            previewObject.SetActive(true);

            Vector3 up = hit.normal.normalized;

            Vector3 forward = Vector3.ProjectOnPlane(playerCamera.forward, up).normalized;
            if (forward.sqrMagnitude < 0.001f)
                forward = Vector3.ProjectOnPlane(controller.forward, up).normalized;

            Vector3 position = hit.point + hit.normal * 0.01f;

            previewObject.transform.position = position;
            previewObject.transform.rotation = Quaternion.LookRotation(forward, up);

            previewObject.transform.localScale = Vector3.one * 0.2f;

            if (OVRInput.GetDown(button))
            {
                PlaceObject(position, previewObject.transform.rotation);
            }
        }
        else
        {
            previewObject.SetActive(false);
        }
    }

    void PlaceObject(Vector3 position, Quaternion rotation)
    {
        placedObject = Instantiate(taulaPrefab);

        placedObject.transform.position = position;
        placedObject.transform.rotation = rotation;
        placedObject.transform.localScale = Vector3.one * 0.2f;

        previewObject.SetActive(false);
    }
}
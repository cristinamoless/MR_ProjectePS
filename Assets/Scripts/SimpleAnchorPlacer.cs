using UnityEngine;
using Meta.XR.BuildingBlocks;

public class SimpleAnchorPlacer : MonoBehaviour
{
    public SpatialAnchorSpawnerBuildingBlock spawner;
    public GameObject previewPrefab;

    private GameObject previewInstance;
    private GameObject currentAnchor;

    private bool canPlace = false;

    void Start()
    {
        previewInstance = Instantiate(previewPrefab);
        previewInstance.SetActive(false);
    }

    void Update()
    {
        if (!canPlace)
        {
            previewInstance.SetActive(false);
            return;
        }

        Transform cam = Camera.main.transform;
        Vector3 pos = cam.position + cam.forward * 1.5f;

        previewInstance.SetActive(true);
        previewInstance.transform.position = pos;

        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            PlaceAnchor(pos);
        }
    }

    public void EnablePlacement()
    {
        canPlace = true;
        previewInstance.SetActive(true);
    }

    public void DisablePlacement()
    {
        canPlace = false;
        previewInstance.SetActive(false);
    }

    void PlaceAnchor(Vector3 position)
    {
        if (currentAnchor != null)
        {
            Destroy(currentAnchor);
        }

        spawner.SpawnSpatialAnchor(position, Quaternion.identity);

        currentAnchor = GameObject.Find("Spatial Anchor(Clone)");

        canPlace = false;
        previewInstance.SetActive(false);
    }
}
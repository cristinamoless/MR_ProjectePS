using UnityEngine;
using Meta.XR.BuildingBlocks;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class SimpleAnchorPlacer : MonoBehaviour
{
    public SpatialAnchorSpawnerBuildingBlock spawner;
    public Vector3 rotationOffset = Vector3.zero;

    private GameObject currentActivePrefab;
    private GameObject previewInstance;
    private GameObject currentTriggerButton;
    private List<GameObject> placedAnchors = new List<GameObject>();
    private bool canPlace = false;
    private SpatialAnchorCoreBuildingBlock spatialAnchorCore;

    void Awake()
    {
        spatialAnchorCore = FindFirstObjectByType<SpatialAnchorCoreBuildingBlock>();
        if (spatialAnchorCore != null)
        {
            spatialAnchorCore.OnAnchorCreateCompleted.AddListener(OnAnchorCreated);
        }
        else
        {
            Debug.LogWarning("SimpleAnchorPlacer: SpatialAnchorCoreBuildingBlock no s'ha trobat.");
        }
    }

    void OnDestroy()
    {
        if (spatialAnchorCore != null)
        {
            spatialAnchorCore.OnAnchorCreateCompleted.RemoveListener(OnAnchorCreated);
        }
    }

    void Update()
    {
        if (!canPlace)
        {
            if (previewInstance != null) previewInstance.SetActive(false);
            return;
        }

        Transform cam = Camera.main.transform;
        Vector3 pos = cam.position + cam.forward * 1.5f;

        if (previewInstance != null)
        {
            previewInstance.SetActive(true);
            previewInstance.transform.position = pos;

            Vector3 forward = cam.forward;
            forward.y = 0;
            if (forward.sqrMagnitude > 0.001f)
            {
                Quaternion lookRotation = Quaternion.LookRotation(forward.normalized, Vector3.up);
                previewInstance.transform.rotation = lookRotation * Quaternion.Euler(rotationOffset);
            }
        }

        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            PlaceAnchorAtCurrentPosition();
        }
    }

    public void EnablePlacement(GameObject prefabACollocar)
    {
        if (prefabACollocar == null) return;

        if (previewInstance != null) Destroy(previewInstance);

        currentActivePrefab = prefabACollocar;

        if (EventSystem.current != null && EventSystem.current.currentSelectedGameObject != null)
        {
            currentTriggerButton = EventSystem.current.currentSelectedGameObject;
        }

        previewInstance = Instantiate(currentActivePrefab);
        previewInstance.SetActive(true);

        if (currentTriggerButton != null) currentTriggerButton.SetActive(false);

        canPlace = true;
    }

    public void DisablePlacement(bool placementSuccessful)
    {
        canPlace = false;
        if (previewInstance != null)
        {
            Destroy(previewInstance);
        }

        if (currentTriggerButton != null)
        {
            currentTriggerButton.SetActive(!placementSuccessful);
        }

        currentTriggerButton = null;
    }

    public void PlaceAnchorAtCurrentPosition()
    {
        if (!canPlace || currentActivePrefab == null) return;

        // Comprovació de seguretat per si la preview s'ha destruït o desactivat inesperadament
        if (previewInstance == null || !previewInstance.activeSelf) return;

        Vector3 pos = previewInstance.transform.position;
        PlaceAnchor(pos);
    }

    void PlaceAnchor(Vector3 position)
    {
        if (spatialAnchorCore == null || currentActivePrefab == null) return;

        Quaternion rotation = previewInstance != null ? previewInstance.transform.rotation : Quaternion.identity;

        spatialAnchorCore.InstantiateSpatialAnchor(currentActivePrefab, position, rotation);

        DisablePlacement(true);

        var flow = FindFirstObjectByType<GameFlowManager>();
        if (flow != null) { }
    }

    private void OnAnchorCreated(OVRSpatialAnchor anchor, OVRSpatialAnchor.OperationResult result)
    {
        if (result == OVRSpatialAnchor.OperationResult.Success)
        {
            placedAnchors.Add(anchor.gameObject);
            Debug.Log($"SimpleAnchorPlacer: Nou element a la sala: {anchor.name}");
        }
        else
        {
            Debug.LogError($"SimpleAnchorPlacer: Error: {result}");
        }
    }
}
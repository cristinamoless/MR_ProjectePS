using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class PlacementManager : MonoBehaviour
{
    public XRRayInteractor rightHandRay;

    public GameObject windowPrefab;
    public Transform placementMarker;

    public Transform tableAnchor;
    public Transform windowAnchor;

    private GameObject previewWindow;
    private bool placingTable = false;
    private bool placingWindow = false;

    void Update()
    {
        if (!placingTable && !placingWindow)
            return;

        if (rightHandRay.TryGetCurrent3DRaycastHit(out RaycastHit hit))
        {
            placementMarker.gameObject.SetActive(true);
            placementMarker.position = hit.point;
            placementMarker.rotation = Quaternion.LookRotation(hit.normal);

            if (placingWindow && previewWindow != null)
            {
                previewWindow.transform.position = hit.point;
                previewWindow.transform.rotation = Quaternion.LookRotation(hit.normal);
            }
        }
    }

    public void StartPlacingTable()
    {
        placingTable = true;
        placingWindow = false;

        if (previewWindow != null)
            Destroy(previewWindow);

        placementMarker.gameObject.SetActive(true);
    }

    public void StartPlacingWindow()
    {
        placingTable = false;
        placingWindow = true;

        if (previewWindow != null)
            Destroy(previewWindow);

        previewWindow = Instantiate(windowPrefab);
        placementMarker.gameObject.SetActive(true);
    }

    public void ConfirmPlacement()
    {
        if (placingTable)
        {
            tableAnchor.position = placementMarker.position;
            tableAnchor.rotation = placementMarker.rotation;

            placingTable = false;
            placementMarker.gameObject.SetActive(false);
            return;
        }

        if (placingWindow)
        {
            windowAnchor.position = previewWindow.transform.position;
            windowAnchor.rotation = previewWindow.transform.rotation;

            var npcManager = FindFirstObjectByType<NPCManager>();
            npcManager.spawnPoint = windowAnchor;

            placingWindow = false;
            placementMarker.gameObject.SetActive(false);

            previewWindow = null;

            var gameFlow = FindFirstObjectByType<GameFlowManager>();
            gameFlow.StartDay();
        }
    }
}



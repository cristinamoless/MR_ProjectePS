using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class PlacementManager : MonoBehaviour
{
    public XRRayInteractor rightHandRay;

    public Transform placementMarker;

    public GameObject tablePrefab;
    public GameObject windowPrefab;

    public Transform tableAnchor;
    public Transform windowAnchor;

    private GameObject previewTable;
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

            if (placingTable && previewTable != null)
            {
                previewTable.transform.position = hit.point;
                previewTable.transform.rotation = Quaternion.LookRotation(hit.normal);
            }

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

        if (previewWindow != null) Destroy(previewWindow);
        if (previewTable != null) Destroy(previewTable);

        previewTable = Instantiate(tablePrefab);
        placementMarker.gameObject.SetActive(true);
    }

    public void StartPlacingWindow()
    {
        placingTable = false;
        placingWindow = true;

        if (previewWindow != null) Destroy(previewWindow);
        if (previewTable != null) Destroy(previewTable);

        previewWindow = Instantiate(windowPrefab);
        placementMarker.gameObject.SetActive(true);
    }

    public void ConfirmPlacement()
    {
        if (placingTable)
        {
            tableAnchor.position = previewTable.transform.position;
            tableAnchor.rotation = previewTable.transform.rotation;

            Destroy(previewTable);
            placingTable = false;
            placementMarker.gameObject.SetActive(false);
            return;
        }

        if (placingWindow)
        {
            windowAnchor.position = previewWindow.transform.position;
            windowAnchor.rotation = previewWindow.transform.rotation;

            Destroy(previewWindow);
            placingWindow = false;
            placementMarker.gameObject.SetActive(false);

            // Moure ComandaArea
            var comandaArea = FindFirstObjectByType<ComandaArea>();
            comandaArea.transform.position = windowAnchor.position + windowAnchor.forward * 0.5f;
            comandaArea.transform.rotation = windowAnchor.rotation;

            // Passar a botiga
            var gameFlow = FindFirstObjectByType<GameFlowManager>();
            gameFlow.StartShopPhase();
        }
    }
}

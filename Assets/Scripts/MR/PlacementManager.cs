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

    private bool tablePlaced = false;
    private bool windowPlaced = false;

    [Header("UI Integration (Optional)")]
    public GameObject setupMenuPanel;
    public GameObject confirmPanel;
    public GameObject continueButton;

    public bool IsTablePlaced => tablePlaced;
    public bool IsWindowPlaced => windowPlaced;

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

        UpdateUIPanels(true);
    }

    public void StartPlacingWindow()
    {
        placingTable = false;
        placingWindow = true;

        if (previewWindow != null) Destroy(previewWindow);
        if (previewTable != null) Destroy(previewTable);

        previewWindow = Instantiate(windowPrefab);
        placementMarker.gameObject.SetActive(true);

        UpdateUIPanels(true);
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
            
            tablePlaced = true;
            UpdateUIPanels(false);
            return;
        }

        if (placingWindow)
        {
            windowAnchor.position = previewWindow.transform.position;
            windowAnchor.rotation = previewWindow.transform.rotation;

            Destroy(previewWindow);
            placingWindow = false;
            placementMarker.gameObject.SetActive(false);

            // Moure ComandaArea (si existeix)
            var comandaArea = FindFirstObjectByType<ComandaArea>();
            if (comandaArea != null)
            {
                comandaArea.transform.position = windowAnchor.position + windowAnchor.forward * 0.5f;
                comandaArea.transform.rotation = windowAnchor.rotation;
            }

            windowPlaced = true;
            UpdateUIPanels(false);
        }
    }

    public void StartGame()
    {
        if (tablePlaced && windowPlaced)
        {
            var gameFlow = FindFirstObjectByType<GameFlowManager>();
            if (gameFlow != null)
            {
                gameFlow.StartShopPhase();
            }
            else
            {
                Debug.LogWarning("GameFlowManager not found in scene! Starting game simulation locally.");
            }

            // Ocultar el Canvas completament en començar el joc
            if (setupMenuPanel != null) setupMenuPanel.SetActive(false);
            if (confirmPanel != null) confirmPanel.SetActive(false);
            if (continueButton != null) continueButton.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Cannot start game: Both Table and Window must be placed first.");
        }
    }

    private void UpdateUIPanels(bool isPlacing)
    {
        if (setupMenuPanel != null) setupMenuPanel.SetActive(!isPlacing);
        if (confirmPanel != null) confirmPanel.SetActive(isPlacing);
        
        // El botó Continuar només es mostra o activa si ambdues coses estan col·locades
        if (continueButton != null)
        {
            continueButton.SetActive(tablePlaced && windowPlaced);
        }
    }
}

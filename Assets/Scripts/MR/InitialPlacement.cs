using UnityEngine;

public class InitialPlacement : MonoBehaviour
{
    public PlacementManager placementManager;
    public GameObject tablePrefab;
    private GameObject previewTable;


    public void OnPlaceTable()
    {
        placementManager.StartPlacingTable();
    }

    public void OnPlaceWindow()
    {
        placementManager.StartPlacingWindow();
    }

    public void OnConfirm()
    {
        placementManager.ConfirmPlacement();
    }
}

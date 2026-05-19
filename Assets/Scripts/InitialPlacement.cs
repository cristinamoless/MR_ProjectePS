using UnityEngine;

public class InitialPlacement : MonoBehaviour
{
    public PlacementManager placementManager;

    public void OnPlaceTable()
    {
        placementManager.StartPlacingTable();
    }

    public void OnPlaceWindow()
    {
        placementManager.StartPlacingWindow();
    }
}

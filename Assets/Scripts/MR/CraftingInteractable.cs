using UnityEngine;

public class CraftingInteractable : MonoBehaviour
{
    public void OnSelectEntered()
    {
        var gameFlow = FindFirstObjectByType<GameFlowManager>();
        gameFlow.StartCraftingPhase();
    }
}

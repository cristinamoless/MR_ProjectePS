using UnityEngine;

public class WindowInteractable : MonoBehaviour
{
    public void OnSelectEntered()
    {
        var gameFlow = FindFirstObjectByType<GameFlowManager>();
        gameFlow.TalkClients();
    }
}

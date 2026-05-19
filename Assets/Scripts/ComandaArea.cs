using UnityEngine;
using TMPro;

public class ComandaArea : MonoBehaviour
{
    public TMP_Text text;
    public GameFlowManager gameFlow;

    public bool hasTalked = false;

    private void Start()
    {
        text.text = " ";
    }
    private void OnTriggerStay(Collider other)
    {
        if (hasTalked) return;
        text.text = "Donali a la F per parlar amb el client";
        if (Input.GetKeyDown(KeyCode.F))
        {
            hasTalked = true;
            gameFlow.TalkClients();
            text.text = " ";
            FindFirstObjectByType<RockNPC>().StopWaving();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        text.text = " ";
    }
}

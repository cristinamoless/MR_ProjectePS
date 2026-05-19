using UnityEngine;
using System.Collections.Generic;

public enum MRPhase
{
    Placement,
    Shop,
    Clients,
    Crafting
}

public class GameFlowManager : MonoBehaviour
{
    public MRPhase currentMRPhase = MRPhase.Placement;

    public GameObject placementUI;

    public DadesComanda database;
    public OrderDisplay uiOrder;
    public BuyFlower buyFlower;
    public ComandaArea comandaArea;

    public GameObject repartidor;
    public GameObject dialeg;
    public GameObject fiDia;
    public GameObject date;

    public Comanda currentComanda;
    public int currentDay = 1;
    private int comandaIndex = 0;

    public DialogueManager dialogueManager;
    public DialogueManager dialogueManagerRepartidor;
    public Dialogue currentDialogue;
    public Dialogue[] allDialogues;

    public CustomCursor cc;
    public bool lastOrderWasCorrect;
    private bool waitingForFinalDialogue = false;

    public List<CompletedOrderInfo> completedOrders = new List<CompletedOrderInfo>();

    void Start()
    {
        placementUI.SetActive(true);

        repartidor.SetActive(false);
        dialeg.SetActive(false);
        fiDia.SetActive(false);
        date.SetActive(false);
    }

    public void StartShopPhase()
    {
        currentMRPhase = MRPhase.Shop;

        placementUI.SetActive(false);

        buyFlower.showFlowers();
    }

    public void StartClientsPhase()
    {
        currentMRPhase = MRPhase.Clients;
        StartDay();
    }

    public void StartCraftingPhase()
    {
        currentMRPhase = MRPhase.Crafting;
        Debug.Log("Comença la fase de muntar el ram en 3D!");
    }

    public void StartDay()
    {
        currentDay++;
        comandaIndex = 0;

        date.SetActive(true);
        fiDia.SetActive(false);
        repartidor.SetActive(true);

        currentDialogue = GetDialogue(currentDay, 0, DialogueType.Repartidor);
        dialogueManagerRepartidor.StartDialogue(currentDialogue);
    }

    public void BeginClients()
    {
        repartidor.SetActive(false);
    }

    public void TalkClients()
    {
        dialeg.SetActive(true);
        dialogueManager.isDialogueInici = true;
        currentDialogue = GetDialogue(currentDay, comandaIndex, DialogueType.Initial);
        dialogueManager.StartDialogue(currentDialogue);
    }

    public void GetComanda()
    {
        dialeg.SetActive(false);
        if (dialogueManager.isDialogueInici)
        {
            if (currentDay == 1)
                currentComanda = database.day1Orders[comandaIndex];
            else
                currentComanda = database.day2Orders[comandaIndex];

            uiOrder.ShowOrder(currentComanda);
        }
    }

    public void OnOrderConfirmed()
    {
        bool correct = lastOrderWasCorrect;

        var list = currentDay == 1 ? database.day1Orders : database.day2Orders;

        completedOrders.Add(new CompletedOrderInfo
        {
            comanda = list[comandaIndex],
            wasCorrect = correct
        });

        comandaIndex++;
        currentComanda = null;
        uiOrder.ClearUI();

        Dialogue result = null;

        if (correct)
        {
            result = GetDialogue(currentDay, comandaIndex - 1, DialogueType.Choice);

            if (result == null)
                result = GetDialogue(currentDay, comandaIndex - 1, DialogueType.Happy);
        }
        else
        {
            result = GetDialogue(currentDay, comandaIndex - 1, DialogueType.Sad);
        }

        dialogueManager.isDialogueInici = false;
        dialeg.SetActive(true);
        dialogueManager.StartDialogue(result);

        if (comandaIndex >= list.Count)
        {
            waitingForFinalDialogue = true;
        }

        comandaArea.hasTalked = false;
        cc.SetCursor();
    }

    public void OnDialogueEnded()
    {
        if (!dialogueManager.isDialogueInici && !waitingForFinalDialogue)
        {
            var npcManager = FindFirstObjectByType<NPCManager>();
            if (npcManager != null)
                npcManager.MakeCurrentClientLeave();
        }

        if (waitingForFinalDialogue)
        {
            fiDia.SetActive(true);
            date.SetActive(false);
            uiOrder.ShowEndOfDay(completedOrders);
            waitingForFinalDialogue = false;

            var timeManager = FindFirstObjectByType<TimeManager>();
            timeManager.currentDayIndex++;
            timeManager.SetDay(timeManager.currentDayIndex);
            timeManager.ResetDay();
        }
    }

    public Dialogue GetDialogue(int day, int index, DialogueType type)
    {
        foreach (Dialogue d in allDialogues)
        {
            if (d.day == day && d.orderIndex == index && d.type == type)
                return d;
        }

        return null;
    }
}

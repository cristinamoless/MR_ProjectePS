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
    public GameObject toDo;
    public GameObject notEnough;

    public Comanda currentComanda;
    public int currentDay = 0; // Comença a 0 perquè StartDay() fa el ++
    private int comandaIndex = 0;
    public bool lastOrderWasCorrect;
    private bool waitingForFinalDialogue = false;
    public List<CompletedOrderInfo> completedOrders = new List<CompletedOrderInfo>();

    public DialogueManager dialogueManager;
    public DialogueManager dialogueManagerRepartidor;
    public Dialogue currentDialogue;
    public Dialogue[] allDialogues;


    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        currentMRPhase = MRPhase.Placement;
        
        placementUI.SetActive(true);
        repartidor.SetActive(false);
        dialeg.SetActive(false);
        fiDia.SetActive(false);
        date.SetActive(false);
        toDo.SetActive(false);
        notEnough.SetActive(false);
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
        toDo.SetActive(false);
        dialeg.SetActive(false);
    }


    public void StartDay()
    {
        currentDay++;
        comandaIndex = 0;

        date.SetActive(true);
        fiDia.SetActive(false);
        toDo.SetActive(false);
        repartidor.SetActive(true);

        currentDialogue = GetDialogue(currentDay, 0, DialogueType.Repartidor);
        if (currentDialogue != null && dialogueManagerRepartidor != null)
        {
            dialogueManagerRepartidor.StartDialogue(currentDialogue);
        }
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
            var list = currentDay == 1 ? database.day1Orders : database.day2Orders;
            
            if (comandaIndex < list.Count)
            {
                currentComanda = list[comandaIndex];
                uiOrder.ShowOrder(currentComanda);
                toDo.SetActive(true);

                StartCraftingPhase();
            }
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
        toDo.SetActive(false);

        Dialogue result = null;
        if (correct)
        {
            result = GetDialogue(currentDay, comandaIndex - 1, DialogueType.Choice) ?? 
                     GetDialogue(currentDay, comandaIndex - 1, DialogueType.Happy);
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
        
        currentMRPhase = MRPhase.Clients;
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
            EndDay();
        }
    }

    public void EndDay()
    {
        fiDia.SetActive(true);
        date.SetActive(false);
        dialeg.SetActive(false);
        uiOrder.ShowEndOfDay(completedOrders);
        waitingForFinalDialogue = false;

        bool hasEnoughStars = PlayerStars.Instance.totalStars >= GetMinimumStarsForNextDay();

        if (!hasEnoughStars)
        {
            notEnough.SetActive(true);
            currentDay--; 
            return;
        }

        var timeManager = FindFirstObjectByType<TimeManager>();
        timeManager.ResetDay();
        StartShopPhase();
    }

    private int GetMinimumStarsForNextDay()
    {
        int total = 0;
        foreach (var flower in buyFlower.allFlowers)
        {
            if (flower.availableDay == currentDay + 1 && !flower.unlocked)
                total += flower.seedPrice;
        }
        return total;
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

    public void Update()
    {
        if (notEnough.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                notEnough.SetActive(false);
                StartDay(); 
            }
        }
    }
}
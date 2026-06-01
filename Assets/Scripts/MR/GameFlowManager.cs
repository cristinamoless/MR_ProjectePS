using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameFlowManager : MonoBehaviour
{
    public DadesComanda database;
    public OrderDisplay uiOrder;
    public BuyFlower buyFlower;
    public NPCManager npcManager;

    [Header("--- UI Windows & Screens ---")]
    public GameObject placementUI;
    public GameObject repartidor;
    public GameObject dialeg;
    public GameObject fiDia;
    public GameObject date;
    public GameObject toDo;
    public GameObject notEnough;

    public Comanda currentComanda;
    public int currentDay = 1;
    private int comandaIndex = 0;

    public DialogueManager dialogueManager;
    public Dialogue currentDialogue;
    public Dialogue[] allDialogues;

    public bool lastOrderWasCorrect;
    private bool waitingForFinalDialogue = false;

    public List<CompletedOrderInfo> completedOrders = new List<CompletedOrderInfo>();

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        currentDay = 0;

        if (placementUI != null) placementUI.SetActive(true);

        repartidor.SetActive(false);
        dialeg.SetActive(false);
        fiDia.SetActive(false);
        date.SetActive(false);
        toDo.SetActive(false);
        notEnough.SetActive(false);
    }

    public void StartDay()
    {
        currentDay++;
        comandaIndex = 0;

        if (placementUI != null) placementUI.SetActive(false);

        date.SetActive(true);
        fiDia.SetActive(false);
        toDo.SetActive(false);
        buyFlower.showFlowers();
        repartidor.SetActive(true);

        if (dialogueManager != null && dialogueManager.botoParlarClient != null)
        {
            dialogueManager.botoParlarClient.SetActive(false);
        }

        currentDialogue = GetDialogue(currentDay, 0, DialogueType.Repartidor);
        if (currentDialogue != null && dialogueManager != null)
        {
            dialeg.SetActive(true);
            dialogueManager.StartDialogue(currentDialogue);
        }
    }

    public void BeginClients()
    {
        repartidor.SetActive(false);
        dialeg.SetActive(false);
        if (npcManager != null)
        {
            npcManager.ShowClient(comandaIndex);
        }

        if (dialogueManager != null && dialogueManager.botoParlarClient != null)
        {
            dialogueManager.botoParlarClient.SetActive(true);
        }
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
            toDo.SetActive(true);

            var comandaManager = FindFirstObjectByType<ComandaManager>();
            if (comandaManager != null)
            {
                comandaManager.SetCurrentComanda(currentComanda);
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
    }

    public void OnDialogueEnded()
    {
        if (!dialogueManager.isDialogueInici)
        {
            dialeg.SetActive(false); 

            if (npcManager != null)
            {
                npcManager.MakeCurrentClientLeave(() =>
                {
                    if (waitingForFinalDialogue)
                    {
                        EndDay();
                    }
                    else
                    {
                        npcManager.ShowClient(comandaIndex);
                        if (dialogueManager != null && dialogueManager.botoParlarClient != null)
                        {
                            dialogueManager.botoParlarClient.SetActive(true);
                        }
                    }
                });
            }
            else
            {
                if (waitingForFinalDialogue) EndDay();
                else if (dialogueManager?.botoParlarClient != null) dialogueManager.botoParlarClient.SetActive(true);
            }
        }
    }

    public void EndDay()
    {
        fiDia.SetActive(true);
        date.SetActive(false);
        dialeg.SetActive(false);

        if (dialogueManager != null && dialogueManager.botoParlarClient != null)
        {
            dialogueManager.botoParlarClient.SetActive(false);
        }

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
            }
        }
    }
}
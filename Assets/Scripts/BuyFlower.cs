using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BuyFlower : MonoBehaviour
{
    public List<FlowerType> allFlowers;
    public GameFlowManager gfm;

    [System.Serializable]
    public class FlowerButton
    {
        public FlowerType flower;
        public GameObject button;
    }

    public List<FlowerButton> flowerButtons;

    [Header("--- UI Elements ---")]
    public GameObject marcComprar;
    public GameObject perComprar;
    public GameObject comprat;
    public TMP_Text confirmText;
    public TMP_Text resultText;

    [Header("--- VR Continue Button ---")]
    public GameObject botoContinuar;

    private FlowerType selectedFlower;

    void Start()
    {
        if (botoContinuar != null) botoContinuar.SetActive(false);
    }

    public void showFlowers()
    {
        List<FlowerType> available = new List<FlowerType>();

        foreach (var flower in allFlowers)
        {
            if (flower.availableDay == gfm.currentDay && !flower.unlocked)
                available.Add(flower);
        }

        foreach (var fb in flowerButtons)
        {
            fb.button.SetActive(available.Contains(fb.flower));
        }

        ComprovarBotóContinuar();
    }

    public void AskToBuy(FlowerType flower)
    {
        selectedFlower = flower;

        marcComprar.SetActive(true);
        perComprar.SetActive(true);
        comprat.SetActive(false);

        confirmText.text =
            $"Vols comprar llavors de {flower.flowerName} per {flower.seedPrice} estrelles?";
    }

    public void CancelBuy()
    {
        marcComprar.SetActive(false);
        perComprar.SetActive(false);
        comprat.SetActive(false);
    }

    public void ConfirmBuy()
    {
        perComprar.SetActive(false);
        comprat.SetActive(true);

        if (PlayerStars.Instance.totalStars >= selectedFlower.seedPrice)
        {
            PlayerStars.Instance.totalStars -= selectedFlower.seedPrice;
            selectedFlower.unlocked = true;

            resultText.text = "COMPRAT!";
            showFlowers();
        }
        else
        {
            resultText.text = "No tens prou estrelles!";
        }
    }

    public void TancarCartellResultat()
    {
        comprat.SetActive(false);
        marcComprar.SetActive(false);
    }

    private void ComprovarBotóContinuar()
    {
        if (botoContinuar == null) return;

        if (CheckSiNoQuedanFlores())
        {
            botoContinuar.SetActive(true);
        }
        else
        {
            botoContinuar.SetActive(false);
        }
    }

    public void PremutBotoContinuar()
    {
        if (botoContinuar != null) botoContinuar.SetActive(false);

        comprat.SetActive(false);
        marcComprar.SetActive(false);

        gfm.BeginClients();
    }

    private bool CheckSiNoQuedanFlores()
    {
        foreach (var flower in allFlowers)
        {
            if (flower.availableDay == gfm.currentDay && !flower.unlocked)
            {
                return false;
            }
        }
        return true;
    }
}
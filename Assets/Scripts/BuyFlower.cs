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

    public GameObject marcComprar;
    public GameObject perComprar;
    public GameObject comprat;
    public TMP_Text confirmText;
    public TMP_Text resultText;

    public GameObject continueButton;

    private FlowerType selectedFlower;

    public void showFlowers()
    {
        foreach (var fb in flowerButtons)
        {
            bool hauriaDeSerVisible = false;

            foreach (var flower in allFlowers)
            {
                if (flower.flowerName == fb.flower.flowerName)
                {
                    if (flower.availableDay <= gfm.currentDay && !flower.unlocked)
                    {
                        hauriaDeSerVisible = true;
                    }
                    break;
                }
            }

            fb.button.SetActive(hauriaDeSerVisible);
        }
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

            foreach (var flower in allFlowers)
            {
                if (flower.flowerName == selectedFlower.flowerName)
                {
                    flower.unlocked = true;
                }
            }

            foreach (var fb in flowerButtons)
            {
                if (fb.flower.flowerName == selectedFlower.flowerName)
                {
                    fb.flower.unlocked = true;
                }
            }

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

    public bool HasBoughtFlowersForDay(int day)
    {
        foreach (var flower in allFlowers)
        {
            if (flower.availableDay == day)
            {
                if (!flower.unlocked)
                    return false;
            }
        }

        return true;
    }

    void Update()
    {
        bool allBought = HasBoughtFlowersForDay(gfm.currentDay);
        continueButton.SetActive(allBought);
    }
}
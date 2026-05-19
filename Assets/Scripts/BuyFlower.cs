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

    private FlowerType selectedFlower;

    public void showFlowers()
    {
        List<FlowerType> available = new List<FlowerType>();

        foreach (var flower in allFlowers)
        {
            if (flower.availableDay <= gfm.currentDay && !flower.unlocked)
                available.Add(flower);
        }

        foreach (var fb in flowerButtons)
        {
            fb.button.SetActive(available.Contains(fb.flower));
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

            resultText.text = "COMPRAT!";
            showFlowers(); 
        }
        else
        {
            resultText.text = "No tens prou estrelles!";
        }
    }
    void Update()
    {
        if (comprat.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                comprat.SetActive(false);
                marcComprar.SetActive(false);
            }
        }
    }

}

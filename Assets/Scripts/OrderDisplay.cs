using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class OrderDisplay : MonoBehaviour
{
    public TMP_Text comandaText;
    public TMP_Text fiDiaText;
    public TMP_Text starsText;
    private int stars = 0;

    public void ShowOrder(Comanda c)
    {
        comandaText.text = c.nomComanda;
    }

    public void ClearUI()
    {
        comandaText.text = "";
    }

    public void ShowEndOfDay(List<CompletedOrderInfo> completedOrders)
    {
        fiDiaText.text = "";
       
        foreach (var info in completedOrders)
        {
            string symbol = info.wasCorrect ? "Completada" : "No Completada";
            if (info.wasCorrect)
                stars++;

            fiDiaText.text +=
            info.comanda.nomComanda + " - " + symbol + "\n";
            starsText.text = "" + stars;
        }
    }
}

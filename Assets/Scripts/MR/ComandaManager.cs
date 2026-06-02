using System.Collections.Generic;
using UnityEngine;

public class ComandaManager : MonoBehaviour
{
    public TableManager table;
    public GameObject botoConfirmarRam;

    private GameFlowManager flow;
    public Comanda currentComanda;

    void Start()
    {
        flow = FindFirstObjectByType<GameFlowManager>();

        if (flow != null && flow.currentComanda != null)
        {
            currentComanda = flow.currentComanda;
        }

        if (botoConfirmarRam != null)
        {
            botoConfirmarRam.SetActive(false);
        }
    }

    void Update()
    {
        if (flow == null)
        {
            flow = FindFirstObjectByType<GameFlowManager>();
        }

        if (flow != null)
        {
            currentComanda = flow.currentComanda;
        }
        else
        {
            currentComanda = null;
        }

        if (table == null)
        {
            table = FindFirstObjectByType<TableManager>();
        }

        if (currentComanda != null && table != null)
        {
            List<FlowerType> tableFlowers = table.GetFlowersOnTable();
            bool bouquetDone = tableFlowers.Count >= currentComanda.requiredFlowers.Count;

            if (botoConfirmarRam != null && botoConfirmarRam.activeSelf != bouquetDone)
            {
                botoConfirmarRam.SetActive(bouquetDone);
            }
        }
        else
        {
            if (botoConfirmarRam != null && botoConfirmarRam.activeSelf)
            {
                botoConfirmarRam.SetActive(false);
            }
        }
    }

    public void SetCurrentComanda(Comanda nuevaComanda)
    {
        currentComanda = nuevaComanda;
        if (botoConfirmarRam != null)
        {
            botoConfirmarRam.SetActive(false);
        }
    }

    public bool CheckOrder()
    {
        if (currentComanda == null) return false;

        List<FlowerType> tableFlowers = table.GetFlowersOnTable();

        if (tableFlowers.Count != currentComanda.requiredFlowers.Count)
            return false;

        List<FlowerType> temp = new List<FlowerType>(tableFlowers);

        foreach (FlowerType req in currentComanda.requiredFlowers)
        {
            bool found = false;

            for (int i = temp.Count - 1; i >= 0; i--)
            {
                if (temp[i].name == req.name)
                {
                    found = true;
                    temp.RemoveAt(i); 
                    break;
                }
            }

            if (!found)
                return false;
        }

        return true;
    }

    public void ConfirmOrder()
    {
        if (currentComanda == null) return;

        if (table == null)
        {
            table = FindFirstObjectByType<TableManager>();
        }

        bool correct = CheckOrder();
        flow.lastOrderWasCorrect = correct;

        if (correct)
        {
            int reward = currentComanda.reward;
            PlayerStars.Instance.addStars(reward);
        }

        if (botoConfirmarRam != null)
        {
            botoConfirmarRam.SetActive(false);
        }

        if (flow != null)
        {
            flow.currentComanda = null;
        }
        currentComanda = null;

        if (table != null)
        {
            table.ClearTable();
        }
        
        flow.OnOrderConfirmed();
    }
}
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

        if (botoConfirmarRam != null)
        {
            botoConfirmarRam.SetActive(false);
        }
    }

    public void SetCurrentComanda(Comanda nuevaComanda)
    {
        currentComanda = nuevaComanda;

        if (currentComanda != null)
        {
            MostrarBotoConfirmar(true);
        }
    }

    public void MostrarBotoConfirmar(bool activar)
    {
        if (botoConfirmarRam != null)
        {
            botoConfirmarRam.SetActive(activar);
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

            foreach (FlowerType f in temp)
            {
                if (f.name == req.name)
                {
                    found = true;
                    temp.Remove(f);
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

        bool correct = CheckOrder();
        flow.lastOrderWasCorrect = correct;

        if (correct)
        {
            int reward = currentComanda.reward;
            PlayerStars.Instance.addStars(reward);
        }

        MostrarBotoConfirmar(false);
        currentComanda = null;

        table.ClearTable();
        flow.OnOrderConfirmed();
    }
}
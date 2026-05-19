using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class ComandaManager : MonoBehaviour
{
    public TableManager table;

    private GameFlowManager flow;
    public Comanda currentComanda;

    void Start()
    {
        flow = FindFirstObjectByType<GameFlowManager>();
        currentComanda = flow.currentComanda;
    }

    public bool CheckOrder()
    {
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
       bool correct = CheckOrder();
        flow.lastOrderWasCorrect = correct;

        if (correct)
        {
            int reward = currentComanda.reward;
            PlayerStars.Instance.addStars(reward);
        }
        SceneManager.UnloadSceneAsync("BuildFlower");
        table.ClearTable();
        flow.OnOrderConfirmed();
    
    }

}

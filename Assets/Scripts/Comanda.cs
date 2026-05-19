using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class CompletedOrderInfo
{
    public Comanda comanda;
    public bool wasCorrect;
}

[System.Serializable]
public class Comanda
{
    public string nomComanda;
    public string clientName;
    public List<FlowerType> requiredFlowers;
    public int reward;
}

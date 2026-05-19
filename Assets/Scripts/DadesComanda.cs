using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OrderDatabase", menuName = "Game/OrderDatabase")]
public class DadesComanda : ScriptableObject
{
    public List<Comanda> day1Orders;
    public List<Comanda> day2Orders;
}

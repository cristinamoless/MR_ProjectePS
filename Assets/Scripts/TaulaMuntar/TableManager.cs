using System.Collections.Generic;
using UnityEngine;

public class TableManager : MonoBehaviour
{
    public Transform workArea; 

    public List<FlowerType> GetFlowersOnTable()
    {
        List<FlowerType> result = new List<FlowerType>();

        foreach (Transform child in workArea)
        {
            Flower f = child.GetComponent<Flower>();
            if (f != null)
                result.Add(f.flowerType);
        }

        return result;
    }

    public void ClearTable()
    {
        foreach (Transform child in workArea)
            Destroy(child.gameObject);
    }
}


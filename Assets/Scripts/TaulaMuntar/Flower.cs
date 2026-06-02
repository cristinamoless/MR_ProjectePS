using UnityEngine;
using UnityEngine.UI;

public class Flower : MonoBehaviour
{
    private Image img;
    public FlowerType flowerType;

    void Awake()
    {
        img = GetComponent<Image>();
        if (img != null && flowerType != null)
        {
            img.sprite = flowerType.withLeaves;
        }
    }

    public void RemoveLeaves()
    {
        if (img != null && flowerType != null)
            img.sprite = flowerType.withoutLeaves;
    }

    public void Rotate(float angle)
    {
        RectTransform rt = GetComponent<RectTransform>();
        if (rt != null)
        {
            rt.Rotate(0, 0, angle);
        }
    }
}

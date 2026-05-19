using UnityEngine;
using UnityEngine.EventSystems;

public class FlowerSource : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject flowerPrefab;
    public RectTransform tableArea;

    private RectTransform currentFlower;

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (ToolManager.activeTool != ToolManager.ToolType.Hand) return;

        GameObject clone = Instantiate(flowerPrefab, tableArea);

        FlowerSource fs = clone.GetComponent<FlowerSource>();
        if (fs != null)
            Destroy(fs);

        if (clone.GetComponent<DragDrop>() == null)
            clone.AddComponent<DragDrop>();
        if (clone.GetComponent<EditFlower>() == null)
            clone.AddComponent<EditFlower>();

        currentFlower = clone.GetComponent<RectTransform>();

        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            tableArea,
            Input.mousePosition,
            eventData.pressEventCamera,
            out localPos
        );

        currentFlower.anchoredPosition = localPos;
        currentFlower.SetAsLastSibling();
    }


    public void OnDrag(PointerEventData eventData)
    {
        if (currentFlower != null)
        {
            Vector2 localPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                tableArea,
                Input.mousePosition,
                eventData.pressEventCamera,
                out localPos
            );

            currentFlower.anchoredPosition = localPos;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        currentFlower = null;
    }
}

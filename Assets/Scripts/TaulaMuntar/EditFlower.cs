using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EditFlower : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IPointerClickHandler
{
    private Flower flower;
    private RectTransform rt;

    private bool rotating = false;
    private float lastAngle;

    void Awake()
    {
        flower = GetComponent<Flower>();
        rt = GetComponent<RectTransform>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (ToolManager.activeTool == ToolManager.ToolType.Rotate)
        {
            rotating = true;
            lastAngle = GetMouseAngle(eventData.position);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        rotating = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (rotating && ToolManager.activeTool == ToolManager.ToolType.Rotate)
        {
            float currentAngle = GetMouseAngle(eventData.position);
            float rotateAngle = currentAngle - lastAngle;

            rt.Rotate(0, 0, rotateAngle);

            lastAngle = currentAngle;
        }
    }

    private float GetMouseAngle(Vector2 mousePos)
    {
        Vector2 dir = mousePos - (Vector2)rt.position;
        return Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
    }


public void OnPointerClick(PointerEventData eventData)
    {
        if (ToolManager.activeTool == ToolManager.ToolType.Scissors)
        {
            flower.RemoveLeaves();
        }
        if (ToolManager.activeTool == ToolManager.ToolType.Delete)
        {
            Destroy(gameObject);
        }
    }
}





using UnityEngine;

public class ToolManager : MonoBehaviour
{
    public enum ToolType
    {
        Hand,
        Scissors,
        Rotate,
        Delete
    }

    public static ToolType activeTool = ToolType.Hand;

    public GameObject handCursor;
    public GameObject handClosedCursor;
    public GameObject scissorsCursor;
    public GameObject scissorsClosedCursor;
    public GameObject rotateCursor;
    public GameObject deleteCursor;

    void Start()
    {
        Cursor.visible = false;
        SetCursor(ToolType.Hand);
    }

    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        UpdateCursorPosition(mousePos);
    }
    public void SetCursor(ToolType tool)
    {
        activeTool = tool;

        handCursor.SetActive(tool == ToolType.Hand);
        handClosedCursor.SetActive(false);
        scissorsCursor.SetActive(tool == ToolType.Scissors);
        scissorsClosedCursor.SetActive(false);
        rotateCursor.SetActive(tool == ToolType.Rotate);
        deleteCursor.SetActive(tool == ToolType.Delete);
    }

    void UpdateCursorPosition(Vector3 mousePos)
    {
        bool clicking = Input.GetMouseButton(0);
        switch (activeTool)
        {
            case ToolType.Hand:
                handCursor.SetActive(!clicking);
                handClosedCursor.SetActive(clicking);

                if (clicking)
                    handClosedCursor.transform.position = mousePos;
                else
                    handCursor.transform.position = mousePos;
                break;

            case ToolType.Scissors:
                scissorsCursor.SetActive(!clicking);
                scissorsClosedCursor.SetActive(clicking);

                if (clicking)
                    scissorsClosedCursor.transform.position = mousePos;
                else
                    scissorsCursor.transform.position = mousePos;
                break;

            case ToolType.Rotate:
                rotateCursor.transform.position = mousePos;
                break;

            case ToolType.Delete:
                deleteCursor.transform.position = mousePos;
                break;
        }
    }
    public void ActivateHand() {
        SetCursor(ToolType.Hand);
    }
    public void ActivateScissors() {
        SetCursor(ToolType.Scissors);
    }
    public void ActivateRotate() {
        SetCursor(ToolType.Rotate);
    }
    public void ActivateDelete() {
        SetCursor(ToolType.Delete);
    }

}

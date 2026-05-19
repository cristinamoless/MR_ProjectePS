using UnityEngine;

public class CustomCursor : MonoBehaviour
{ 
    public Texture2D normalCursor;
    public Texture2D clickCursor;
    public Vector2 hotspot = Vector2.zero;

    void Start()
    {
        SetCursor();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Cursor.SetCursor(clickCursor, hotspot, CursorMode.Auto);
        }

        if (Input.GetMouseButtonUp(0))
        {
            Cursor.SetCursor(normalCursor, hotspot, CursorMode.Auto);
        }
    }

    public void SetCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.SetCursor(normalCursor, hotspot, CursorMode.Auto);
    }
}

using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class DuplicateOnGrab3D : MonoBehaviour
{
    public GameObject flowerPrefab;
    private XRGrabInteractable grab;
    private bool hasDuplicated = false;

    void Awake()
    {
        grab = GetComponent<XRGrabInteractable>();
        grab.selectEntered.AddListener(OnGrab);
    }

    void OnDestroy()
    {
        if (grab != null)
        {
            grab.selectEntered.RemoveListener(OnGrab);
        }
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        if (hasDuplicated) return;

        hasDuplicated = true;

        TableManager table = FindFirstObjectByType<TableManager>();
        if (table != null && table.workArea != null)
        {
            transform.SetParent(table.workArea);
        }

        GameObject nextSource = Instantiate(flowerPrefab, transform.position, transform.rotation);

        DuplicateOnGrab3D newScript = nextSource.GetComponent<DuplicateOnGrab3D>();
        if (newScript != null)
        {
            newScript.hasDuplicated = false;
        }
    }
}
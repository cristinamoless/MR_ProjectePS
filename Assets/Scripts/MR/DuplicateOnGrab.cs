using UnityEngine;
using Oculus.Interaction;

public class DuplicateOnGrab : MonoBehaviour
{
    public GameObject flowerPrefab;
    public FlowerType flowerType;
    private Grabbable grabbable;
    private bool hasDuplicated = false;

    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private Transform initialParent;

    void Awake()
    {
        grabbable = GetComponent<Grabbable>();
        if (grabbable == null)
        {
            Debug.LogError($"[DuplicateOnGrab] Grabbable component not found on {gameObject.name}!");
        }
    }

    void Start()
    {
        // Cache the initial stand position, rotation, and parent
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        initialParent = transform.parent;

        if (grabbable != null)
        {
            grabbable.WhenPointerEventRaised += HandlePointerEventRaised;
        }
    }

    void OnDestroy()
    {
        if (grabbable != null)
        {
            grabbable.WhenPointerEventRaised -= HandlePointerEventRaised;
        }
    }

    private void HandlePointerEventRaised(PointerEvent evt)
    {
        // When the hand/controller selects (grabs) the object
        if (evt.Type == PointerEventType.Select)
        {
            OnGrab();
        }
    }

    private void OnGrab()
    {
        if (hasDuplicated) return;

        hasDuplicated = true;
        Debug.Log($"[DuplicateOnGrab] {gameObject.name} grabbed. Duplicating...");

        // 1. Determine the template to clone
        GameObject template = flowerPrefab;
        if (template == null || template == gameObject)
        {
            template = gameObject;
        }

        // 2. Spawn a new flower source at the cached stand position
        GameObject nextSource = Instantiate(template, initialPosition, initialRotation, initialParent);
        nextSource.name = template.name;

        // Reset the hasDuplicated flag on the next source script so it can be grabbed
        DuplicateOnGrab newScript = nextSource.GetComponent<DuplicateOnGrab>();
        if (newScript != null)
        {
            newScript.hasDuplicated = false;
        }

        // 3. Ensure the grabbed flower (this object) has the Flower component and assign the type
        Flower f = GetComponent<Flower>();
        if (f == null)
        {
            f = gameObject.AddComponent<Flower>();
        }
        f.flowerType = flowerType;

        // 4. Parent this grabbed flower to the TableManager's workArea
        TableManager table = FindFirstObjectByType<TableManager>();
        if (table != null && table.workArea != null)
        {
            transform.SetParent(table.workArea);
            Debug.Log($"[DuplicateOnGrab] Parented grabbed flower to {table.workArea.name}");
        }
        else
        {
            Debug.LogWarning("[DuplicateOnGrab] TableManager or table.workArea not found in scene!");
        }
    }
}
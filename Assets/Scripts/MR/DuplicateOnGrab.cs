using UnityEngine;
using Oculus.Interaction;

public class DuplicateOnGrab : MonoBehaviour
{
    public GameObject flowerPrefab;
    public FlowerType flowerType;
    private Grabbable grabbable;
    private bool hasDuplicated = false;

    // Guardarem les posicions globals (World) per evitar errors d'escala dels pares
    private Vector3 worldPosition;
    private Quaternion worldRotation;
    private Vector3 worldScale;
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
        // Guardem les coordenades mundials reals d'aquest objecte exacte a l'escena
        worldPosition = transform.position;
        worldRotation = transform.rotation;
        worldScale = transform.lossyScale; // L'escala real al món
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

        // Usem aquest mateix GameObject de l'escena com a plantilla, ja que té l'escala i posició correctes
        GameObject template = gameObject;

        // Instanciem primer al món net (sense pare) per aplicar la transformació real
        GameObject nextSource = Instantiate(template, worldPosition, worldRotation);
        nextSource.name = template.name;

        // Li tornem a assignar el pare original
        nextSource.transform.SetParent(initialParent, true);

        // Forcem l'escala mundial correcta
        nextSource.transform.localScale = transform.localScale;

        DuplicateOnGrab newScript = nextSource.GetComponent<DuplicateOnGrab>();
        if (newScript != null)
        {
            newScript.hasDuplicated = false;
        }

        Flower f = GetComponent<Flower>();
        if (f == null)
        {
            f = gameObject.AddComponent<Flower>();
        }
        f.flowerType = flowerType;

        TableManager table = FindFirstObjectByType<TableManager>();
        if (table != null && table.workArea != null)
        {
            transform.SetParent(table.workArea, true);
            Debug.Log($"[DuplicateOnGrab] Parented grabbed flower to {table.workArea.name}");
        }
        else
        {
            Debug.LogWarning("[DuplicateOnGrab] TableManager or table.workArea not found in scene!");
        }
    }
}
using UnityEngine;
using Meta.XR.BuildingBlocks;

public class SimpleAnchorPlacer : MonoBehaviour
{
    [Header("Meta Building Blocks References")]
    public SpatialAnchorSpawnerBuildingBlock spawner; // Opcional, ja no és necessari per fer el spawn
    public GameObject previewPrefab; // El teu prefab (ex. prefabFinestra) que s'usarà tant per a la preview com per a l'anchor final
    public GameObject botoUICollocar; // <--- Referencia al boto de la UI que activa el mode

    [Header("Rotation Settings")]
    [Tooltip("Offset de rotació en graus (X, Y, Z) per corregir prefabs que vinguin girats de fàbrica o vulguis reorientar.")]
    public Vector3 rotationOffset = Vector3.zero;

    private GameObject previewInstance;
    private GameObject currentAnchor;

    private bool canPlace = false;
    private bool hasPlacedAnchor = false; // <--- Controla si ja hi ha un element a la sala
    private SpatialAnchorCoreBuildingBlock spatialAnchorCore;

    void Awake()
    {
        // Ens suscribim al callback de creacio d'anchors per evitar haver de fer cerques per text ("Spatial Anchor(Clone)")
        spatialAnchorCore = FindFirstObjectByType<SpatialAnchorCoreBuildingBlock>();
        if (spatialAnchorCore != null)
        {
            spatialAnchorCore.OnAnchorCreateCompleted.AddListener(OnAnchorCreated);
        }
        else
        {
            Debug.LogWarning("SimpleAnchorPlacer: SpatialAnchorCoreBuildingBlock no s'ha trobat a l'escena.");
        }
    }

    void OnDestroy()
    {
        if (spatialAnchorCore != null)
        {
            spatialAnchorCore.OnAnchorCreateCompleted.RemoveListener(OnAnchorCreated);
        }
    }

    void Start()
    {
        if (previewPrefab != null)
        {
            previewInstance = Instantiate(previewPrefab);
            previewInstance.SetActive(false);
        }
        else
        {
            Debug.LogWarning("SimpleAnchorPlacer: previewPrefab es null! Si us plau, assigna'l a l'inspector.");
        }
    }

    void Update()
    {
        if (!canPlace)
        {
            if (previewInstance != null) previewInstance.SetActive(false);
            return;
        }

        Transform cam = Camera.main.transform;
        Vector3 pos = cam.position + cam.forward * 1.5f;

        if (previewInstance != null)
        {
            previewInstance.SetActive(true);
            previewInstance.transform.position = pos;

            // Orientem la preview perquè miri cap a l'usuari (mantenint-se perfectament dreta a l'eix Y)
            // de manera que s'alinei de forma natural de cara a tu.
            Vector3 forward = cam.forward;
            forward.y = 0; // Evita inclinacions cap a dalt o cap a baix
            if (forward.sqrMagnitude > 0.001f)
            {
                Quaternion lookRotation = Quaternion.LookRotation(forward.normalized, Vector3.up);
                
                // Apliquem l'offset de rotació per corregir pivotacions del model 3D
                previewInstance.transform.rotation = lookRotation * Quaternion.Euler(rotationOffset);
            }
        }

        // Mantenim OVRInput com a fallback, encara que normalment no funcionara si s'usa OpenXR/New Input System sense OVRManager
        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            PlaceAnchorAtCurrentPosition();
        }
    }

    public void EnablePlacement()
    {
        // Si ja hem col-locat l'element definitiu, no deixem tornar a activar el mode
        if (hasPlacedAnchor) return;

        canPlace = true;
        if (previewInstance != null) previewInstance.SetActive(true);

        // Amaguem el boto de la UI temporalment mentre l'usuari esta buscant lloc
        if (botoUICollocar != null) botoUICollocar.SetActive(false);
    }

    public void DisablePlacement()
    {
        canPlace = false;
        if (previewInstance != null) previewInstance.SetActive(false);

        // Si l'usuari cancel-la (o no ha col-locat res), tornem a mostrar el boto. 
        // Si ja l'ha col-locat, el boto es queda apagat per sempre.
        if (botoUICollocar != null)
        {
            botoUICollocar.SetActive(!hasPlacedAnchor);
        }
    }

    /// <summary>
    /// Metode public per poder ser cridat des de el 'Controller Buttons Mapper' Building Block de Meta
    /// quan es prem el boto "A" (o qualsevol altre boto configurat).
    /// </summary>
    public void PlaceAnchorAtCurrentPosition()
    {
        if (!canPlace) return;

        Vector3 pos = previewInstance != null 
            ? previewInstance.transform.position 
            : (Camera.main.transform.position + Camera.main.transform.forward * 1.5f);

        PlaceAnchor(pos);
    }

    void PlaceAnchor(Vector3 position)
    {
        if (spatialAnchorCore == null)
        {
            Debug.LogError("SimpleAnchorPlacer: El 'spatialAnchorCore' es null! No es pot col-locar l'anchor.");
            return;
        }

        if (previewPrefab == null)
        {
            Debug.LogError("SimpleAnchorPlacer: El 'previewPrefab' es null! No es pot col-locar l'anchor.");
            return;
        }

        // Capturem la rotació exacta que té la preview actualment a l'escena (la qual ja inclou el rotationOffset)
        Quaternion rotation = previewInstance != null ? previewInstance.transform.rotation : Quaternion.identity;

        // Fem el spawn del persistent spatial anchor DIRECTAMENT usant el Spatial Anchor Core
        // amb la mateixa posició i la rotació exacta que veus en la preview!
        spatialAnchorCore.InstantiateSpatialAnchor(previewPrefab, position, rotation);

        // MARQUEM QUE JA ESTA COL-LOCAT PER SEMPRE
        hasPlacedAnchor = true;

        // CRIDEM AL DISABLE PER TANCAR EL MODE AUTOMATICAMENT
        DisablePlacement();

        // OPCIONAL: Avisar al GameFlowManager que la taula ja esta a l'escena
        // per que pugui activar el seguent pas del joc (per exemple, comencar el dia)
        var flow = FindFirstObjectByType<GameFlowManager>();
        if (flow != null)
        {
            // flow.StartDay(); o el metode que facis servir per avancar
        }
    }

    private void OnAnchorCreated(OVRSpatialAnchor anchor, OVRSpatialAnchor.OperationResult result)
    {
        if (result == OVRSpatialAnchor.OperationResult.Success)
        {
            if (currentAnchor != null)
            {
                Destroy(currentAnchor);
            }
            currentAnchor = anchor.gameObject;
            Debug.Log($"SimpleAnchorPlacer: Anchor espacial creat correctament: {anchor.name}");
        }
        else
        {
            Debug.LogError($"SimpleAnchorPlacer: Error en crear l'anchor espacial: {result}");
        }
    }
}

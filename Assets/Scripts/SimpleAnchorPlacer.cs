using UnityEngine;
using Meta.XR.BuildingBlocks;

public class SimpleAnchorPlacer : MonoBehaviour
{
    public SpatialAnchorSpawnerBuildingBlock spawner;
    public GameObject previewPrefab;
    public GameObject botoUICollocar; // <--- Referència al botó de la UI que activa el mode

    private GameObject previewInstance;
    private GameObject currentAnchor;

    private bool canPlace = false;
    private bool hasPlacedAnchor = false; // <--- Controla si ja hi ha un element a la sala

    void Start()
    {
        previewInstance = Instantiate(previewPrefab);
        previewInstance.SetActive(false);
    }

    void Update()
    {
        if (!canPlace)
        {
            previewInstance.SetActive(false);
            return;
        }

        Transform cam = Camera.main.transform;
        Vector3 pos = cam.position + cam.forward * 1.5f;

        previewInstance.SetActive(true);
        previewInstance.transform.position = pos;

        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            PlaceAnchor(pos);
        }
    }

    public void EnablePlacement()
    {
        // Si ja hem col·locat l'element definitiu, no deixem tornar a activar el mode
        if (hasPlacedAnchor) return;

        canPlace = true;
        previewInstance.SetActive(true);

        // Amaguem el botó de la UI temporalment mentre l'usuari està buscant lloc
        if (botoUICollocar != null) botoUICollocar.SetActive(false);
    }

    public void DisablePlacement()
    {
        canPlace = false;
        if (previewInstance != null) previewInstance.SetActive(false);

        // Si l'usuari cancel·la (o no ha col·locat res), tornem a mostrar el botó. 
        // Si ja l'ha col·locat, el botó es queda apagat per sempre.
        if (botoUICollocar != null)
        {
            botoUICollocar.SetActive(!hasPlacedAnchor);
        }
    }

    void PlaceAnchor(Vector3 position)
    {
        if (currentAnchor != null)
        {
            Destroy(currentAnchor);
        }

        spawner.SpawnSpatialAnchor(position, Quaternion.identity);

        currentAnchor = GameObject.Find("Spatial Anchor(Clone)");

        // MARQUEM QUE JA ESTÀ COL·LOCAT PER SEMPRE
        hasPlacedAnchor = true;

        // CRIDEM AL DISABLE PER TANCAR EL MODE AUTOMÀTICAMENT
        DisablePlacement();

        // OPCIONAL: Avisar al GameFlowManager que la taula ja està a l'escena
        // perquè pugui activar el següent pas del joc (per exemple, començar el dia)
        var flow = FindFirstObjectByType<GameFlowManager>();
        if (flow != null)
        {
            // flow.StartDay(); o el mètode que facis servir per avançar
        }
    }
}
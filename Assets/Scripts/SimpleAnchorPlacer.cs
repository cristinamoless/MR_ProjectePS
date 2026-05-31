using UnityEngine;
using Meta.XR.BuildingBlocks;

public class SimpleAnchorPlacer : MonoBehaviour
{
    public SpatialAnchorSpawnerBuildingBlock spawner;

    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.One)) // botón A
        {
            Transform cam = Camera.main.transform;

            Vector3 position = cam.position + cam.forward * 1.5f;
            Quaternion rotation = Quaternion.identity;

            spawner.SpawnSpatialAnchor(position, rotation);
        }
    }
}
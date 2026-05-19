using UnityEngine;

public class TableSetup : MonoBehaviour
{
    public Transform flowerSpawnPoint;
    public Transform scissorsSpawnPoint;

    public GameObject flowerPrefab;
    public GameObject scissorsPrefab;

    void Start()
    {
        SpawnObjects();
    }

    void SpawnObjects()
    {
        GameObject flower = Instantiate(
            flowerPrefab,
            flowerSpawnPoint.position,
            flowerSpawnPoint.rotation,
            flowerSpawnPoint
        );

        GameObject scissors = Instantiate(
            scissorsPrefab,
            scissorsSpawnPoint.position,
            scissorsSpawnPoint.rotation,
            scissorsSpawnPoint
        );

        PreparePhysics(flower);
        PreparePhysics(scissors);
    }

    void PreparePhysics(GameObject obj)
    {
        Rigidbody rb = obj.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }
    }
}
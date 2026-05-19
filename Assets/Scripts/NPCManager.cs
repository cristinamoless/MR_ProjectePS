using UnityEngine;

public class NPCManager : MonoBehaviour
{
    public GameObject[] clients;   // Rock, Gemma, Marc, Maria...
    public Transform spawnPoint;
    public TimeManager timeManager;
    public int[] clientHours = { 10, 14, 17 };
    
    private int currentClientIndex = 0;
    private GameObject currentClient;

    void Start()
    {
        SpawnNextClient();
    }

    public void SpawnNextClient()
    {
        if (currentClient != null)
            Destroy(currentClient);

        if (currentClientIndex >= clients.Length)
            return;

        timeManager.SetTime(clientHours[currentClientIndex]);

        currentClient = Instantiate(clients[currentClientIndex], spawnPoint.position, Quaternion.identity);
        currentClient.transform.rotation = Quaternion.Euler(0, 180, 0);

        currentClientIndex++;
    }

    public void MakeCurrentClientLeave()
    {
        var rock = currentClient.GetComponent<RockNPC>();
        if (rock != null)
        {
            rock.LeaveShop();
            Invoke(nameof(SpawnNextClient), 2f); // espera que surti
        }
        else
        {
            Destroy(currentClient);
            SpawnNextClient();
        }
    }
}

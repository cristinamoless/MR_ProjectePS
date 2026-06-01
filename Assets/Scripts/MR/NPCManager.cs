using UnityEngine;

public class NPCManager : MonoBehaviour
{
    public GameObject[] clients;
    private GameObject currentClient;

    void Start()
    {
        foreach (GameObject client in clients)
        {
            if (client != null) client.SetActive(false);
        }
    }

    public void ShowClient(int index)
    {
        if (index < 0 || index >= clients.Length)
            return;

        if (currentClient != null)
            currentClient.SetActive(false);

        currentClient = clients[index];
        if (currentClient != null) currentClient.SetActive(true);
    }

    public void MakeCurrentClientLeave(System.Action onFinished)
    {
        if (currentClient == null)
        {
            onFinished?.Invoke();
            return;
        }

        RockNPC rock = currentClient.GetComponent<RockNPC>();

        if (rock != null)
        {
            rock.OnExitFinished = onFinished;
            rock.LeaveShop();
        }
        else
        {
            currentClient.SetActive(false);
            onFinished?.Invoke();
        }
    }
}
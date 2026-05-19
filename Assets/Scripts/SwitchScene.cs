using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SwitchScene : MonoBehaviour
{
    public string build = "BuildFlower";
    public string custom = "CreacioPersonatge";
    public string shop = "Floristeria";
    public string start = "MenuInicial";
    public TMP_Text text;

    private void Start()
    {
        text.text = " ";
    }
    private void OnTriggerStay(Collider other)
    {
        text.text = "Donali a la F si vols muntar flors";
        if (Input.GetKeyDown(KeyCode.F))
        {
            SceneManager.LoadScene(build, LoadSceneMode.Additive);

        }
    }
    private void OnTriggerExit(Collider other)
    {
        text.text = " ";
    }

    public void shopScene()
    {
        SceneManager.LoadScene(shop);
    }
    public void closeBuild()
    {
        SceneManager.UnloadSceneAsync(build);
    }
    public void creacioScene()
    {
        SceneManager.LoadScene(custom);
    }
}

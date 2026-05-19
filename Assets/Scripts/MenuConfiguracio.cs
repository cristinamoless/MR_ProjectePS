using UnityEngine;

public class MenuConfiguracio : MonoBehaviour
{
    public GameObject panelConfiguracio;
    public GameObject Titol;
    public GameObject Button;
    public GameObject ToDo;

    void Start()
    {
        panelConfiguracio.SetActive(false);
        Titol.SetActive(true);
        Button.SetActive(true);
        ToDo.SetActive(true);
    }

    public void ToggleConfiguracio()
    {
        bool actiu = panelConfiguracio.activeSelf;
        panelConfiguracio.SetActive(!actiu);
        Titol.SetActive(actiu);
        Button.SetActive(actiu);
        ToDo.SetActive(actiu);

    }
    public void TancarConfiguracio()
    {
        panelConfiguracio.SetActive(false);
        Titol.SetActive(true);
        Button.SetActive(true);
        ToDo.SetActive(true);

    }
}
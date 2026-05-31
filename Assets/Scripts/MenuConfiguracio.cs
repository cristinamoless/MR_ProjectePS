using UnityEngine;

public class MenuConfiguracio : MonoBehaviour
{
    public GameObject panelConfiguracio;

    void Start()
    {
        panelConfiguracio.SetActive(false);
    }

    public void ToggleConfiguracio()
    {
        bool actiu = panelConfiguracio.activeSelf;
        panelConfiguracio.SetActive(!actiu);
    }
    public void TancarConfiguracio()
    {
        panelConfiguracio.SetActive(false);
    }
}
using UnityEngine;

public class ExitGame : MonoBehaviour
{
    /// <summary>
    /// Tanca el joc. Funciona tant a l'Editor de Unity com a la build final de les Meta Quest (Android).
    /// </summary>
    public void Quit()
    {
        Debug.Log("Surt del joc...");

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}

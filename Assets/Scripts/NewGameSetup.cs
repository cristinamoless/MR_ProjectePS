using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


public class NewGameSetup : MonoBehaviour
{
    public TMP_InputField nameField;
    public TMP_InputField FlowerField;
    public string shop = "Floristeria";

    public void StartGame()
    {
        PlayerPrefs.SetString("playerName", nameField.text);
 
        PlayerPrefs.SetString("favoriteFlower", FlowerField.text);
        SceneManager.LoadScene(shop);

    }
    
    
        
    
}

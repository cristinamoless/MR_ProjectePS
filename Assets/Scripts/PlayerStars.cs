using UnityEngine;

public class PlayerStars : MonoBehaviour
{
    public static PlayerStars Instance;
    public int totalStars = 0;

    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    } 
    public void addStars(int amount)
    {
        totalStars += amount;
    }
}
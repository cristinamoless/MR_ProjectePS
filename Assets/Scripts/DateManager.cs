using UnityEngine;
using TMPro;

public class DateManager : MonoBehaviour
{
    public TMP_Text dayText;
    public TMP_Text starsText;

    void Update()
    {
        int day = FindFirstObjectByType<GameFlowManager>().currentDay;
        dayText.text = GetDayName(day);
        starsText.text = PlayerStars.Instance.totalStars.ToString();
    }


    string GetDayName(int day)
    {
        switch (day)
        {
            case 1: return "DILLUNS";
            case 2: return "DIMARTS";
            default: return "???";
        }
    }
}

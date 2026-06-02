using UnityEngine;
using TMPro;

public class TimeManager : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text timeText;

    [Header("Time")]
    public int currentHour = 10;
    public int currentDayIndex = 0;


    public void SetTime(int hour)
    {
        currentHour = hour;
        timeText.text = hour.ToString("00") + ":00";
    }


}

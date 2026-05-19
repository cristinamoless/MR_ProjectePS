using UnityEngine;
using TMPro;

public class TimeManager : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text timeText;
    public TMP_Text dayText;

    [Header("Time")]
    public int currentHour = 10;
    public int currentDayIndex = 0;

    string[] days = { "Dilluns", "Dimarts", "Dimecres", "Dijous", "Divendres" };

    [Header("Exterior Light")]
    public Light sunLight;
    public Color morningColor = new Color(1f, 0.95f, 0.8f);
    public Color afternoonColor = new Color(1f, 1f, 0.9f);
    public Color eveningColor = new Color(1f, 0.6f, 0.4f);

    [Header("Sun Rotation")]
    public Transform sunTransform;
    public Vector3 morningRotation = new Vector3(25f, 30f, 0f);
    public Vector3 afternoonRotation = new Vector3(60f, 30f, 0f);
    public Vector3 eveningRotation = new Vector3(10f, 30f, 0f);

    [Header("Smooth Transition")]
    public float transitionSpeed = 2f;

    private Color targetColor;
    private float targetIntensity;
    private Quaternion targetRotation;


    public void SetTime(int hour)
    {
        currentHour = hour;
        timeText.text = hour.ToString("00") + ":00";
        UpdateLighting(hour);
    }

    public void SetDay(int dayIndex)
    {
        currentDayIndex = dayIndex;
        dayText.text = days[dayIndex];
    }


    void UpdateLighting(int hour)
    {
        if (hour < 12)
        {
            targetColor = morningColor;
            targetIntensity = 1.2f;
            targetRotation = Quaternion.Euler(morningRotation);
        }
        else if (hour < 17)
        {
            targetColor = afternoonColor;
            targetIntensity = 1.5f;
            targetRotation = Quaternion.Euler(afternoonRotation);
        }
        else
        {
            targetColor = eveningColor;
            targetIntensity = 0.8f;
            targetRotation = Quaternion.Euler(eveningRotation);
        }
    }
    void Update()
    {
        sunLight.color = Color.Lerp(sunLight.color, targetColor, Time.deltaTime * transitionSpeed);

        sunLight.intensity = Mathf.Lerp(sunLight.intensity, targetIntensity, Time.deltaTime * transitionSpeed);

        sunTransform.rotation = Quaternion.Slerp(sunTransform.rotation, targetRotation, Time.deltaTime * transitionSpeed);
    }


    public void ResetDay()
    {
        SetTime(10);

        targetColor = morningColor;
        targetIntensity = 1.2f;
        targetRotation = Quaternion.Euler(morningRotation);
    }

}

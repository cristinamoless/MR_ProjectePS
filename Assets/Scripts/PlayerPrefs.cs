using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider volumeSlider;

    void Start()
    {
        float volume = PlayerPrefs.GetFloat("MusicVolume", 1f);

        volumeSlider.onValueChanged.RemoveAllListeners();

        volumeSlider.value = volume;
        SetVolume(volume);

        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    public void SetVolume(float volume)
    {
        volume = Mathf.Clamp(volume, 0.0001f, 1f);

        audioMixer.SetFloat("Music", Mathf.Log10(volume) * 20f);

        PlayerPrefs.SetFloat("MusicVolume", volume);
    }
}
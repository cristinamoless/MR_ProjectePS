using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    public AudioMixer audioMixer;

    public Slider musicSlider;
    public Slider audioSlider;

    void Start()
    {
        float musicVol = PlayerPrefs.GetFloat("MusicVolume", 1f);
        musicSlider.value = musicVol;
        SetMusicVolume(musicVol);
        musicSlider.onValueChanged.RemoveAllListeners();
        musicSlider.onValueChanged.AddListener(SetMusicVolume);

        float audioVol = PlayerPrefs.GetFloat("audioVolume", 1f);
        audioSlider.value = audioVol;
        SetAudioVolume(audioVol);
        audioSlider.onValueChanged.RemoveAllListeners();
        audioSlider.onValueChanged.AddListener(SetAudioVolume);
    }

    public void SetMusicVolume(float volume)
    {
        volume = Mathf.Clamp(volume, 0.0001f, 1f);
        audioMixer.SetFloat("Music", Mathf.Log10(volume) * 20f);
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    public void SetAudioVolume(float volume)
    {
        volume = Mathf.Clamp(volume, 0.0001f, 1f);
        audioMixer.SetFloat("Audio", Mathf.Log10(volume) * 20f);
        PlayerPrefs.SetFloat("AudioVolume", volume);
    }
}

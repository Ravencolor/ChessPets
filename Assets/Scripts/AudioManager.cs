using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider musicSlider;

    private const string MusicVolumeKey = "MusicVolume";

    void Start()
    {
        // Charger les volumes sauvegard�s
        float musicVolume = PlayerPrefs.GetFloat(MusicVolumeKey, 0.5f);

        // Appliquer aux sliders
        musicSlider.value = musicVolume;

        // Appliquer au mixer
        SetMusicVolume(musicVolume);

        // Ajouter les listeners
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
    }

    public void SetMusicVolume(float volume)
    {
        // Convertir en décibels (-80 0 dB)
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat(MusicVolumeKey, volume);
        PlayerPrefs.Save();
    }

    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.Save();
    }
}

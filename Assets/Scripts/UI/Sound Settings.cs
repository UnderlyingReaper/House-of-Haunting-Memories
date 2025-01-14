using System;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundSettings : MonoBehaviour
{
    [Header("Sound Settings")]
    [SerializeField] private AudioMixer audioMixer;
    public RectTransform soundTab;
    public Slider masterVolSlider;
    [SerializeField] private TextMeshProUGUI masterVolText;
    public Slider sfxVolSlider;
    [SerializeField] private TextMeshProUGUI sfxVolText;
    public Slider musicVolSlider;
    [SerializeField] private TextMeshProUGUI musicVolText;




    public void DefaultSoundSettingsBtn()
    {
        masterVolSlider.value = 100;
        sfxVolSlider.value = 100;
        musicVolSlider.value = 100;
    }

    public void OnMasterVolChange(float val)
    {
        // Convert slider value from 0-100 to 0-1 range
        float volume = val / 100;

        // Display Text
        masterVolText.text = val.ToString();

        // Ensure volume doesn't reach exactly 0 to avoid log(0) which is undefined
        volume = Mathf.Max(0.0001f, volume);

        // Convert to decibels; -80dB is almost inaudible, 0dB is full volume
        float dbVolume = Mathf.Log10(volume) * 20;

        // Apply the volume to the AudioMixer
        audioMixer.SetFloat("MasterVol", dbVolume);
        PlayerPrefs.SetInt("MasterVol", Mathf.Clamp(Convert.ToInt32(val), 0, 100));
    }
    public void OnSFXVolChange(float val)
    {
        // Convert slider value from 0-100 to 0-1 range
        float volume = val / 100;

        // Display Text
        sfxVolText.text = val.ToString();

        // Ensure volume doesn't reach exactly 0 to avoid log(0) which is undefined
        volume = Mathf.Max(0.0001f, volume);

        // Convert to decibels; -80dB is almost inaudible, 0dB is full volume
        float dbVolume = Mathf.Log10(volume) * 20;

        // Apply the volume to the AudioMixer
        audioMixer.SetFloat("SFXVol", dbVolume);
        PlayerPrefs.SetInt("SFXVol", Mathf.Clamp(Convert.ToInt32(val), 0, 100));
    }
    public void OnMusicVolChange(float val)
    {
        // Convert slider value from 0-100 to 0-1 range
        float volume = val / 100;

        // Display Text
        musicVolText.text = val.ToString();

        // Ensure volume doesn't reach exactly 0 to avoid log(0) which is undefined
        volume = Mathf.Max(0.0001f, volume);

        // Convert to decibels; -80dB is almost inaudible, 0dB is full volume
        float dbVolume = Mathf.Log10(volume) * 20;

        // Apply the volume to the AudioMixer
        audioMixer.SetFloat("MusicVol", dbVolume);
        PlayerPrefs.SetInt("MusicVol", Mathf.Clamp(Convert.ToInt32(val), 0, 100));
    }
}
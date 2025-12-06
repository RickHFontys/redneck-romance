using UnityEngine;
using UnityEngine.Audio;

public class SoundMixerManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;

    public void SetMasterVolume(float level)
    {
        audioMixer.SetFloat("masterVolume", Mathf.Log10(level) * 20f);
    }

    public void SetSFXVolume(float level)
    {
        audioMixer.SetFloat("sfxVolume", Mathf.Log10(level) * 20f);
    }
    
    public void SetMusicVolume(float level)
    {
        audioMixer.SetFloat("musicVolume", Mathf.Log10(level) * 20f);
    }

    public void SetAmbienceVolume(float level)
    {
        audioMixer.SetFloat("ambienceVolume", Mathf.Log10(level) * 20f);
    }

    //TODO: Set audio settings sliders values to max: 1 and min: 0.0001
    //TODO: Set this scripts as On Value Changed events and set dynamic floats with these functions for each slider
}

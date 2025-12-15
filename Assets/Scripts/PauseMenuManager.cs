using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio; // Required for the Mixer
using System.Collections;

public class PauseMenuManager : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button backButton;
    [SerializeField] private Button menuButton;
    [SerializeField] private Button quitButton;

    [Header("Audio Settings")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider ambienceSlider;

    public Crossfade crossfade;

    private void Start()
    {
        backButton.onClick.AddListener(OnBackPressed);
        menuButton.onClick.AddListener(OnMenuPressed);
        quitButton.onClick.AddListener(OnQuitPressed);

        InitializeSlider(masterSlider, "MasterVol", SetMasterVolume);
        InitializeSlider(musicSlider, "MusicVol", SetMusicVolume);
        InitializeSlider(sfxSlider, "SFXVol", SetSFXVolume);
        InitializeSlider(ambienceSlider, "AmbienceVol", SetAmbienceVolume);
    }

    private void InitializeSlider(Slider slider, string prefKey, UnityEngine.Events.UnityAction<float> action)
    {
        if (slider != null)
        {
            float savedValue = PlayerPrefs.GetFloat(prefKey, 1f);
            slider.value = savedValue;
            
            action(savedValue);

            slider.onValueChanged.AddListener(action);
        }
    }

    public void SetMasterVolume(float value)
    {
        float db = Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20; 
        audioMixer.SetFloat("SetMasterVolume", db); 
        PlayerPrefs.SetFloat("MasterVol", value);
    }

    public void SetMusicVolume(float value)
    {
        float db = Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20;
        audioMixer.SetFloat("SetMusicVolume", db);
        PlayerPrefs.SetFloat("MusicVol", value);
    }

    public void SetSFXVolume(float value)
    {
        float db = Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20;
        audioMixer.SetFloat("SetSFXVolume", db);
        PlayerPrefs.SetFloat("SFXVol", value);
    }

    public void SetAmbienceVolume(float value)
    {
        float db = Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20;
        audioMixer.SetFloat("SetAmbienceVolume", db);
        PlayerPrefs.SetFloat("AmbienceVol", value);
    }

    private void OnQuitPressed() => Application.Quit();

    private void OnMenuPressed()
    {
        Time.timeScale = 1;
        StartCoroutine(crossfade.ChangeScene("StartScene"));
    }

    private void OnBackPressed() => GameManager.Instance.Pause();
    
    private void OnDisable() => PlayerPrefs.Save();
}
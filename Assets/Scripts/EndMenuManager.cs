using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndMenuManager : MonoBehaviour
{
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button creditsButton;
    [SerializeField] private Button creditsOffButton;

    [SerializeField] private GameObject creditsPage;

    public Crossfade crossfade;

    private void Start()
    {
        creditsPage.SetActive(false);

        mainMenuButton.onClick.AddListener(OnMenuPressed);
        quitButton.onClick.AddListener(OnQuitPressed);
        creditsButton.onClick.AddListener(OnCreditsPressed);
        creditsOffButton.onClick.AddListener(OnCreditsBackPressed);
    }

    private void OnCreditsPressed()
    {
        SoundFXManager.Instance.PlayButtonSFX(transform);
        creditsPage.SetActive(true);
    }

    private void OnCreditsBackPressed()
    {
        SoundFXManager.Instance.PlayButtonSFX(transform);
        creditsPage.SetActive(false);
    }

    private void OnQuitPressed()
    {
        SoundFXManager.Instance.PlayButtonSFX(transform);
        Application.Quit();
    }

    private void OnMenuPressed()
    {
        SoundFXManager.Instance.PlayButtonSFX(transform);
        StartCoroutine(crossfade.ChangeScene("StartScene"));
    }
}

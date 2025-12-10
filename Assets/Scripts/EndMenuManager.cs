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
        creditsPage.SetActive(true);
    }

    private void OnCreditsBackPressed()
    {
        creditsPage.SetActive(false);
    }

    private void OnQuitPressed()
    {
        Application.Quit();
    }

    private void OnMenuPressed()
    {
        StartCoroutine(crossfade.ChangeScene("StartScene"));
    }
}

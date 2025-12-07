using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using UnityEditor;

public class PauseMenuManager : MonoBehaviour
{
    [SerializeField] private Button backButton;
    [SerializeField] private Button menuButton;
    [SerializeField] private Button quitButton;

    private void Start()
    {
        backButton.onClick.AddListener(OnBackPressed);
        menuButton.onClick.AddListener(OnMenuPressed);
        quitButton.onClick.AddListener(OnQuitPressed);
    }

    private void OnQuitPressed()
    {
        Application.Quit();
    }

    private void OnMenuPressed()
    {
        SceneManager.LoadScene("StartScene");
    }

    private void OnBackPressed()
    {
        GameManager.Instance.Pause();
    }
}

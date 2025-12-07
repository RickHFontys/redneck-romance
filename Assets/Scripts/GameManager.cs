using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance => _instance;

    private GameObject pauseMenuPanel;
    private Character chosenCharacter;
    private bool isPaused = false;
    private bool isTimerPaused = false;

    public bool IsTimerPaused
    {
        get { return isTimerPaused; }
        set { isTimerPaused = value; }
    }

    public Character ChosenCharacter
    {
        get { return chosenCharacter; }
        set { chosenCharacter = value; }
    }

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }

        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        pauseMenuPanel = GameObject.FindWithTag("PauseMenu");

        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
    }

    public void Pause()
    {
        if (!isPaused)
        {
            pauseMenuPanel.SetActive(true);
            Time.timeScale = 0;
            isPaused = true;
        }
        else if (isPaused)
        {
            pauseMenuPanel.SetActive(false);
            Time.timeScale = 1;
            isPaused = false;
        }
    }
}

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;
using Unity.VisualScripting;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private GameObject dateSelectionPanel;
    [SerializeField] private Button shotgunChooseButton;
    [SerializeField] private Button tractorChooseButton;
    [SerializeField] private Button secondndAmendmentChooseButton;
    [SerializeField] private Button backToMenu;

    [SerializeField] private Character shotgun;
    [SerializeField] private Character tractor;
    [SerializeField] private Character secondAmendment;

    public Crossfade crossfade;

    private void Start()
    {
        dateSelectionPanel.SetActive(false);

        startButton.onClick.AddListener(OnStartPressed);
        quitButton.onClick.AddListener(OnQuitPressed);
        shotgunChooseButton.onClick.AddListener(OnShotgunChosen);
        tractorChooseButton.onClick.AddListener(OnTractorChosen);
        secondndAmendmentChooseButton.onClick.AddListener(OnSecondAmendmentChosen);
        backToMenu.onClick.AddListener(OnBackToMenuPressed);
    }

    private void OnStartPressed()
    {
        dateSelectionPanel.SetActive(true);
    }

    private void OnQuitPressed()
    {
        Application.Quit();
    }

    private void OnShotgunChosen()
    {
        GameManager.Instance.ChosenCharacter = shotgun;
        StartCoroutine(crossfade.ChangeScene("GameSceneCharactersprite"));
    }

    private void OnTractorChosen()
    {
        GameManager.Instance.ChosenCharacter = tractor;
        StartCoroutine(crossfade.ChangeScene("GameSceneCharactersprite"));
    }

    private void OnSecondAmendmentChosen()
    {
        GameManager.Instance.ChosenCharacter = secondAmendment;
        StartCoroutine(crossfade.ChangeScene("GameSceneCharactersprite"));
    }

    private void OnBackToMenuPressed()
    {
        dateSelectionPanel.SetActive(false);
    }
}

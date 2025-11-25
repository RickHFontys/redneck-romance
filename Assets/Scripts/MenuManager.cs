using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
        SceneManager.LoadScene("GameScene");
    }

    private void OnTractorChosen()
    {
        GameManager.Instance.ChosenCharacter = tractor;
        SceneManager.LoadScene("GameScene");
    }

    private void OnSecondAmendmentChosen()
    {
        GameManager.Instance.ChosenCharacter = secondAmendment;
        SceneManager.LoadScene("GameScene");
    }

    private void OnBackToMenuPressed()
    {
        dateSelectionPanel.SetActive(false);
    }
}

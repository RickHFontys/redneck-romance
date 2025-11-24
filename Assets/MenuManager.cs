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
        SceneManager.LoadScene("GameScene");
        //TODO: Make sure that the gamemanager knows which character is chosen for the game scene
    }

    private void OnTractorChosen()
    {
        Debug.Log("Tractor chosen");
    }

    private void OnSecondAmendmentChosen()
    {
        Debug.Log("Second Amendment chosen");
    }

    private void OnBackToMenuPressed()
    {
        dateSelectionPanel.SetActive(false);
    }
}

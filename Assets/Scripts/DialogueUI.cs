using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using Unity.VisualScripting;

public class DialogueUI : MonoBehaviour
{
    public DialogueManager manager;

    public TextMeshProUGUI speakerNameText;
    public TextMeshProUGUI dialogueText;

    public Button[] responseButtons; // Assign in inspector

    [Header("Typewriter Settings")]
    public float typingSpeed = 0.03f;   // lower = faster
    public AudioClip typingSoundFX;
    public AudioClip[] characterCasualSoundFX;
    private AudioClip characterTypingFX;
    private Coroutine typingCoroutine;

    private void OnEnable()
    {
        manager.OnDialogueUpdated += UpdateUI;
    }

    private void OnDisable()
    {
        manager.OnDialogueUpdated -= UpdateUI;
    }

    void UpdateUI(DialogueNode node)
    {
        // Update speaker name
        speakerNameText.text = node.speaker != null ? node.speaker.characterName : "???";

        // Stop previous typewriter effect if still running
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        // Start new typewriter animation
        typingCoroutine = StartCoroutine(TypeText(node.text));

        // Set up response buttons
        for (int i = 0; i < responseButtons.Length; i++)
        {
            if (i < node.responses.Count)
            {
                var option = node.responses[i];
                responseButtons[i].gameObject.SetActive(true);
                responseButtons[i].GetComponentInChildren<Text>().text = option.text;

                int index = i;
                responseButtons[i].onClick.RemoveAllListeners();
                responseButtons[i].onClick.AddListener(() =>
                {
                    manager.ChooseResponse(node.responses[index]);
                });
            }
            else
            {
                responseButtons[i].gameObject.SetActive(false);
            }
        }
    }

    IEnumerator TypeText(string fullText)
    {
        switch (GameManager.Instance.ChosenCharacter.characterName)
        {
            case "Shotty": // Shotgun
                characterTypingFX = characterCasualSoundFX[0];
                break;
            case "Angelica": // Tractor
                characterTypingFX = characterCasualSoundFX[1];
                break;
            case "Amanda II": // 2nd Amendment
                characterTypingFX = characterCasualSoundFX[2];
                break;
            default:
                break;
        }
        dialogueText.text = ""; // clear before typing
        GameManager.Instance.IsTimerPaused = true;
        SoundFXManager.Instance.PlaySoundFXClipWithRandomPitch(characterTypingFX, transform, 1f);

        foreach (char c in fullText)
        {
            dialogueText.text += c;
            if (typingSoundFX != null)
                SoundFXManager.Instance.PlaySoundFXClipWithRandomPitch(typingSoundFX, transform, 0.3f);
            yield return new WaitForSeconds(typingSpeed);
        }

        GameManager.Instance.IsTimerPaused = false;
    }
}

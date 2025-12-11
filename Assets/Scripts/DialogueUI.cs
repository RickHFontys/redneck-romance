using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class DialogueUI : MonoBehaviour
{
    public DialogueManager manager;

    public TextMeshProUGUI speakerNameText;
    public TextMeshProUGUI dialogueText;

    public Button[] responseButtons;

    [Header("Character Sprite UI")]
    public Image characterImage; // ← ASSIGN IN INSPECTOR

    [Header("Typewriter Settings")]
    public float typingSpeed = 0.03f;
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
        // -------------------------
        // SPEAKER NAME
        // -------------------------
        speakerNameText.text = node.speaker != null ? node.speaker.characterName : "???";

        // -------------------------
        // CHARACTER SPRITE (Idle / Default)
        // -------------------------
        if (node.speaker != null && characterImage != null)
        {
            characterImage.sprite = node.speaker.defaultSprite;
        }

        // -------------------------
        // TYPEWRITER RESET
        // -------------------------
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeText(node.text));

        // -------------------------
        // RESPONSE BUTTONS
        // -------------------------
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
                    ApplyCharacterExpression(option);
                    manager.ChooseResponse(node.responses[index]);
                });
            }
            else
            {
                responseButtons[i].gameObject.SetActive(false);
            }
        }
    }

    // ------------------------------------------------------------
    // TYPEWRITER EFFECT
    // ------------------------------------------------------------
    IEnumerator TypeText(string fullText)
    {
        // Set who is talking sound
        switch (GameManager.Instance.ChosenCharacter.characterName)
        {
            case "Shotty":
                characterTypingFX = characterCasualSoundFX[0];
                break;
            case "Angelica":
                characterTypingFX = characterCasualSoundFX[1];
                break;
            case "Amanda II":
                characterTypingFX = characterCasualSoundFX[2];
                break;
            default:
                break;
        }

        dialogueText.text = "";
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

    // ------------------------------------------------------------
    // EXPRESSION HANDLER
    // ------------------------------------------------------------
    void ApplyCharacterExpression(ResponseOption option)
    {
        if (GameManager.Instance.ChosenCharacter == null)
            return;

        var chara = GameManager.Instance.ChosenCharacter;

        if (characterImage == null)
            return;

        // Determine sprite based on response tag(s)
        if (option.tags.Contains("happy") && chara.happySprite != null)
        {
            characterImage.sprite = chara.happySprite;
        }
        else if (option.tags.Contains("angry") && chara.angrySprite != null)
        {
            characterImage.sprite = chara.angrySprite;
        }
        else if (option.tags.Contains("sad") && chara.sadSprite != null)
        {
            characterImage.sprite = chara.sadSprite;
        }
        else if (option.tags.Contains("neutral") && chara.neutralSprite != null)
        {
            characterImage.sprite = chara.neutralSprite;
        }
        else
        {
            // fallback
            characterImage.sprite = chara.defaultSprite;
        }
    }
}

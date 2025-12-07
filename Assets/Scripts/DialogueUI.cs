using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DialogueUI : MonoBehaviour
{
    public DialogueManager manager;

    public Text speakerNameText;
    public Text dialogueText;

    public Button[] responseButtons; // Assign in inspector

    [Header("Typewriter Settings")]
    public float typingSpeed = 0.03f;   // lower = faster
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
        dialogueText.text = ""; // clear before typing
        GameManager.Instance.IsTimerPaused = true;

        foreach (char c in fullText)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        GameManager.Instance.IsTimerPaused = false;
    }
}

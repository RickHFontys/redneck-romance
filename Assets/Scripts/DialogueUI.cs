using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    public DialogueManager manager;

    public Text speakerNameText;
    public Text dialogueText;

    public Button[] responseButtons; // Assign in inspector

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
        speakerNameText.text = node.speaker != null ? node.speaker.characterName : "???";
        dialogueText.text = node.text;

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
}

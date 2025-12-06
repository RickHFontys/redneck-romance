using Unity.VisualScripting;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    private DialogueNode currentNode;
    [SerializeField] private DialogueNode shotgunStartNode;
    [SerializeField] private DialogueNode tractorStartNode;
    [SerializeField] private DialogueNode secondAmendmentStartNode;

    public delegate void DialogueUpdated(DialogueNode node);
    public event DialogueUpdated OnDialogueUpdated;

    private void Start()
    {
        Character chosenCharacter = GameManager.Instance.ChosenCharacter;
        switch (chosenCharacter.name)
        {
            case "Shotgun":
                currentNode = shotgunStartNode;
                break;
            case "Tractor":
                currentNode = tractorStartNode;
                break;
            case "2ndAmendment":
                currentNode = secondAmendmentStartNode;
                break;
            default:
                break;
        }

        if (currentNode != null)
            OnDialogueUpdated?.Invoke(currentNode);
    }

    public void ChooseResponse(ResponseOption response)
    {
        // Apply love change
        Rizzometer.Instance.ApplyChange(response.loveChange);

        if (response.responseSFX != null)
        {
            SoundFXManager.Instance.PlaySoundFXClip(response.responseSFX, transform, 1f);
        }
        else
        {
            Debug.LogWarning($"No sound effect is attached to this response option: {response.name}");
        }
        
        // Find index of this response
        int index = currentNode.responses.IndexOf(response);

        if (index >= 0 && index < currentNode.nextNodes.Count)
        {
            currentNode = currentNode.nextNodes[index];
            OnDialogueUpdated?.Invoke(currentNode);
        }
        else
        {
            Debug.LogWarning("No next node available. Dialogue ends.");
        }
    }

    public void SetNode(DialogueNode node)
    {
        currentNode = node;
        OnDialogueUpdated?.Invoke(node);
    }
}

using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public DialogueNode currentNode;

    public delegate void DialogueUpdated(DialogueNode node);
    public event DialogueUpdated OnDialogueUpdated;

    private void Start()
    {
        if (currentNode != null)
            OnDialogueUpdated?.Invoke(currentNode);
    }

    public void ChooseResponse(ResponseOption response)
    {
        // Apply love change
        Rizzometer.Instance.ApplyChange(response.loveChange);

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

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic; // Needed for Lists

public class DialogueManager : MonoBehaviour
{
    [Header("Dependencies")]
    public HandController handController;

    [Header("Shotgun Character")]
    [SerializeField] private List<DialogueNode> shotgunStartNodes;
    [SerializeField] private AudioClip[] shotgunSoundFX;
    [SerializeField] private Image[] shotgunSprites;

    [Header("Tractor Character")]
    [SerializeField] private List<DialogueNode> tractorStartNodes;
    [SerializeField] private AudioClip[] tractorSoundFX;
    [SerializeField] private Image[] tractorSprites;

    [Header("2nd Amendment Character")]
    [SerializeField] private List<DialogueNode> secondAmendmentStartNodes;
    [SerializeField] private AudioClip[] secondndAmendmentSoundFX;
    [SerializeField] private Image[] secondAmendmentSprites;

    [Header("Topic System")]
    [SerializeField] private List<DialogueNode> topicPool;
    [SerializeField] private DialogueNode goodEndingNode;
    [SerializeField] private DialogueNode badEndingNode;
    [SerializeField] private int endingThreshold = 50;

    private List<DialogueNode> currentAvailableTopics;

    private DialogueNode currentNode;

    public delegate void DialogueUpdated(DialogueNode node);
    public event DialogueUpdated OnDialogueUpdated;

    private void Start()
    {
        // Reset the available topics list at the start of the game
        currentAvailableTopics = new List<DialogueNode>(topicPool);

        Character chosenCharacter = GameManager.Instance.ChosenCharacter;
        List<DialogueNode> possibleStarts = null;

        // 1. Pick the correct list of start nodes
        switch (chosenCharacter.name)
        {
            case "Shotgun":
                possibleStarts = shotgunStartNodes;
                break;
            case "Tractor":
                possibleStarts = tractorStartNodes;
                break;
            case "2ndAmendment":
                possibleStarts = secondAmendmentStartNodes;
                break;
        }
        
        //Random start node
        if (possibleStarts != null && possibleStarts.Count > 0)
        {
            int randomIndex = Random.Range(0, possibleStarts.Count);
            currentNode = possibleStarts[randomIndex];
            OnDialogueUpdated?.Invoke(currentNode);
        }
    }

    public void ChooseResponse(ResponseOption response)
    {
        Rizzometer.Instance.ApplyChange(response.loveChange);

        AudioClip[] currentClips = null;
        Image[] currentSprites = null;

        switch (GameManager.Instance.ChosenCharacter.characterName)
        {
            case "Shotty": // Shotgun
                currentClips = shotgunSoundFX;
                currentSprites = shotgunSprites;
                break;
            case "Angelica": // Tractor
                currentClips = tractorSoundFX;
                currentSprites = tractorSprites;
                break;
            case "Amanda II": // 2nd Amendment
                currentClips = secondndAmendmentSoundFX;
                currentSprites = secondAmendmentSprites;
                break;
        }

        if (currentClips != null)
        {
            PlayReaction(currentClips, currentSprites, response.loveChange);
        }

        handController.SetHands(response);

        int index = currentNode.responses.IndexOf(response);

        //Check if a next node exists for this response
        if (index >= 0 && index < currentNode.nextNodes.Count && currentNode.nextNodes[index] != null)
        {
            DialogueNode nextNode = currentNode.nextNodes[index];
            if (currentAvailableTopics.Contains(nextNode))
            {
                currentAvailableTopics.Remove(nextNode);
                Debug.Log($"[DialogueManager] Topic '{nextNode.name}' selected and removed. Remaining: {currentAvailableTopics.Count}");
            }

            currentNode = nextNode;
            OnDialogueUpdated?.Invoke(currentNode);
        }
        else
        {
            //If no next node is assigned, we assume the conversation branch has ended.
            //Triggers the topic selection node.
            GenerateTopicSelection();
        }
    }

    public void SetNode(DialogueNode node)
    {
        currentNode = node;
        OnDialogueUpdated?.Invoke(node);
    }

    private void GenerateTopicSelection()
    {
        Debug.Log($"[DialogueManager] Switching to topics. Remaining: {currentAvailableTopics.Count}");

        //Check for End Game
        if (currentAvailableTopics.Count == 0)
        {
            int currentRizz = Rizzometer.Instance.Love;
            DialogueNode endingNode = (currentRizz >= endingThreshold) ? goodEndingNode : badEndingNode;

            if (endingNode != null) SetNode(endingNode);
            else Debug.LogError("Ending Node is NULL!");

            return;
        }

        DialogueNode hubNode = ScriptableObject.CreateInstance<DialogueNode>();
        hubNode.speaker = currentNode.speaker;
        hubNode.text = "...";
        hubNode.responses = new List<ResponseOption>();
        hubNode.nextNodes = new List<DialogueNode>();

        List<DialogueNode> tempPool = new List<DialogueNode>(currentAvailableTopics);

        int topicsToSelect = Mathf.Min(2, tempPool.Count);

        for (int i = 0; i < topicsToSelect; i++)
        {
            int randomIndex = Random.Range(0, tempPool.Count);
            DialogueNode selectedTopic = tempPool[randomIndex];

            tempPool.RemoveAt(randomIndex);

            ResponseOption topicButton = ScriptableObject.CreateInstance<ResponseOption>();
            topicButton.text = "Talk about " + selectedTopic.name;
            topicButton.tags = new List<string>();

            hubNode.responses.Add(topicButton);
            hubNode.nextNodes.Add(selectedTopic);
        }

        SetNode(hubNode);
    }

    private void PlayReaction(AudioClip[] clips, Image[] sprites, int loveChange)
    {
        int index = 0;
        if (loveChange > 5) index = 1;
        else if (loveChange < -5) index = 2;

        if (clips.Length > index && clips[index] != null)
        {
            SoundFXManager.Instance.PlaySoundFXClipWithRandomPitch(clips[index], transform, 1);
        }
    }
}
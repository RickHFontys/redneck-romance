using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic; // Needed for Lists

public class DialogueManager : MonoBehaviour
{
    [Header("Dependencies")]
    public HandController handController;

    [Header("Shotgun Character")]
    // CHANGE 1: Changed single start node to a List for randomness
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

    // CHANGE 2: The Topic System
    [Header("Topic System")]
    [Tooltip("Drag your Topic Hub nodes here (Nationality, Guns, etc)")]
    [SerializeField] private List<DialogueNode> topicPool;
    [SerializeField] private DialogueNode goodEndingNode;
    [SerializeField] private DialogueNode badEndingNode;
    [SerializeField] private int endingThreshold = 50;

    // We use this list to track which topics we haven't used yet
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

        // 2. Pick a RANDOM start node from that list
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

        // --- Sound/Sprite Logic (Same as before) ---
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
        // ------------------------------------------

        // CHANGE 3: Check for Next Node OR Trigger Topics
        int index = currentNode.responses.IndexOf(response);

        // Check if a next node exists for this response
        if (index >= 0 && index < currentNode.nextNodes.Count && currentNode.nextNodes[index] != null)
        {
            // If yes, go to that node
            currentNode = currentNode.nextNodes[index];
            OnDialogueUpdated?.Invoke(currentNode);
        }
        else
        {
            // If NO next node is assigned, we assume the conversation branch has ended.
            // Triggers the Topic Selection screen.
            GenerateTopicSelection();
        }
    }

    public void SetNode(DialogueNode node)
    {
        currentNode = node;
        OnDialogueUpdated?.Invoke(node);
    }

    // CHANGE 4: The Topic Generator (With Debugging)
    private void GenerateTopicSelection()
    {
        Debug.Log($"[DialogueManager] Attempting to switch topics. Topics available: {currentAvailableTopics.Count}");

        // A. Check for End Game (No topics left)
        if (currentAvailableTopics.Count == 0)
        {
            Debug.Log("[DialogueManager] No topics left. Checking for End Game.");
            int currentRizz = Rizzometer.Instance.Love; // Check your property name here (Love vs CurrentValue)

            DialogueNode endingNode = (currentRizz >= endingThreshold) ? goodEndingNode : badEndingNode;

            if (endingNode != null)
            {
                SetNode(endingNode);
            }
            else
            {
                Debug.LogError("[DialogueManager] Ending Node is NULL! Check inspector assignments.");
            }
            return;
        }

        // B. Create the "Hub" Node on the fly
        DialogueNode hubNode = ScriptableObject.CreateInstance<DialogueNode>();
        hubNode.speaker = currentNode.speaker;
        hubNode.text = "...";

        // IMPORTANT: Initialize these lists!
        hubNode.responses = new List<ResponseOption>();
        hubNode.nextNodes = new List<DialogueNode>();

        // C. Pick up to 2 random topics
        int topicsToSelect = Mathf.Min(2, currentAvailableTopics.Count);
        Debug.Log($"[DialogueManager] Selecting {topicsToSelect} topics to display.");

        for (int i = 0; i < topicsToSelect; i++)
        {
            int randomIndex = Random.Range(0, currentAvailableTopics.Count);
            DialogueNode selectedTopicStartNode = currentAvailableTopics[randomIndex];

            // Remove from list so it doesn't repeat
            currentAvailableTopics.RemoveAt(randomIndex);

            // Create a button for this topic
            ResponseOption topicButton = ScriptableObject.CreateInstance<ResponseOption>();
            topicButton.text = "Talk about " + selectedTopicStartNode.name;
            // Initialize tags to prevent UI errors
            topicButton.tags = new List<string>();

            hubNode.responses.Add(topicButton);
            hubNode.nextNodes.Add(selectedTopicStartNode);
        }

        Debug.Log($"[DialogueManager] Hub Node Created with {hubNode.responses.Count} responses.");
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
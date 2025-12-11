using Microsoft.Unity.VisualStudio.Editor;
using Unity.VisualScripting;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    private DialogueNode currentNode;
    [SerializeField] private DialogueNode shotgunStartNode;
    [SerializeField] private DialogueNode tractorStartNode;
    [SerializeField] private DialogueNode secondAmendmentStartNode;

    [SerializeField] private AudioClip[] shotgunSoundFX;
    [SerializeField] private AudioClip[] tractorSoundFX;
    [SerializeField] private AudioClip[] secondndAmendmentSoundFX;

    [SerializeField] private Image[] shotgunSprites;
    [SerializeField] private Image[] tractorSprites;
    [SerializeField] private Image[] secondAmendmentSprites;

    public delegate void DialogueUpdated(DialogueNode node);
    public event DialogueUpdated OnDialogueUpdated;

    public HandController handController;


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
    // Apply rizz / love change
    Rizzometer.Instance.ApplyChange(response.loveChange);

    //Apply character-specific reactions (sprite + SFX)
    switch (GameManager.Instance.ChosenCharacter.characterName)
    {
        case "Shotty": // Shotgun
            ApplyShotgunSFXAndSprite(response.loveChange);
            break;

        case "Angelica": // Tractor
            ApplyTractorSFXAndSprite(response.loveChange);
            break;

        case "Amanda II": // 2nd Amendment
            ApplySecondAmendmentSFXAndSprite(response.loveChange);
            break;

        default:
            break;
    }

    //Apply hand reactions based on player response
    handController.SetHands(response);

    // Continue dialogue logic
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

    private void ApplyShotgunSFXAndSprite(int loveChange)
    {
        if (loveChange >= -5 && loveChange <= 5)
        {
            SoundFXManager.Instance.PlaySoundFXClipWithRandomPitch(shotgunSoundFX[0], transform, 1);
        }
        else if (loveChange > 5)
        {
            SoundFXManager.Instance.PlaySoundFXClipWithRandomPitch(shotgunSoundFX[1], transform, 1);
        }
        else if (loveChange < -5)
        {
            SoundFXManager.Instance.PlaySoundFXClipWithRandomPitch(shotgunSoundFX[2], transform, 1);
        }
    }

    private void ApplyTractorSFXAndSprite(int loveChange)
    {
        if (loveChange >= -5 && loveChange <= 5)
        {
            SoundFXManager.Instance.PlaySoundFXClipWithRandomPitch(tractorSoundFX[0], transform, 1);
        }
        else if (loveChange > 5)
        {
            SoundFXManager.Instance.PlaySoundFXClipWithRandomPitch(tractorSoundFX[1], transform, 1);
        }
        else if (loveChange < -5)
        {
            SoundFXManager.Instance.PlaySoundFXClipWithRandomPitch(tractorSoundFX[2], transform, 1);
        }
    }

    private void ApplySecondAmendmentSFXAndSprite(int loveChange)
    {
        if (loveChange >= -5 && loveChange <= 5)
        {
            SoundFXManager.Instance.PlaySoundFXClipWithRandomPitch(secondndAmendmentSoundFX[0], transform, 1);
        }
        else if (loveChange > 5)
        {
            SoundFXManager.Instance.PlaySoundFXClipWithRandomPitch(secondndAmendmentSoundFX[1], transform, 1);
        }
        else if (loveChange < -5)
        {
            SoundFXManager.Instance.PlaySoundFXClipWithRandomPitch(secondndAmendmentSoundFX[2], transform, 1);
        }
    }
}

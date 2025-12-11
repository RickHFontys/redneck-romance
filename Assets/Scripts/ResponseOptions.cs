using System.Collections.Generic;
using UnityEngine;

public enum HandState
{
    Neutral,
    Happy,
    Angry,
    Flirty,
    Nervous,
    Slam
}

[CreateAssetMenu(menuName = "Dialogue/Response")]
public class ResponseOption : ScriptableObject
{
    [TextArea] public string text;

    public int loveChange;
    public List<string> tags;

    [Header("Hand Reactions")]
    public HandState leftHandReaction;
    public HandState rightHandReaction;
}


using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/Node")]
public class DialogueNode : ScriptableObject
{
    public Character speaker;
    
    [TextArea]
    public string text;

    public List<ResponseOption> responses = new List<ResponseOption>();

    public List<DialogueNode> nextNodes = new List<DialogueNode>();

    // Optional for callback logic
    public List<string> tags = new List<string>();
}
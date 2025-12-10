using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/Response")]
public class ResponseOption : ScriptableObject
{
    [TextArea]
    public string text;

    public int loveChange; // Can be negative or positive

    public List<string> tags = new List<string>();
}

using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "Scriptable Objects/Character")]
public class Character : ScriptableObject
{
    public string characterName;
    // Idle sprite
    public Sprite defaultSprite;

    // Expressions based on tags
    public Sprite happySprite;
    public Sprite angrySprite;
    public Sprite neutralSprite;
    public Sprite confusedSprite;

}

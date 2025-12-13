using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [Header("Cursor Textures")]
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite clickSprite;

    [Header("Settings")]
    [SerializeField] private Vector2 hotSpot = Vector2.zero;

    private CursorMode cursorMode = CursorMode.Auto;

    private Texture2D normalCursorTexture;
    private Texture2D clickCursorTexture;

    void Start()
    {
        normalCursorTexture = TextureFromSprite(normalSprite);
        clickCursorTexture = TextureFromSprite(clickSprite);

        SetNormalCursor();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SetClickCursor();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            SetNormalCursor();
        }
    }

    void SetNormalCursor()
    {
        Cursor.SetCursor(normalCursorTexture, hotSpot, cursorMode);
    }

    void SetClickCursor()
    {
        Cursor.SetCursor(clickCursorTexture, hotSpot, cursorMode);
    }

    private Texture2D TextureFromSprite(Sprite sprite)
    {
        if (sprite.rect.width != sprite.texture.width)
        {
            // Create a new texture of the exact size of the sprite slice
            Texture2D newText = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);

            // Get the pixels from the original sheet, but only the part the sprite occupies
            Color[] newColors = sprite.texture.GetPixels(
                (int)sprite.textureRect.x,
                (int)sprite.textureRect.y,
                (int)sprite.textureRect.width,
                (int)sprite.textureRect.height
            );

            // Apply pixels to the new texture
            newText.SetPixels(newColors);
            newText.Apply();
            return newText;
        }
        else
        {
            // If the sprite uses the full texture, just return it
            return sprite.texture;
        }
    }
}
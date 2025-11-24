using UnityEngine;
using UnityEngine.UI;

public class Rizzometer : MonoBehaviour
{
    public static Rizzometer Instance { get; private set; }

    [Header("Rizz Settings")]
    public int maxLove = 100;

    [SerializeField] 
    private int love = 0;              // Shows in Inspector for debugging

    [Header("UI")]
    public Slider loveSlider;          // Drag your UI Slider here

    public int Love => love;           // Read-only from other scripts

    private void Awake()
    {
        // Singleton setup
        if (Instance != this && Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        // Auto-assign if not manually set
        if (loveSlider == null)
        {
            loveSlider = FindObjectOfType<Slider>();
        }

        if (loveSlider != null)
        {
            loveSlider.minValue = 0;
            loveSlider.maxValue = maxLove;
            loveSlider.value = love;
        }
    }

    /// Change the love meter and update UI.
    public void ApplyChange(int delta)
    {
        int oldLove = love;

        // Clamp between 0 and max
        love = Mathf.Clamp(love + delta, 0, maxLove);

        // Update slider only if value changed
        if (love != oldLove && loveSlider != null)
        {
            loveSlider.value = love;
        }

        Debug.Log($"[Rizzometer] Love updated: {love}");
    }
}

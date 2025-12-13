using UnityEngine;
using UnityEngine.UI;

public class Rizzometer : MonoBehaviour
{
    public static Rizzometer Instance { get; private set; }

    [Header("Rizz Settings")]
    public int maxLove = 100;

    [SerializeField]
    private int love = 50;

    [Header("UI")]
    public Slider loveSlider;          // Drag your UI Slider here

    [Header("VFX Prefabs")]
    public ParticleSystem vfxIncreasePrefab;
    public ParticleSystem vfxDecreasePrefab;

    public int Love => love;           // Read-only from other scripts

    public Crossfade crossfade;

    private void Awake()
    {
        if (Instance != this && Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        if (loveSlider == null)
        {
            loveSlider = GameObject.FindWithTag("Rizzometer").GetComponentInChildren<Slider>();
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
            if (loveSlider != null)
                loveSlider.value = love;

            Vector3 spawnPos = Camera.main.ScreenToWorldPoint(
                new Vector3(Screen.width / 2f, Screen.height / 2f, 5f)
            );

            if (delta > 0 && vfxIncreasePrefab != null)
            {
                ParticleSystem fx = Instantiate(vfxIncreasePrefab, spawnPos, Quaternion.identity);
                fx.Play();
            }

            if (delta < 0 && vfxDecreasePrefab != null)
            {
                ParticleSystem fx = Instantiate(vfxDecreasePrefab, spawnPos, Quaternion.identity);
                fx.Play();
            }
        }

        if (love <= 0)
        {
            GameManager.Instance.GameEnding = GameEnding.bad;
            StartCoroutine(crossfade.ChangeScene("EndScene"));
        }
        else if (love >= 100)
        {
            GameManager.Instance.GameEnding = GameEnding.good;
            StartCoroutine(crossfade.ChangeScene("EndScene"));
        }

        Debug.Log($"[Rizzometer] Love updated: {love}");
    }
}

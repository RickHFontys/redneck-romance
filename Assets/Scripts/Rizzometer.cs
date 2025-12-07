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
    public Slider loveSlider;

    [Header("VFX Prefabs")]
    public ParticleSystem vfxIncreasePrefab;
    public ParticleSystem vfxDecreasePrefab;
    // Play when love goes DOWN

    public int Love => love;

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

    public void ApplyChange(int delta)
    {
        int oldLove = love;
        love = Mathf.Clamp(love + delta, 0, maxLove);

        if (love != oldLove)
        {
            if (loveSlider != null)
                loveSlider.value = love;

            // Spawn position: screen center
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

        Debug.Log($"[Rizzometer] Love updated: {love}");
    }

}

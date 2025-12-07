using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    [SerializeField] private Slider timerSlider;
    [SerializeField] private Text timerText;
    [SerializeField] private float maxTimerValue;

    private bool stopTimer;
    private float currentTime;

    private void Start()
    {
        stopTimer = false;
        currentTime = maxTimerValue;

        timerSlider.minValue = 0;
        timerSlider.maxValue = maxTimerValue;
        timerSlider.value = maxTimerValue;
        timerText.text = maxTimerValue.ToString();

        timerText.text = maxTimerValue.ToString("0.000");
    }

    private void Update()
    {
        if (stopTimer) return;
        if (GameManager.Instance.IsTimerPaused) return;

        currentTime -= Time.deltaTime;

        if (currentTime <= 0f)
        {
            currentTime = 0f;
            stopTimer = true;
            timerText.text = "0.000";
            timerSlider.value = 0;
        }

        if (!stopTimer)
        {
            timerText.text = string.Format("{0:#.000}", currentTime);
            timerSlider.value = currentTime;
        }
    }
}

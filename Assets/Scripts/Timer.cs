using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    [SerializeField] private Slider timerSlider;
    [SerializeField] private Text timerText;
    [SerializeField] private float maxTimerValue;

    private bool stopTimer;

    private void Start()
    {
        stopTimer = false;
        timerSlider.minValue = 0;
        timerSlider.maxValue = maxTimerValue;
        timerSlider.value = maxTimerValue;
        timerText.text = maxTimerValue.ToString();
    }

    private void Update()
    {
        float time = maxTimerValue - Time.time;

        if (time <= 0)
        {
            stopTimer = true;
            timerText.text = "0.000";
            timerSlider.value = 0;
        }

        if (!stopTimer)
        {
            timerText.text = string.Format("{0:#.000}", time);
            timerSlider.value = time;
        }
    }
}

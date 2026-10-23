using TMPro;
using UnityEngine;

public class TimerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private float totalTime = 300f;

    private float timeLeft;
    private bool isRunning = true;

    public float TimeLeft => timeLeft;
    public bool IsTimeUp => timeLeft <= 0f;

    void Start()
    {
        timeLeft = totalTime;
        UpdateText();
    }

    void Update()
    {
        if (!isRunning) return;

        timeLeft -= Time.deltaTime;
        if (timeLeft < 0f) timeLeft = 0f;
        UpdateText();

        if (IsTimeUp) isRunning = false;
    }

    public void StopTimer() => isRunning = false;

    void UpdateText()
    {
        int minutes = Mathf.FloorToInt(timeLeft / 60f);
        int seconds = Mathf.FloorToInt(timeLeft % 60f);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }
}
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private TimerUI timer;
    [SerializeField] private TextMeshProUGUI lootText;

    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TextMeshProUGUI gameOverScore;

    private int lootCollected = 0;
    private bool isGameOver = false;
    public bool IsGameOver => isGameOver;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }
    void Start()
    {
        UpdateLootText();
    }
    private void Update()
    {
        if (isGameOver) return;

        if (timer != null && timer.IsTimeUp)
            EndGame();
    }

    public void AddLoot()
    {
        lootCollected++;
        UpdateLootText();
    }

    void UpdateLootText()
    {
        lootText.text = $"Collected: {lootCollected}";
    }
    void EndGame()
    {
        Time.timeScale = 0f;
        isGameOver = true;
        gameOverScore.text = $"Собрано предметов: {lootCollected}";
        gameOverPanel.SetActive(true);
    }
}
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private TimerUI timer;
    [SerializeField] private TextMeshProUGUI lootText;

    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TextMeshProUGUI gameOverScore;

    [SerializeField] private GameObject exitPanel;
    private float exitCooldownUntil = 0f;


    private int lootCollected = 0;
    private bool isGameOver = false;
    private bool isExitMenuOpen = false;
    private int bankedLoot = 0;
    public bool IsGameOver => isGameOver;

    void Awake()
    {
        if (instance == null) instance = this;
        if (exitPanel != null) exitPanel.SetActive(false);
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
    public void OpenExitMenu()
    {
        if (isGameOver || isExitMenuOpen) return;
        if (Time.unscaledTime < exitCooldownUntil) return;
        isExitMenuOpen = true;
        if (exitPanel != null) exitPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    void CloseExitMenu()
    {
        isExitMenuOpen = false;
        if (exitPanel != null) exitPanel.SetActive(false);
        Time.timeScale = 1f;
        exitCooldownUntil = Time.unscaledTime + 1f;
    }
    public void SubmitAndContinue()
    {
        bankedLoot += lootCollected;
        lootCollected = 0;
        UpdateLootText();
        CloseExitMenu();
    }
    public void FinishGame()
    {
        CloseExitMenu();
        EndGame();
    }
    void EndGame()
    {
        isGameOver = true;

        int total = bankedLoot + lootCollected;
        if (gameOverScore != null)
            gameOverScore.text = $"Собрано предметов: {total}";

        if (gameOverPanel != null) gameOverPanel.SetActive(true);
        if (exitPanel != null) exitPanel.SetActive(false);

        Time.timeScale = 0f;
    }
    public void OnPlayerCaught()
    {
        if (isGameOver) return;

        int total = bankedLoot + lootCollected;
        if (gameOverScore != null)
            gameOverScore.text = $"Вас поймали!\nПредметов собрано: {total}";

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        isGameOver = true;
        Time.timeScale = 0f;
    }
}

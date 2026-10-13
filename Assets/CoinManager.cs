using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CoinManager : MonoBehaviour
{
    public static CoinManager instance;

    public TMP_Text coinText;
    public TMP_Text scoreText;
    public TMP_Text timeText;

    public float startTime = 400f;
    public float timeSpeed = 2.5f;   // в Марио «секунда» таймера быстрее настоящей

    private int coins = 0;
    private int score = 0;
    private float timeLeft;
    private bool timerStopped = false;

    void Awake()
    {
        if (instance == null) instance = this;
        timeLeft = startTime;
        UpdateUI();
    }

    void Update()
    {
        if (timerStopped) return;

        timeLeft -= Time.deltaTime * timeSpeed;
        if (timeLeft <= 0)
        {
            // Время вышло — смерть
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            return;
        }
        if (timeText != null) timeText.text = "TIME " + Mathf.CeilToInt(timeLeft);
    }

    public void AddCoin()
    {
        coins++;
        AddScore(200);           // монета в Марио = 200 очков
    }

    public void AddScore(int points)
    {
        score += points;
        UpdateUI();
    }

    public void StopTimer()
    {
        timerStopped = true;
    }

    void UpdateUI()
    {
        if (coinText != null) coinText.text = "COINS " + coins;
        if (scoreText != null) scoreText.text = "SCORE " + score.ToString("D6");
    }
}
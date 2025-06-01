using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI errorsText;
    public GameObject gameOverPanel;

    [Header("Config")]
    public int maxErrors = 5;

    [Header("Dificuldade")]
    public float difficultyMultiplier = 1f;
    public float difficultyIncrement = 0.1f; // Aumenta a velocidade a cada pato abatido

    private int ducksHit = 0;
    private int errors = 0;

    void Start()
    {
        UpdateUI();
        gameOverPanel.SetActive(false);
    }

    public void DuckHit()
    {
        ducksHit++;
        difficultyMultiplier += difficultyIncrement;
        UpdateUI();
    }

    public void MissedShot()
    {
        errors++;
        UpdateUI();

        if (errors >= maxErrors)
        {
            GameOver();
        }
    }

    void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = $"Patos: {ducksHit}";

        if (errorsText != null)
            errorsText.text = $"Erros: {errors}/{maxErrors}";
    }

    void GameOver()
    {
        Time.timeScale = 0f;
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

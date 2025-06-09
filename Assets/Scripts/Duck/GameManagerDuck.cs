using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class GameManagerDuck : MonoBehaviour
{
    public static GameManagerDuck Instance;

    [Header("UI")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI errorsText;
    public TextMeshProUGUI roundText;
    public GameObject gameOverPanel;
    public GameObject victoryPanel;

    [Header("Configuração")]
    public string[] possibleColors = { "Red", "Blue" };
    public string currentTargetColor;
    public DogController dog;
    public float waitBeforeRound = 1f;
    public float roundTime = 3f;
    public float waitAfterRound = 1f;
    public int maxErrors = 5;

    [Header("Sons")]
    public AudioSource audioSource;
    public AudioClip winRoundSound;
    public AudioClip loseRoundSound;
    public AudioClip duckClickSound;
    public AudioClip signChangeSound;
    public AudioClip gameOverSound;
    public AudioClip victorySound;
    public AudioClip backgroundMusic;

    private int roundsWon = 0;
    private int roundCount = 0;
    private int errors = 0;
    private List<Duck> allDucks = new List<Duck>();
    private bool isShuffling = false;

    void Awake() => Instance = this;

    void Start()
    {
        gameOverPanel.SetActive(false);
        victoryPanel.SetActive(false);
        Time.timeScale = 1f;
        PlayBackgroundMusic();
        StartCoroutine(GameLoop());
    }

    void PlayBackgroundMusic()
    {
        if (backgroundMusic != null)
        {
            audioSource.clip = backgroundMusic;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    public void RegisterDuck(Duck duck) => allDucks.Add(duck);

    public void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    IEnumerator GameLoop()
    {
        yield return dog.SetInitialSign();

        while (roundCount < 8)
        {
            int shelves = GetShelfCount(roundCount + 1);
            SetShelfVisibility(shelves);

            yield return NewRound();
            yield return new WaitForSeconds(roundTime);
            yield return dog.MoveUp();

            bool won = EvaluateHits();

            if (errors >= maxErrors)
            {
                GameOver();
                yield break;
            }

            if (won)
            {
                PlaySound(winRoundSound);
                roundsWon++;
                roundCount++;
            }
            else
            {
                PlaySound(loseRoundSound);
            }

            UpdateUI();

            if (roundCount == 8)
            {
                StartCoroutine(ShuffleShelves());
                Victory();
                yield break;
            }

            yield return new WaitForSeconds(waitAfterRound);
        }
    }

    IEnumerator NewRound()
    {
        foreach (Duck duck in allDucks)
        {
            duck.SetRandomColor(possibleColors);
            duck.SetClickable(false);
        }

        List<string> roundColors = new List<string>();
        foreach (Duck duck in allDucks)
        {
            if (duck.gameObject.activeInHierarchy && !roundColors.Contains(duck.CurrentColor))
                roundColors.Add(duck.CurrentColor);
        }

        currentTargetColor = roundColors[Random.Range(0, roundColors.Count)];
        yield return dog.MoveDown(currentTargetColor);
        PlaySound(signChangeSound);

        foreach (Duck duck in allDucks)
        {
            duck.SetClickable(duck.gameObject.activeInHierarchy);
        }
    }

    bool EvaluateHits()
    {
        int hits = 0;
        int expected = 0;
        bool anyError = false;

        foreach (Duck duck in allDucks)
        {
            if (!duck.gameObject.activeInHierarchy) continue;

            if (duck.CurrentColor == currentTargetColor)
            {
                expected++;
                if (duck.wasHit) hits++;
                else anyError = true;
            }
            else
            {
                if (duck.wasHit) anyError = true;
            }

            duck.ResetDuck();
        }

        if (anyError || hits < expected)
        {
            errors++;
            return false;
        }

        return true;
    }

    void UpdateUI()
    {
        scoreText.text = $"vencidas: {roundsWon}";
        errorsText.text = $"Erros: {errors}/{maxErrors}";
        roundText.text = $"Rodada: {roundCount}/8";
    }

    void GameOver()
    {
        PlaySound(gameOverSound);
        Time.timeScale = 0f;
        gameOverPanel.SetActive(true);
    }

    void Victory()
    {
        PlaySound(victorySound);
        Time.timeScale = 0f;
        victoryPanel.SetActive(true);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    int GetShelfCount(int round)
    {
        if (round < 3) return 1;
        if (round < 5) return 2;
        if (round < 7) return 3;
        return 4;
    }

    void SetShelfVisibility(int count)
    {
        for (int i = 0; i < allDucks.Count; i++)
        {
            allDucks[i].gameObject.SetActive(i < count * 3);
        }
    }

    IEnumerator ShuffleShelves()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.5f);

            List<Vector3> positions = new List<Vector3>();
            foreach (var duck in allDucks)
            {
                positions.Add(duck.transform.position);
            }

            for (int i = 0; i < allDucks.Count; i++)
            {
                int targetIndex = (i + 1) % allDucks.Count;
                allDucks[i].transform.position = positions[targetIndex];
            }
        }
    }

    public bool IsClickable => true;
}

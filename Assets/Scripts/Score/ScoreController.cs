using UnityEngine;
using System.Collections.Generic;

public class ScoreController : MonoBehaviour
{
    public ScoreComponent[] scores;
    public GameObject lineChart;
    private GameData gameData;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameData gameData = SaveSystem.Load();
        for (int i = 0; i < 5; i++)
        {
            var scoreComponent = scores[i];
            var level = gameData.GetType().GetProperty(gameData.currentLevel)?.GetValue(gameData) as List<GameSessionData>;
            var score = level[i].score;
            scoreComponent.scoreText.text = score.ToString();
            scoreComponent.trophy.SetActive(level[i].timestamp == gameData.currentTimestamp);
            scoreComponent.bg.SetActive(level[i].timestamp == gameData.currentTimestamp);
        }
    }
}

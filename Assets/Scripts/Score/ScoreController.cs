using UnityEngine;
using System.Collections.Generic;
using XCharts.Runtime;
using System.Linq;
using System;
using TMPro;

public class ScoreController : MonoBehaviour
{
    public ScoreComponent[] scores;
    public GameObject lineChart;
    public TextMeshProUGUI levelNameText;
    private GameData gameData;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameData gameData = SaveSystem.Load();
        levelNameText.text = gameData.currentLevelName;
        var level = gameData.currentLevel;
        level = level.OrderByDescending(x => x.score).ToList();
        for (int i = 0; i < 5; i++)
        {
            var scoreComponent = scores[i];
            if (level == null || i < level.Count)
            {
                var score = level[i].score;
                scoreComponent.scoreText.text = score.ToString();
                scoreComponent.trophy.SetActive(level[i].timestamp == gameData.currentTimestamp);
                scoreComponent.bg.SetActive(level[i].timestamp == gameData.currentTimestamp);
            }
        }
        // var chart = gameObject.GetComponent<LineChart>();
        // chart.ClearData();
        
        // var graphData = gameData.currentLevel.OrderBy(x => DateTime.TryParse(x.timestamp, out DateTime parsedTimestamp)).Take(5).ToList();
        // for (int i = 0; i < graphData.Count; i++)
        // {
        //     var data = graphData[i];
        //     chart.AddXAxisData("Data", data.score);
        //     chart.AddYAxisData("Score", data.timestamp);
        // }
    }
    public void GoHome() {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
        }
    }
}

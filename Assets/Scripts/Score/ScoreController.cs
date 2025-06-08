using UnityEngine;
using System.Collections.Generic;
using XCharts.Runtime;
using System.Linq;
using System;
using TMPro;

public class ScoreController : MonoBehaviour
{
    public ScoreComponent[] scores;
    public LineChart lineChart;
    public TextMeshProUGUI levelNameText;
    private GameData gameData;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameData gameData = SaveSystem.Load();
        levelNameText.text = gameData.currentLevelName;
        var level = gameData.currentLevel;
        var orderedLevel = level.AsEnumerable().OrderByDescending(x => x.score).ToList();
        var reversed = level.TakeLast(5).ToList();
        lineChart.RemoveData();
        lineChart.AddSerie<Line>("Score");
        for (int i = 0; i < 5; i++)
        {
            var scoreComponent = scores[i];
            if (orderedLevel == null || i < orderedLevel.Count)
            {
                var score = orderedLevel[i].score;
                scoreComponent.scoreText.text = score.ToString();
                scoreComponent.trophy.SetActive(orderedLevel[i].timestamp == gameData.currentTimestamp);
                scoreComponent.bg.SetActive(orderedLevel[i].timestamp == gameData.currentTimestamp);
            }
            double[] data = new double[2];
            data[0] = i;
            data[1] = reversed[i].score;
            lineChart.AddData(0, data);
        }
        
        // lineChart.AddData(0, serie);
        // var chart = gameObject.GetComponent<LineChart>();


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

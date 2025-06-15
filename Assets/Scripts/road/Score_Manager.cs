using System.Collections;

using System.Collections.Generic;

using UnityEngine;

using TMPro;



public class Score_Manager : MonoBehaviour

{

    public int score = 0;

    public int highScore;

    public static int lastScore = 0;



    public TextMeshProUGUI scoreText;

    public TextMeshProUGUI highScoreText;

    public TextMeshProUGUI lastScoreText;


    private float timeLeft = 60f;





    void Start()

    {

        StartCoroutine(Score());



        highScore = PlayerPrefs.GetInt("high_score", 0);



        // highScoreText.text = "HighScore: " + highScore.ToString();

        // lastScoreText.text = "LastScore: " + lastScore.ToString();

    }





    void Update()

    {

        scoreText.text = "Score:" + score.ToString();

        timeLeft -= Time.deltaTime;

        if (score > highScore)

        {

            highScore = score;

            PlayerPrefs.SetInt("high_score", highScore);

        }

    }



    IEnumerator Score()

    {

        while (true)

        {

            yield return new WaitForSeconds(0.8f);

            score = score + 10;

            lastScore = score;

        }

    }
    
    public void SetScore()

    {

        var gameData = SaveSystem.Load();
        gameData.currentHighScore = score;
        gameData.currentTimestamp = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        gameData.currentLevelName = "Car";
        if (gameData.car == null)
        {
            gameData.car = new List<GameSessionData>();
        }
        gameData.car.Add(new GameSessionData { score = score, timestamp = gameData.currentTimestamp });
        gameData.currentLevel = gameData.car;
        SaveSystem.Save(gameData);

    }

}
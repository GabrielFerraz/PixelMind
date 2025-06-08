using System.Collections;

using System.Collections.Generic;

using UnityEngine;

using TMPro; 

using UnityEngine.SceneManagement;



public class Game_Controller : MonoBehaviour

{

    public TextMeshProUGUI highSoreText; 
    public TextMeshProUGUI scoreText;    



    private int score;

    private int highScore;



    public Score_Manager score_manager;



    public GameObject gamePausePanel;

    public GameObject gamePauseButton;



    
    void Start()

    {

        gamePausePanel.SetActive(false);

        gamePauseButton.SetActive(true);

    }



   

    void Update()

    {

        highScore = PlayerPrefs.GetInt("high_score");

        score = 0;



        // highSoreText.text = "Highscore: " + highScore.ToString();

        scoreText.text = "Your Score: " + score.ToString();

    }



    public void Restart()

    {

        SceneManager.LoadScene("Road");

    }



    public void PauseGame()

    {

        Time.timeScale = 0;

        gamePausePanel.SetActive(true);

        gamePauseButton.SetActive(false);

    }



    public void ResumeGame()

    {

        Time.timeScale = 1;

        gamePausePanel.SetActive(false);

        gamePauseButton.SetActive(true);

    }



    public void Main_Menu()

    {

        SceneManager.LoadScene("Menu");

    }




}
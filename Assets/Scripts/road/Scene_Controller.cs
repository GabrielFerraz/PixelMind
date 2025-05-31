 using System.Collections;

using System.Collections.Generic;

using UnityEngine;

using UnityEngine.SceneManagement;



public class Scene_Controller : MonoBehaviour

{

    public void Start()

    {

        Time.timeScale = 1;

    }

    public void GameScene()

    {

        SceneManager.LoadScene("Game");

    }



    public void ExitGame()

    {

        Application.Quit();

    }

}


using UnityEngine;

public class Menu : MonoBehaviour
{
    public GameObject startButton;
    public GameObject creditsButton;
    public GameObject carroButton;
    public GameObject sapoButton;
    public GameObject backButton;
    public GameObject creditsText;

    public void StartGame()
    {
        startButton.SetActive(false);
        creditsButton.SetActive(false);
        carroButton.SetActive(true);
        sapoButton.SetActive(true);
        backButton.SetActive(true);
    }

    public void ShowCredits()
    {
        startButton.SetActive(false);
        creditsButton.SetActive(false);
        backButton.SetActive(true);
        creditsText.SetActive(true);
    }

    public void RunCarro()
    {
        // Load the Carro game scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("Road");
    }

    public void RunSapo()
    {
        // Load the Sapo game scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("Frogger");
    }

    public void Back() {
        startButton.SetActive(true);
        creditsButton.SetActive(true);
        carroButton.SetActive(false);
        sapoButton.SetActive(false);
        backButton.SetActive(false);
        creditsText.SetActive(false);
    }
}

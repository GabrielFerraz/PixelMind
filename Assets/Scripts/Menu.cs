using UnityEngine;
using UnityEngine.Audio;

public class Menu : MonoBehaviour
{
    public GameObject startButton;
    public GameObject creditsButton;
    public GameObject carroButton;
    public GameObject sapoButton;
    public GameObject patoButton;
    public GameObject backButton;
    public GameObject creditsText;

    private AudioSource audioSource;
    private AudioResource music;
    private AudioResource pr;

    public void Start()
    {
        audioSource = GetComponent<AudioSource>();
        music = Resources.Load<AudioResource>("Sounds/Menu/Menu");
        pr = Resources.Load<AudioResource>("Sounds/Menu/PR");
        audioSource.resource = music;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void StartGame()
    {
        startButton.SetActive(false);
        creditsButton.SetActive(false);
        carroButton.SetActive(true);
        sapoButton.SetActive(true);
        patoButton.SetActive(true);
        backButton.SetActive(true);
    }

    public void ShowCredits()
    {
        startButton.SetActive(false);
        creditsButton.SetActive(false);
        backButton.SetActive(true);
        creditsText.SetActive(true);
        
        audioSource.Stop();
        audioSource.resource = pr;
        audioSource.Play();
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

    public void RunExp()
    {
        // Load the Sapo game scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("Frogger");
    }

    public void RunPato()
    {
        // Load the Sapo game scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("Pato");
    }

    public void Back() {
        startButton.SetActive(true);
        creditsButton.SetActive(true);
        carroButton.SetActive(false);
        sapoButton.SetActive(false);
        patoButton.SetActive(false);
        backButton.SetActive(false);
        creditsText.SetActive(false);
        if (audioSource.resource.name != music.name)
        {
            audioSource.Stop();
            audioSource.resource = music;
            audioSource.Play();
        }
    }
}

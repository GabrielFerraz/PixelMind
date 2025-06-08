using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    public float speed = 5f;
    public float rotationSpeed = 5f;

    public Score_Manager scoreValue;
    public GameObject gameOverPanel;
    public Car_Spawner spawner;

    bool isMovingLeft = false;
    bool isMovingRight = false;
    public AudioSource audioSource;
    private AudioClip collisionSound;
    private AudioClip coinSound;
    private float timeLeft = 60f;

    

    void Start()
    {
        gameOverPanel.SetActive(false);
        Time.timeScale = 1;
        audioSource = GetComponent<AudioSource>();
        coinSound = Resources.Load<AudioClip>("Sounds/carro/moeda");
        collisionSound = Resources.Load<AudioClip>("Sounds/carro/colisao");
    }

    void Update()
    {
        HandleInput();
        Movement();
        Clamp();

        if (Input.GetKey(KeyCode.LeftArrow))
            MoveLeft();
        if (Input.GetKey(KeyCode.RightArrow))
            MoveRight();

        if (isMovingLeft)
        {
            MoveLeft();
        }

        if (isMovingRight)
        {
            MoveRight();
        }
        if (timeLeft <= 0)
        {
            Time.timeScale = 0;
            timeLeft = 10000;
            scoreValue.SetScore();
            gameOverPanel.SetActive(true);
        }
        else
        {
            timeLeft -= Time.deltaTime;
        }
    }

    void HandleInput()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (worldPoint.x < transform.position.x)
            {
                MoveLeft();
            }
            else
            {
                MoveRight();
            }
        }
    }

    void Movement()
    {
        if (transform.rotation.z != 90)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, 0), 10f * Time.deltaTime);
        }
    }

    void MoveLeft()
    {
        transform.position -= new Vector3(speed * Time.deltaTime, 0, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, 47), rotationSpeed * Time.deltaTime);
    }

    void MoveRight()
    {
        transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, -47), rotationSpeed * Time.deltaTime);
    }

    public void MovingLeft(bool i)
    {
        isMovingLeft = i;
    }

    public void MovingRight(bool i)
    {
        isMovingRight = i;
    }

    void Clamp()
    {
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, -1.8f, 1.8f);
        transform.position = pos;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Cars")
        {
            audioSource.PlayOneShot(collisionSound);
            spawner.ResetSpeed();
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag == "Coin")
        {
            audioSource.PlayOneShot(coinSound);
            scoreValue.score += 10;
            Destroy(collision.gameObject);
        }
    }

    // private IEnumerator OpenMenu()
    // {

    //     gameOverPanel.SetActive(true);
    // }
}

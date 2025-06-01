using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    public float speed = 5f;
    public float rotationSpeed = 5f;

    public Score_Manager scoreValue;
    public GameObject gameOverPanel;

    bool isMovingLeft = false;
    bool isMovingRight = false;

    public AudioSource collisionAudio;
    public AudioSource collectAudio;

    void Start()
    {
        gameOverPanel.SetActive(false);
        Time.timeScale = 1;
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
        if(collision.gameObject.tag == "Cars")
        {
            Time.timeScale = 0;
            gameOverPanel.SetActive(true);
            collisionAudio.Play();
        }

        if (collision.gameObject.tag == "Coin")
        {
            scoreValue.score += 10;
            Destroy(collision.gameObject);
            collectAudio.Play();
        }
    }
}

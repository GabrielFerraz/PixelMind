using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Car_Spawner : MonoBehaviour
{
    public GameObject[] carPrefabs;
    public GameObject alertPrefab;
    public TextMeshProUGUI speedText;

    private float carSpeed = 30f; 
    private float maxSpeed = 130f;
    private float speedIncrement = 5f;

    private float spawnDelay = 2f;
    private float minSpawnDelay = 0.8f;

    private float[] lanes = new float[] { -1.8f, 0f, 1.8f };

    void Start()
    {
        StartCoroutine(SpawnCars());
        StartCoroutine(IncreaseDifficulty());
    }

    IEnumerator SpawnCars()
    {
        while (true)
        {
            int carsToSpawn = Random.Range(1, lanes.Length);

            List<int> usedLanes = new List<int>();
            while (usedLanes.Count < carsToSpawn)
            {
                int randLane = Random.Range(0, lanes.Length);
                if (!usedLanes.Contains(randLane))
                {
                    usedLanes.Add(randLane);
                    Vector3 alertPos = new Vector3(lanes[randLane], transform.position.y, transform.position.z);
                    GameObject alert = Instantiate(alertPrefab, alertPos, Quaternion.identity);
                    StartCoroutine(SpawnCarWithDelay(alert, alertPos));
                }
            }

            yield return new WaitForSeconds(spawnDelay);
        }
    }

    IEnumerator SpawnCarWithDelay(GameObject alert, Vector3 spawnPos)
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(alert);

        int randCar = Random.Range(0, carPrefabs.Length);
        GameObject newCar = Instantiate(carPrefabs[randCar], spawnPos, Quaternion.Euler(0, 0, -180));

        Rigidbody2D rb = newCar.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            float speedMPS = carSpeed / 3.6f;
            rb.linearVelocity = new Vector2(0, -speedMPS);
        }
    }

    IEnumerator IncreaseDifficulty()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);

            if (carSpeed < maxSpeed)
                carSpeed += speedIncrement;

            if (spawnDelay > minSpawnDelay)
                spawnDelay -= 0.1f;

            if (speedText != null)
                speedText.text = "Velocidade: " + Mathf.Min(carSpeed, maxSpeed).ToString("F0") + " km/h";
        }
    }
}

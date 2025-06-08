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
    public Road_Movement roadMovement;

    public float carSpeed = 0f;
    private float maxSpeed = 130f;
    public float speedIncrement = 1f;
    public float speedConstant = 0.2f;

    private float spawnDelay = 2f;
    private float minSpawnDelay = 0.8f;

    private float[] lanes = new float[] { -1.8f, 0f, 1.8f };
    private float[] laneAngles = new float[] { -202.5f, -180f, -157.5f };

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
                    StartCoroutine(SpawnCarWithDelay(alert, alertPos, laneAngles[randLane]));
                }
            }

            yield return new WaitForSeconds(spawnDelay);
        }
    }

    IEnumerator SpawnCarWithDelay(GameObject alert, Vector3 spawnPos, float angle)
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(alert);

        int randCar = Random.Range(0, carPrefabs.Length);
        GameObject newCar = Instantiate(carPrefabs[randCar], spawnPos, Quaternion.Euler(0, 0, -180));

        Rigidbody2D rb = newCar.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            float speedMPS = carSpeed / 3.6f;
            float angleRad = angle * Mathf.Deg2Rad;
            Vector2 velocity = new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad)) * speedMPS;
            rb.linearVelocity = velocity;
            // rb.linearVelocity = new Vector2(0, -speedMPS);
        }
    }

    IEnumerator IncreaseDifficulty()
    {

        float vmax = 130f;
        float tempoAtual = 0f;
        while (carSpeed < maxSpeed)
        {

            float t = tempoAtual; // em segundos
            float dvdt = vmax * speedConstant * Mathf.Exp(-speedConstant * t);
            float timePerKmH = 1f / dvdt;
            yield return new WaitForSeconds(timePerKmH);
            tempoAtual += timePerKmH;

            if (carSpeed < maxSpeed)
                carSpeed += speedIncrement;

            if (spawnDelay > minSpawnDelay)
                spawnDelay -= 0.1f;

            if (speedText != null)
                speedText.text = "Vel: " + Mathf.Min(carSpeed, maxSpeed).ToString("F0") + " km/h";
            roadMovement.speed = carSpeed / 75f; 
        }
    }

    public void ResetSpeed()
    {
        carSpeed = 0f;
        spawnDelay = 2f;
        if (speedText != null)
            speedText.text = "Vel: " + carSpeed.ToString("F0") + " km/h";
        StopAllCoroutines();
        StartCoroutine(SpawnCars());
        StartCoroutine(IncreaseDifficulty());
    }
}

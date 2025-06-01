using UnityEngine;

public class DuckSpawner : MonoBehaviour
{
    public GameObject duckPrefab;
    public float spawnInterval = 2f;
    public float minX = -7f;
    public float maxX = 7f;
    public float spawnY = -4.5f;

    private float timer;

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            SpawnDuck();
            timer = spawnInterval;
        }
    }

    void SpawnDuck()
    {
        float randomX = Random.Range(minX, maxX);
        Vector3 spawnPosition = new Vector3(randomX, spawnY, 0);
        Instantiate(duckPrefab, spawnPosition, Quaternion.identity);
    }
}

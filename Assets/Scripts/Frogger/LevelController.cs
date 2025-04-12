using UnityEngine;
using System.Collections.Generic;

public class LevelController : MonoBehaviour
{
    public GameObject playerPrefab; 
    public GameObject guidePrefab; 
    public GameObject[] groundPositions; 

    private Queue<int> steps;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var p = groundPositions[Random.Range(0, groundPositions.Length - 1)].transform.position;
        Instantiate(playerPrefab, new Vector2(p.x, p.y) , Quaternion.identity);
        p = groundPositions[Random.Range(0, groundPositions.Length - 1)].transform.position;
        Instantiate(guidePrefab, new Vector2(p.x, p.y), Quaternion.identity);
    }

    // Update is called once per frame  
    void Update()
    {
        
    }
}

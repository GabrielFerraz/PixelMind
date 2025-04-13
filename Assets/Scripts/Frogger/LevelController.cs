using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class LevelController : MonoBehaviour
{
    public GameObject playerPrefab; 
    public GameObject guidePrefab; 
    public GameObject[] groundPositions; 

    private Queue<int> steps = new Queue<int>();
    private GameObject guideObject;
    private GameObject playerObject;
    private int currentPlayer = 0;
    private bool isEnabled = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        Vector2 p;
        var step = Random.Range(0, groundPositions.Length - 1);
        steps.Enqueue(step);
        p = groundPositions[step].transform.position;
        guideObject = Instantiate(guidePrefab, new Vector2(p.x, p.y), Quaternion.identity);
        int runs = 100;
        do
        {
            step = Random.Range(0, groundPositions.Length - 1);
        } while (steps.Contains(step) && runs-- > 0);
        currentPlayer = step;
        p = groundPositions[step].transform.position;
        playerObject = Instantiate(playerPrefab, new Vector2(p.x, p.y) , Quaternion.identity);
        StartCoroutine(TakeStep(2));
    }

    // Update is called once per frame  
    void Update()
    {
        
    }

    IEnumerator TakeStep(int stepNumber = 1) {
        int next;
        for (int i = 0; i < stepNumber; i++)
        {
            int runs = 100;
            do
            {
                next = Random.Range(0, groundPositions.Length - 1);
            } while (steps.Contains(next) && next != currentPlayer && runs-- > 0);
            steps.Enqueue(next);
            var p = groundPositions[next].transform.position;
            var target = new Vector2(p.x, p.y);
            runs = 100;
            while (guideObject.transform.position.x != target.x && runs-- > 0)
            {
                guideObject.transform.position = Vector2.MoveTowards(guideObject.transform.position, target, 3f * Time.deltaTime);

                yield return null;
            }
        }
        isEnabled = true;
    }

    IEnumerator JumpTo(int position) {
        var p = groundPositions[position].transform.position;
        var target = new Vector2(p.x, p.y);
        int runs = 100;
        while (playerObject.transform.position.x != target.x && runs-- > 0)
        {
            playerObject.transform.position = Vector2.MoveTowards(playerObject.transform.position, target, 3f * Time.deltaTime);

            yield return null;
        }
        isEnabled = true;
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(TakeStep(1));
    }

    public void MovePlayer(int position) {
        Debug.Log("MovePlayer: " + position);
        if (isEnabled) {
            isEnabled = false;
            currentPlayer = position;
            StartCoroutine(JumpTo(position));
        }
    }

    public void Test() {
        Debug.Log("Test");
    }
}   

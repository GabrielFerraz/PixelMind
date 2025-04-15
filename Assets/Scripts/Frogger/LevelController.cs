using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public class LevelController : MonoBehaviour
{
    public GameObject playerPrefab; 
    public GameObject guidePrefab; 
    public GameObject xPrefab; 
    public GameObject[] groundPositions; 

    private Queue<int> steps = new Queue<int>();
    private GameObject guideObject;
    private GameObject playerObject;
    private int currentPlayer = 0;
    private bool isEnabled = false;
    private int correctSteps = 0;
    private LineRenderer lineRenderer;
    private GameObject xObject; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        System.Random rnd = new System.Random();
        Vector2 p;
        var step = rnd.Next(0, groundPositions.Length);
        steps.Enqueue(step);
        p = groundPositions[step].transform.position;
        guideObject = Instantiate(guidePrefab, new Vector2(p.x, p.y), Quaternion.identity);
        int runs = 100;
        do
        {
            step = rnd.Next(0, groundPositions.Length);
        } while (steps.Contains(step) && runs-- > 0);
        currentPlayer = step;
        p = groundPositions[step].transform.position;
        playerObject = Instantiate(playerPrefab, new Vector2(p.x, p.y) , Quaternion.identity);
        
        lineRenderer = GetComponent<LineRenderer>();
        StartCoroutine(TakeStep(2));
    }

    // Update is called once per frame  
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
        }
    }

    IEnumerator TakeStep(int stepNumber = 1) {
        int next;
        System.Random rnd = new System.Random();
        for (int i = 0; i < stepNumber; i++)
        {
            int runs = 1000;
            do
            {
                next = rnd.Next(0, groundPositions.Length);
            } while (steps.Contains(next) && next != currentPlayer && runs-- > 0);
            steps.Enqueue(next);
            var p = groundPositions[next].transform.position;
            var target = new Vector2(p.x, p.y);
            runs = 1000;
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
        int runs = 1000;
        while (playerObject.transform.position.x != target.x && runs-- > 0)
        {
            playerObject.transform.position = Vector2.MoveTowards(playerObject.transform.position, target, 3f * Time.deltaTime);

            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(TakeStep(1));
    }

    public void MovePlayer(int position) {
        if (isEnabled) {
            Debug.Log("Player is enabled: " + position);
            isEnabled = false;
            if (checkNextPosition(position)) {
                StartCoroutine(JumpTo(position));
                currentPlayer = position;
                correctSteps++;
                resetError();
            } else {
                isEnabled = true;
                Debug.Log("Player is enabled: ");
                GenerateGuide();
                xObject = Instantiate(xPrefab, new Vector2(groundPositions[position].transform.position.x, groundPositions[position].transform.position.y), Quaternion.identity);
            }
            // StartCoroutine(JumpTo(position));
        }
    }

    private bool checkNextPosition(int position) {
        if (steps.Peek() == position) {
            steps.Dequeue();
            return true;
        } else {
            return false;
        }
    }

    private void GenerateGuide() {
        lineRenderer.enabled = true;
        var currentSteps = steps.ToArray();
        Vector3 start = groundPositions[currentPlayer].transform.position;
        Vector3 s1 = groundPositions[currentSteps[0]].transform.position;
        Vector3 s2 = groundPositions[currentSteps[1]].transform.position;
        Vector3 guide = guideObject.transform.position;
        Vector3[] positions = new Vector3[steps.Count + 1];
        positions[0] = start;
        positions[1] = s1;
        positions[2] = s2;
        positions[3] = guide;
        lineRenderer.positionCount = positions.Length;
        lineRenderer.SetPositions(positions);
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;
    }

    private void resetError() {
        Debug.Log("Reset error" + xObject != null);
        if (xObject != null) {
            Destroy(xObject);
        }
        lineRenderer.enabled = false;
    }
}   

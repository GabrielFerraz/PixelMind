using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using TMPro;

public class LevelController : MonoBehaviour
{
    public GameObject playerPrefab; 
    public GameObject guidePrefab; 
    public GameObject xPrefab; 
    public GameObject[] groundPositions; 
    public TextMeshProUGUI stepsCountText;

    private Queue<int> steps = new Queue<int>();
    private GameObject guideObject;
    private GameObject playerObject;
    private int currentPlayer = 0;
    private bool isEnabled = false;
    private int correctSteps = 0;
    private LineRenderer lineRenderer;
    private List<GameObject> xObjects; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Random.InitState((int)System.DateTime.Now.Ticks);
        Vector2 pos;
        var step = Random.Range(0, (groundPositions.Length*100) + 1) % groundPositions.Length;
        steps.Enqueue(step);
        pos = groundPositions[step].transform.position;
        guideObject = Instantiate(guidePrefab, new Vector2(pos.x, pos.y), Quaternion.identity);
        int runs = 100;
        do
        {
            step = Random.Range(0, (groundPositions.Length*100) + 1) % groundPositions.Length;
            Debug.Log("Step: " + step);
        } while (steps.Contains(step) && runs-- > 0);
        currentPlayer = step;
        pos = groundPositions[step].transform.position;
        playerObject = Instantiate(playerPrefab, new Vector2(pos.x, pos.y) , Quaternion.identity);
        
        lineRenderer = GetComponent<LineRenderer>();
        StartCoroutine(TakeStep(2));
        stepsCountText.text = "Steps: " + correctSteps + "/30";
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
        Random.InitState((int)System.DateTime.Now.Ticks);
        for (int i = 0; i < stepNumber; i++)
        {
            int runs = 1000;
            do
            {
                next = Random.Range(0, (groundPositions.Length*100) + 1) % groundPositions.Length;
                Debug.Log("next: " + next);
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
        var stepsNo = correctSteps != 0 && correctSteps % 10 == 1 ? 2 : 1;
        StartCoroutine(TakeStep(stepsNo));
    }

    public void MovePlayer(int position) {
        if (isEnabled) {
            Debug.Log("Player is enabled: " + position);
            isEnabled = false;
            if (checkNextPosition(position)) {
                StartCoroutine(JumpTo(position));
                currentPlayer = position;
                correctSteps++;
                stepsCountText.text = "Steps: " + correctSteps + "/30";
                resetError();
            } else {
                resetError();
                isEnabled = true;
                Debug.Log("Player is enabled: ");
                GenerateGuide();
                var xObject = Instantiate(xPrefab, new Vector2(groundPositions[position].transform.position.x, groundPositions[position].transform.position.y), Quaternion.identity);
                xObjects.Add(xObject);
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
        if (xObjects.Count > 0) {
            foreach (var xObject in xObjects) {
                Destroy(xObject);
            }
            xObjects.Clear();
        }
        lineRenderer.enabled = false;
    }
}   

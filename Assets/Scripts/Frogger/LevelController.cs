using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using TMPro;
using DigitalRubyShared;

public class LevelController : MonoBehaviour
{
    public GameObject playerPrefab; 
    public GameObject guidePrefab; 
    public GameObject xPrefab; 
    public GameObject[] groundPositions; 
    public TextMeshProUGUI stepsCountText;
    public TextMeshProUGUI scoreText;
    public Camera myCamera;
    public int maxSteps = 30;

    private Queue<int> steps = new Queue<int>();
    private GameObject guideObject;
    private GameObject playerObject;
    private int currentPlayer = 0;
    private bool isEnabled = false;
    private int correctSteps = 0;
    private int incorrectSteps = 0;
    private LineRenderer lineRenderer;
    private List<GameObject> xObjects = new List<GameObject>(); 
    private int score = 0;
    private int stepsAhead = 2;
    private TapGestureRecognizer tapGesture;
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
        stepsCountText.text = correctSteps + "/30";
        CreateTapGesture();

    }

    private void CreateTapGesture()
    {
        tapGesture = new TapGestureRecognizer();
        tapGesture.StateUpdated += TapGestureCallback;
        FingersScript.Instance.AddGesture(tapGesture);
    }

    // Update is called once per frame  
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            GoHome();
        }
    }

    IEnumerator TakeStep(int stepNumber = 1) {
        if (correctSteps >= maxSteps) {
            var gameData = SaveSystem.Load();
            gameData.currentHighScore = score;
            gameData.currentTimestamp = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            gameData.currentLevelName = "Frogger";
            if (gameData.frogger == null)
            {
                gameData.frogger = new List<GameSessionData>();
            }
            gameData.frogger.Add(new GameSessionData { score = score, timestamp = gameData.currentTimestamp });
            gameData.currentLevel = gameData.frogger;
            SaveSystem.Save(gameData);
            var output = JsonUtility.ToJson(gameData, true);
            Debug.Log(output);
            UnityEngine.SceneManagement.SceneManager.LoadScene("Score");
            yield break;
        }
        int next;
        Random.InitState((int)System.DateTime.Now.Ticks);
        for (int i = 0; i < stepNumber; i++)
        {
            int runs = 1000;
            do
            {
                next = Random.Range(0, (groundPositions.Length*100) + 1) % groundPositions.Length;
                Debug.Log("next: " + next);
            } while ((steps.Contains(next) || next == currentPlayer) && runs-- > 0);
            steps.Enqueue(next);
            var p = groundPositions[next].transform.position;
            var target = new Vector2(p.x, p.y);
            runs = 1000;
            while (guideObject.transform.position.x != target.x && runs-- > 0)
            {
                guideObject.transform.position = Vector2.MoveTowards(guideObject.transform.position, target, 5f * Time.deltaTime);

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
            playerObject.transform.position = Vector2.MoveTowards(playerObject.transform.position, target, 5f * Time.deltaTime);

            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        var stepsNo = correctSteps > 0 && correctSteps % 10 == 0 ? 2 : 1;
        Debug.Log("incorrectSteps: " + incorrectSteps);
        if (incorrectSteps < 2 || steps.Count >= 2){
            StartCoroutine(TakeStep(stepsNo));
            stepsAhead += stepsNo > 1 ? 1 : 0;
        } else {
            incorrectSteps = 0;
        }
    }

    public void MovePlayer(int position) {
        if (isEnabled) {
            isEnabled = false;
            if (checkNextPosition(position)) {
                StartCoroutine(JumpTo(position));
                currentPlayer = position;
                correctSteps++;
                stepsCountText.text = correctSteps + "/30";
                score += 100 * stepsAhead;
                scoreText.text = "" + score;
                resetError();
            } else {
                resetError();
                isEnabled = true;
                GenerateGuide();
                var xObject = Instantiate(xPrefab, new Vector2(groundPositions[position].transform.position.x, groundPositions[position].transform.position.y), Quaternion.identity);
                xObjects.Add(xObject);
                incorrectSteps++;
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
        Vector3 guide = guideObject.transform.position;
        Vector3[] positions = new Vector3[steps.Count + 2];
        positions[0] = start;
        for (int i = 0; i < steps.Count; i++) {
            positions[i + 1] = groundPositions[currentSteps[i]].transform.position;
        }
        positions[positions.Length - 1] = guide;
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

    public void GoHome() {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
    }
    private void TapGestureCallback(GestureRecognizer gesture)
    {
        if (gesture.State == GestureRecognizerState.Ended)
        {
            Debug.Log("Tap at: " + gesture.FocusX + ", " + gesture.FocusY);
            CheckTap(gesture.FocusX, gesture.FocusY, 0.5f);
        }
    }

    private void CheckTap(float screenX, float screenY, float radius)
    {
        var pos = new Vector3(screenX, screenY, 0.0f);
        pos = myCamera.ScreenToWorldPoint(pos);
        Debug.Log("World position: " + pos);
        RaycastHit2D[] hits = Physics2D.CircleCastAll(pos, radius, Vector2.zero);
        foreach (RaycastHit2D h in hits)
        {
            if (h.transform.gameObject.CompareTag("Ground")) {
                var position = System.Array.IndexOf(groundPositions, h.transform.gameObject);
                MovePlayer(position);
            }
        }
    }
}   

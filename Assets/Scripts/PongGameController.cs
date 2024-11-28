using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;

public class PongGameController : MonoBehaviour
{
    // public static GameController instance;
    public string playerName;
    public int playerScore;

    public GameObject ball;
    public GameObject wallOne;
    public GameObject wallTwo;
    public float forceMagnitude = 7f;
    public TextMeshProUGUI leftScore;
    public TextMeshProUGUI rightScore;
    public TextMeshProUGUI endgame;
    public int WinningScore = 3;


    private int left = 0;
    private int right = 0;

    private Rigidbody rb;
    private Vector3 initialPosition;
    public float slowDownFactor = 0.5f; // Factor to slow down the game (e.g., 0.1 for 10% speed)
    public float slowDuration = 1f; // Duration to slow down (in seconds)
    // Start is called before the first frame update
    void Awake(){
        rb = ball.GetComponent<Rigidbody>();
    }
    void Start()
    {
        
        rb = ball.GetComponent<Rigidbody>();
        initialPosition = rb.transform.position;
        Vector3 randomDirection = new Vector3(Random.Range(0.5f, 1f), 0, Random.Range(0.5f, 1f)).normalized;
        rb.AddForce(randomDirection * forceMagnitude, ForceMode.Impulse);
        Time.timeScale = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (left >= WinningScore || right >= WinningScore) {
            // Debug.Log("game ends");
            // Debug.Log(left + " : " + right);
            // endgame.text = left + " : " + right + "\nGame End";
            // endgame.gameObject.SetActive(true);
            // Time.timeScale = 0f;
            // PlayerPrefs.SetInt("left", left);
            // PlayerPrefs.SetInt("right", right);
            // PlayerPrefs.Save(); // Make sure to save the changes
            // SceneManager.LoadScene("MainMenu");
            EndGame(left, right);

        }
    }

    public static void EndGame(int leftScore=0, int rightScore=0){
        PlayerPrefs.SetInt("left", leftScore);
        PlayerPrefs.SetInt("right", rightScore);
        PlayerPrefs.Save(); // Make sure to save the changes
        SceneManager.LoadScene("MainMenu");
    }
    private void FixedUpdate() {
        rb.velocity = rb.velocity.normalized * forceMagnitude;
    }
    public void leftGoal(){
        left += 1;
        leftScore.text = left.ToString();
        resetBall(1);
        if (left < WinningScore) {
        SlowDownGame();
        }

    }
    public void rightGoal(){
        right += 1;
        rightScore.text = right.ToString();
        resetBall(-1);
        if (right < WinningScore) {

        SlowDownGame();
        }
    }
    public void resetBall(int direction) {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.transform.position = initialPosition;
        Vector3 randomDirection = new Vector3(direction * Random.Range(0.5f, 1f), 0, Random.Range(0.5f, 1f)).normalized;
        rb.AddForce(randomDirection * forceMagnitude, ForceMode.Impulse);
    }

        public void SlowDownGame()
    {
        StartCoroutine(SlowDownCoroutine());
    }

    private IEnumerator SlowDownCoroutine()
    {
        // Set time scale to slow down the game
        Time.timeScale = slowDownFactor;

        // Wait for the specified duration
        yield return new WaitForSecondsRealtime(slowDuration);

        // Reset time scale back to normal
        Time.timeScale = 1f;
    }
}

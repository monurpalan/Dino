using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }
    public float gameSpeed { get; private set; }

    [Header("Game Settings")]
    [SerializeField] private float initialGameSpeed = 5f;
    [SerializeField] private float gameSpeedIncrease = 0.1f;

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI gameOverText;
    [SerializeField] private Button restartButton;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;

    private Player player;
    private Spawner spawner;
    private float score;
    private const string HIGH_SCORE_KEY = "HighScore";
    private const string SCORE_FORMAT = "D5"; //

    private void Awake()
    {
        InitializeSingleton();
    }

    private void Start()
    {
        FindGameComponents();
        StartNewGame();
    }

    private void Update()
    {
        if (!enabled) return; // Script devre dışıysa Update çalışmaz

        UpdateGameSpeed();
        UpdateScore();
        UpdateScoreUI();
    }

    private void InitializeSingleton()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void FindGameComponents()
    {
        player = FindObjectOfType<Player>();
        spawner = FindObjectOfType<Spawner>();
    }

    public void StartNewGame()
    {
        ClearObstacles();
        ResetGameState();
        UpdateUI(false);
        float highScore = PlayerPrefs.GetFloat(HIGH_SCORE_KEY, 0);
        highScoreText.text = Mathf.FloorToInt(highScore).ToString(SCORE_FORMAT);
    }

    public void GameOver()
    {
        MusicManager.instance.PlayDeathSound();
        StopGame();
        UpdateUI(true);
        UpdateHighScore();
    }

    private void StopGame()
    {
        gameSpeed = 0f;
        enabled = false;
        player.gameObject.SetActive(false);
        spawner.gameObject.SetActive(false);
    }

    private void ClearObstacles()
    {
        Obstacle[] obstacles = FindObjectsOfType<Obstacle>();
        foreach (Obstacle obstacle in obstacles)
        {
            Destroy(obstacle.gameObject);
        }
    }

    private void ResetGameState()
    {
        score = 0f;
        gameSpeed = initialGameSpeed;
        enabled = true;
        player.gameObject.SetActive(true);
        spawner.gameObject.SetActive(true);
    }

    private void UpdateUI(bool isGameOver)
    {
        gameOverText.gameObject.SetActive(isGameOver);
        restartButton.gameObject.SetActive(isGameOver);
    }

    private void UpdateGameSpeed()
    {
        gameSpeed += gameSpeedIncrease * Time.deltaTime;
    }

    private void UpdateScore()
    {
        score += gameSpeed * Time.deltaTime;
    }

    private void UpdateScoreUI()
    {
        scoreText.text = Mathf.FloorToInt(score).ToString(SCORE_FORMAT);
    }

    private void UpdateHighScore()
    {
        float highScore = PlayerPrefs.GetFloat(HIGH_SCORE_KEY, 0);
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetFloat(HIGH_SCORE_KEY, Mathf.FloorToInt(highScore));
        }
        highScoreText.text = Mathf.FloorToInt(highScore).ToString(SCORE_FORMAT);
    }
}
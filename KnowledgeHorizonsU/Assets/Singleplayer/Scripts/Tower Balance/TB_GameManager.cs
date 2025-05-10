using UnityEngine;
using Cinemachine;
using System.Collections;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TB_GameManager : MonoBehaviour
{
    public static TB_GameManager Instance;

    [Header("References")]
    public GameObject blockPrefab;
    public Transform basePlatform;
    public CinemachineVirtualCamera virtualCamera;
    public LineRenderer ropeRenderer;

    [Header("Settings")]
    public float blockHeight = 1f;
    public float swingRadius = 3f;
    public float cameraMoveSpeed = 3f;
    public float safeDistance = 4f;
    public float spawnDelay = 0.5f;
    public int blocksBeforeMovement = 3;
    public float cameraMoveDuration = 0.5f;

    [Header("Scoreboard UI")]
    public TMP_Text currentScoreText;
    public TMP_Text highScoreText;
    public GameObject scoreboardPanel;

    [Header("Game Over")]
    public GameObject gameOverPanel;
    public GameObject landingMarkerPrefab;
    public float gameOverDelay = 1.5f;

    [Header("Pause Menu")]
    public GameObject pauseMenuPanel;
    private bool isPaused = false;

    [Header("Scoring")]
    public int scorePerBlock = 1;

    [Header("Audio")]
    public AudioSource backgroundMusic;
    public Slider volumeSlider;

    private int currentScore = 0;
    private int highScore = 0;
    private int sessionHighScore = 0;
    private const string HIGH_SCORE_KEY = "BlockStacker_HighScore";
    private bool isFirstRound = true;
    private bool isGameOver = false;
    private int totalBlocksPlaced = 0; // Tracks ALL blocks placed (including first block)

    private GameObject currentBlock;
    private Transform spawnPivot;
    private Transform cameraTarget;
    private float currentTowerHeight;
    private bool canPlaceBlock = true;
    private int blocksPlaced = 0; // Tracks blocks in current round
    private float baseY;
    private CinemachineFramingTransposer transposer;
    private Vector3? fallingBlockPosition = null;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        baseY = basePlatform.position.y;
        InitializeCameraSystem();
        CreateSpawnPivot();
        LoadHighScore();
        sessionHighScore = highScore;

        // Initialize audio
        float savedVolume = PlayerPrefs.GetFloat("MasterVolume", 0.7f);
        backgroundMusic.volume = savedVolume;
        backgroundMusic.loop = true; // Enable looping
        backgroundMusic.Play(); // Start playing immediately

        if (volumeSlider != null)
        {
            volumeSlider.value = savedVolume;
            volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
        }

        UpdateScoreUI();
        SpawnNewBlock();
    }

    void InitializeCameraSystem()
    {
        Camera.main.transform.position = new Vector3(0, baseY + 7.5f, -10);
        virtualCamera.m_Lens.OrthographicSize = 7.5f;

        cameraTarget = new GameObject("CameraTarget").transform;
        cameraTarget.position = new Vector3(0, baseY + 7.5f, 0);
        virtualCamera.Follow = cameraTarget;

        transposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        transposer.m_YDamping = 0;
    }

    void CreateSpawnPivot()
    {
        spawnPivot = new GameObject("SpawnPivot").transform;
        UpdateSpawnPosition();
    }

    void LoadHighScore()
    {
        highScore = PlayerPrefs.GetInt(HIGH_SCORE_KEY, 0);
    }

    void SaveHighScore()
    {
        PlayerPrefs.SetInt(HIGH_SCORE_KEY, highScore);
        PlayerPrefs.Save();
    }

    void UpdateScoreUI()
    {
        currentScoreText.text = "Blocks: " + currentScore;
        highScoreText.text = "Best: " + (isFirstRound ? currentScore : sessionHighScore);
        scoreboardPanel.SetActive(true);
    }

    void UpdateSpawnPosition()
    {
        float cameraTop = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 1f, 0)).y;
        spawnPivot.position = new Vector3(0, cameraTop - (blockHeight / 2f), 0);
    }

    void Update()
    {
        if (isGameOver) return;

        if (Input.GetKeyDown(KeyCode.Space) && canPlaceBlock)
        {
            StartCoroutine(BlockPlacementRoutine());
        }

        if (Input.GetKeyDown(KeyCode.Escape) && !isGameOver)
        {
            TogglePause();
        }

        UpdateRopeVisual();
    }
    public void TogglePause()
    {
        isPaused = !isPaused;
        pauseMenuPanel.SetActive(isPaused);
        Time.timeScale = isPaused ? 0f : 1f;

        // Don't modify volume when pausing - let it play normally
        // backgroundMusic.volume = isPaused ? volumeSlider.value * 0.5f : volumeSlider.value;
    }
    public void OnVolumeChanged(float value)
    {
        backgroundMusic.volume = value;
        PlayerPrefs.SetFloat("MasterVolume", value);
    }

    public void ResumeGame()
    {
        TogglePause();
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Islands");
    }

    IEnumerator BlockPlacementRoutine()
    {
        canPlaceBlock = false;

        TB_MovingBlock movingBlock = currentBlock.GetComponent<TB_MovingBlock>();
        movingBlock.Place();
        blocksPlaced++;
        totalBlocksPlaced++; // Track total blocks placed

        AddScore(scorePerBlock);
        yield return new WaitForSeconds(0.3f);

        currentTowerHeight = Mathf.Max(currentTowerHeight, currentBlock.transform.position.y - baseY);

        if (blocksPlaced >= blocksBeforeMovement)
        {
            float targetY = cameraTarget.position.y + safeDistance;
            yield return StartCoroutine(SmoothCameraMove(cameraTarget.position.y, targetY));
        }

        UpdateSpawnPosition();
        yield return new WaitForSeconds(spawnDelay);
        SpawnNewBlock();

        canPlaceBlock = true;
    }

    public void HandleFallingBlock(Vector3 landingPosition)
    {
        // Skip game over if it's the first block (totalBlocksPlaced = 1)
        if (totalBlocksPlaced <= 1) return;

        if (isGameOver) return;

        isGameOver = true;
        fallingBlockPosition = landingPosition;
        StartCoroutine(GameOverSequence());
    }

    private IEnumerator GameOverSequence()
    {
        // Show landing marker
        if (landingMarkerPrefab && fallingBlockPosition.HasValue)
        {
            Instantiate(landingMarkerPrefab, fallingBlockPosition.Value, Quaternion.identity);
        }

        yield return new WaitForSeconds(gameOverDelay);

        // Show game over UI
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f; // Pause game
    }

    public void RetryGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    void AddScore(int points)
    {
        currentScore += points;

        if (currentScore > sessionHighScore)
        {
            sessionHighScore = currentScore;
            if (sessionHighScore > highScore)
            {
                highScore = sessionHighScore;
                SaveHighScore();
            }
        }
        UpdateScoreUI();
    }

    public void StartNewGame()
    {
        isFirstRound = false;
        currentScore = 0;
        blocksPlaced = 0;
        totalBlocksPlaced = 0; // Reset counter for new game
        currentTowerHeight = 0;
        isGameOver = false;
        UpdateScoreUI();

        // Reset camera position
        cameraTarget.position = new Vector3(0, baseY + 7.5f, 0);
        UpdateSpawnPosition();
        SpawnNewBlock();
    }

    IEnumerator SmoothCameraMove(float startY, float targetY)
    {
        float elapsed = 0f;
        while (elapsed < cameraMoveDuration)
        {
            float newY = Mathf.Lerp(startY, targetY, elapsed / cameraMoveDuration);
            cameraTarget.position = new Vector3(0, newY, 0);
            elapsed += Time.deltaTime;
            UpdateSpawnPosition();
            yield return null;
        }
        cameraTarget.position = new Vector3(0, targetY, 0);
    }

    void SpawnNewBlock()
    {
        Vector3 spawnPos = new Vector3(0, spawnPivot.position.y - swingRadius, 0);
        currentBlock = Instantiate(blockPrefab, spawnPos, Quaternion.identity);
        currentBlock.GetComponent<TB_MovingBlock>().Initialize(spawnPivot, swingRadius);
    }

    void UpdateRopeVisual()
    {
        if (currentBlock != null && !currentBlock.GetComponent<TB_MovingBlock>().IsPlaced)
        {
            ropeRenderer.positionCount = 2;
            ropeRenderer.SetPosition(0, spawnPivot.position);
            ropeRenderer.SetPosition(1, currentBlock.transform.position);
        }
        else
        {
            ropeRenderer.positionCount = 0;
        }
    }

    void LateUpdate()
    {
        UpdateSpawnPosition();
    }
}
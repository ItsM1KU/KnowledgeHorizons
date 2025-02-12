using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class FB_GameManager : MonoBehaviour
{
    [Header("Game Timer")]
    public float gameTime = 60f;
    private float currentTime;
    private bool isTimerRunning = false;

    [Header("UI Elements")]
    public TMP_Text timerText;
    public GameObject questionPanel;
    public TMP_Text questionText;
    public GameObject endGamePanel;
    public TMP_Text scoreText;

    [Header("Buttons")]
    public GameObject retryButton;
    public GameObject mainMenuButton;

    [Header("Flag Spawning")]
    public GameObject flagPrefab;
    public Transform flagSpawnPoint;
    public float spawnSpacing = 3.0f;

    [System.Serializable]
    public class Question
    {
        public string questionText;
        public string correctCountry;
    }

    [Header("Questions Setup")]
    public Question[] questions;

    private int currentQuestionIndex = 0;
    private int correctCount = 0;
    private int incorrectCount = 0;
    private bool gameOver = false;
    private bool roundCompleted = false; // Will be set to true when at least one flag is caught this round

    // ✅ Country name to ISO code mapping
    private Dictionary<string, string> countryCodeMap = new Dictionary<string, string>
    {
        { "India", "IN" },
        { "Afghanistan", "AF" },
        { "Argentina", "AR" },
        { "Brazil", "BR" },
        { "Canada", "CA" },
        { "China", "CN" },
        { "France", "FR" },
        { "Germany", "DE" },
        { "Italy", "IT" },
        { "Japan", "JP" },
        { "Mexico", "MX" },
        { "Russia", "RU" },
        { "United States", "US" },
        { "United Kingdom", "GB" },
        { "Australia", "AU" },
        { "Bangladesh", "BD" },
        { "South Korea", "KR" },
        { "Turkey", "TR" },
        { "South Africa", "ZA" },
        { "Spain", "ES" },
        { "Egypt", "EG" },
        { "Saudi Arabia", "SA" },
        { "United Arab Emirates", "AE" },
        { "Indonesia", "ID" },
        { "Pakistan", "PK" },
        { "Vietnam", "VN" },
        { "Philippines", "PH" },
        { "Thailand", "TH"},
        { "Malaysia", "MY" },
        { "Iran", "IR" },
        { "Iraq", "IQ" },
        { "Syria", "SY" },
        { "Lebanon", "LB" },
        { "Greece", "GR" },
        { "Portugal", "PT" },
        { "Netherlands", "NL" },
        { "Belgium", "BE" },
        { "Switzerland", "CH" },
        { "Sweden", "SE" },
        { "Norway", "NO" },
        { "Denmark", "DK" },
        { "Finland", "FI" },
        { "Poland", "PL" },
        { "Ukraine", "UA" },
        { "Nigeria", "NG" },
        { "Ethiopia", "ET" },
        { "Kenya", "KE" },
        { "Zimbabwe", "ZW" },
        { "New Zealand", "NZ" },
        { "Sri Lanka", "LK" }
    };

    void Start()
    {
        currentTime = gameTime;
        questionPanel.SetActive(false);
        endGamePanel.SetActive(false);
        StartCoroutine(GameLoop());
    }

    void Update()
    {
        if (isTimerRunning && currentTime > 0 && !gameOver)
        {
            currentTime -= Time.deltaTime;
            timerText.text = "Time: " + Mathf.Ceil(currentTime).ToString();
        }
    }

    IEnumerator GameLoop()
    {
        while (currentTime > 0 && currentQuestionIndex < questions.Length && !gameOver)
        {
            // Reset round flag.
            roundCompleted = false;

            // Get the current question (cycle through questions if needed)
            Question currentQuestion = questions[currentQuestionIndex];

            // Show the question panel.
            ShowQuestion(currentQuestion);
            yield return new WaitForSeconds(5f); // Allow time for the player to read.

            HideQuestion();
            isTimerRunning = true;
            yield return new WaitForSeconds(1f); // Brief delay before spawning flags.

            // Spawn flags for the round.
            SpawnFlagsForQuestion(currentQuestion.correctCountry);

            // Wait until the round ends:
            // If a flag is caught, RegisterFlagCaught() will set isTimerRunning = false.
            // If no flag is caught and a flag falls, FlagMissed() will end the game.
            yield return new WaitUntil(() => !isTimerRunning || gameOver);

            if (!gameOver)
            {
                // End the round by destroying any remaining flag objects.
                DestroyAllFlags();

                // Advance to the next question.
                currentQuestionIndex++;
            }
        }

        if (!gameOver)
        {
            EndGame();
        }
    }

    void ShowQuestion(Question question)
    {
        isTimerRunning = false; // Pause timer while showing question.
        questionPanel.SetActive(true);
        questionText.text = question.questionText;
    }

    void HideQuestion()
    {
        questionPanel.SetActive(false);
    }

    void SpawnFlagsForQuestion(string correctCountry)
    {
        // Build a list of 4 flags (1 correct, 3 incorrect).
        List<string> selectedFlags = new List<string> { correctCountry };
        while (selectedFlags.Count < 4)
        {
            string randomCountry = GetRandomCountryExcluding(selectedFlags);
            selectedFlags.Add(randomCountry);
        }
        selectedFlags = ShuffleList(selectedFlags);

        // Evenly distribute flags across the spawn area.
        float totalWidth = spawnSpacing * 3f; // For 4 flags, there are 3 intervals.
        float startX = -totalWidth / 2f;
        for (int i = 0; i < 4; i++)
        {
            float spawnPosX = startX + (i * spawnSpacing);
            Vector3 spawnPos = new Vector3(spawnPosX, flagSpawnPoint.position.y, 0f);

            GameObject flagObj = Instantiate(flagPrefab, spawnPos, Quaternion.identity);
            // Make sure your flag prefab is tagged "Flag"
            FB_Flag flagScript = flagObj.GetComponent<FB_Flag>();

            if (countryCodeMap.ContainsKey(selectedFlags[i]))
            {
                Debug.Log("Spawning Flag: " + selectedFlags[i] + " with Code: " + countryCodeMap[selectedFlags[i]]);
                flagScript.SetFlag(selectedFlags[i], countryCodeMap[selectedFlags[i]]);
            }
            else
            {
                Debug.LogWarning("No country code found for: " + selectedFlags[i]);
            }
        }
    }

    public void RegisterFlagCaught(string flagCountry)
    {
        // Called by basket when a flag is caught.
        if (!gameOver && currentQuestionIndex < questions.Length)
        {
            Question currentQuestion = questions[currentQuestionIndex];
            if (flagCountry == currentQuestion.correctCountry)
            {
                correctCount++;
                Debug.Log("Correct!");
            }
            else
            {
                incorrectCount++;
                Debug.Log("Incorrect!");
            }

            // Mark the round as completed so that missed events from other flags are ignored.
            roundCompleted = true;

            // End this round.
            isTimerRunning = false;

            // Destroy any remaining flags.
            DestroyAllFlags();
        }
    }

    public void FlagMissed()
    {
        // Called by a flag when it falls off-screen.
        // Only end the game if no flag was caught in this round.
        if (!gameOver && !roundCompleted)
        {
            Debug.Log("Flag missed! Game Over.");
            gameOver = true;
            StopAllCoroutines();
            EndGame();
        }
    }

    void EndGame()
    {
        isTimerRunning = false;
        endGamePanel.SetActive(true);
        scoreText.text = "Game Over!\nCorrect: " + correctCount + "\nIncorrect: " + incorrectCount;
        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Islands");
    }

    // Helper: returns a random country not already in excludeCountries.
    string GetRandomCountryExcluding(List<string> excludeCountries)
    {
        List<string> allCountries = new List<string>(countryCodeMap.Keys);
        string randomCountry;
        do
        {
            randomCountry = allCountries[Random.Range(0, allCountries.Count)];
        } while (excludeCountries.Contains(randomCountry));
        return randomCountry;
    }

    // Helper: shuffles the list.
    List<T> ShuffleList<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int randomIndex = Random.Range(i, list.Count);
            (list[i], list[randomIndex]) = (list[randomIndex], list[i]);
        }
        return list;
    }

    // Helper: destroys all flag objects currently in the scene.
    void DestroyAllFlags()
    {
        GameObject[] remainingFlags = GameObject.FindGameObjectsWithTag("Flag");
        foreach (GameObject flag in remainingFlags)
        {
            Destroy(flag);
        }
    }
}

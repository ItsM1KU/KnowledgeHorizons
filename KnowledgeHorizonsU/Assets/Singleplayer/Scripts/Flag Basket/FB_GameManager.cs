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
    public List<Question> questions; // Changed to List for shuffling

    private List<Question> shuffledQuestions; // Stores shuffled questions
    private int currentQuestionIndex = 0;
    private int correctCount = 0;
    private int incorrectCount = 0;
    private bool gameOver = false;
    private bool roundCompleted = false;

    // ✅ Country name to ISO code mapping
    private Dictionary<string, string> countryCodeMap = new Dictionary<string, string>
    {
        { "India", "IN" }, { "Afghanistan", "AF" }, { "Argentina", "AR" },
        { "Brazil", "BR" }, { "Canada", "CA" }, { "China", "CN" },
        { "France", "FR" }, { "Germany", "DE" }, { "Italy", "IT" },
        { "Japan", "JP" }, { "Mexico", "MX" }, { "Russia", "RU" },
        { "United States", "US" }, { "United Kingdom", "GB" }, { "Australia", "AU" },
        { "Bangladesh", "BD" }, { "South Korea", "KR" }, { "Turkey", "TR" },
        { "South Africa", "ZA" }, { "Spain", "ES" }, { "Egypt", "EG" },
        { "Saudi Arabia", "SA" }, { "United Arab Emirates", "AE" }, { "Indonesia", "ID" },
        { "Pakistan", "PK" }, { "Vietnam", "VN" }, { "Philippines", "PH" },
        { "Thailand", "TH" }, { "Malaysia", "MY" }, { "Iran", "IR" }, { "Iraq", "IQ" },
        { "Syria", "SY" }, { "Lebanon", "LB" }, { "Greece", "GR" }, { "Portugal", "PT" },
        { "Netherlands", "NL" }, { "Belgium", "BE" }, { "Switzerland", "CH" },
        { "Sweden", "SE" }, { "Norway", "NO" }, { "Denmark", "DK" }, { "Finland", "FI" },
        { "Poland", "PL" }, { "Ukraine", "UA" }, { "Nigeria", "NG" }, { "Ethiopia", "ET" },
        { "Kenya", "KE" }, { "Zimbabwe", "ZW" }, { "New Zealand", "NZ" },
        { "Sri Lanka", "LK" }
    };

    void Start()
    {
        currentTime = gameTime;
        questionPanel.SetActive(false);
        endGamePanel.SetActive(false);

        // **Shuffle Questions**
        shuffledQuestions = new List<Question>(questions);
        shuffledQuestions = ShuffleList(shuffledQuestions);

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
        while (currentTime > 0 && currentQuestionIndex < shuffledQuestions.Count && !gameOver)
        {
            // Reset round flag.
            roundCompleted = false;

            // Get a **randomized** question from the shuffled list
            Question currentQuestion = shuffledQuestions[currentQuestionIndex];

            // Show the question panel.
            ShowQuestion(currentQuestion);
            yield return new WaitForSeconds(5f);

            HideQuestion();
            isTimerRunning = true;
            yield return new WaitForSeconds(1f);

            // Spawn flags for this round.
            SpawnFlagsForQuestion(currentQuestion.correctCountry);

            // Wait for player input.
            yield return new WaitUntil(() => !isTimerRunning || gameOver);

            if (!gameOver)
            {
                DestroyAllFlags();
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
        isTimerRunning = false;
        questionPanel.SetActive(true);
        questionText.text = question.questionText;
    }

    void HideQuestion()
    {
        questionPanel.SetActive(false);
    }

    void SpawnFlagsForQuestion(string correctCountry)
    {
        List<string> selectedFlags = new List<string> { correctCountry };
        while (selectedFlags.Count < 4)
        {
            string randomCountry = GetRandomCountryExcluding(selectedFlags);
            selectedFlags.Add(randomCountry);
        }
        selectedFlags = ShuffleList(selectedFlags);

        float totalWidth = spawnSpacing * 3f;
        float startX = -totalWidth / 2f;
        for (int i = 0; i < 4; i++)
        {
            float spawnPosX = startX + (i * spawnSpacing);
            Vector3 spawnPos = new Vector3(spawnPosX, flagSpawnPoint.position.y, 0f);

            GameObject flagObj = Instantiate(flagPrefab, spawnPos, Quaternion.identity);
            FB_Flag flagScript = flagObj.GetComponent<FB_Flag>();

            if (countryCodeMap.ContainsKey(selectedFlags[i]))
            {
                flagScript.SetFlag(selectedFlags[i], countryCodeMap[selectedFlags[i]]);
            }
        }
    }

    public void RegisterFlagCaught(string flagCountry)
    {
        if (!gameOver && currentQuestionIndex < shuffledQuestions.Count)
        {
            Question currentQuestion = shuffledQuestions[currentQuestionIndex];
            if (flagCountry == currentQuestion.correctCountry)
            {
                correctCount++;
            }
            else
            {
                incorrectCount++;
            }

            roundCompleted = true;
            isTimerRunning = false;
            DestroyAllFlags();
        }
    }

    public void FlagMissed()
    {
        if (!gameOver && !roundCompleted)
        {
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

    List<T> ShuffleList<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int randomIndex = Random.Range(i, list.Count);
            (list[i], list[randomIndex]) = (list[randomIndex], list[i]);
        }
        return list;
    }

    void DestroyAllFlags()
    {
        GameObject[] remainingFlags = GameObject.FindGameObjectsWithTag("Flag");
        foreach (GameObject flag in remainingFlags)
        {
            Destroy(flag);
        }
    }
}

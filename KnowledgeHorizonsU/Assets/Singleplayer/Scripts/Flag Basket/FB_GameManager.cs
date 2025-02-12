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
    public TMP_Text feedbackText;
    private CanvasGroup feedbackCanvasGroup;

    [Header("Buttons")]
    public GameObject retryButton;
    public GameObject mainMenuButton;

    [Header("Flag Spawning")]
    public GameObject flagPrefab;
    public Transform flagSpawnPoint;
    public float spawnSpacing = 3.0f;

    [Header("Audio Effects")]
    public AudioSource correctSound;
    public AudioSource wrongSound;

    [Header("Basket Visual Feedback")]
    public SpriteRenderer basketSprite; 
    public Color correctColor = Color.green;
    public Color wrongColor = Color.red;
    private Color originalBasketColor;

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

        // ✅ Get the CanvasGroup from FeedbackText for fade effect
        feedbackCanvasGroup = feedbackText.GetComponent<CanvasGroup>();
        if (feedbackCanvasGroup == null)
        {
            feedbackCanvasGroup = feedbackText.gameObject.AddComponent<CanvasGroup>();
        }
        feedbackCanvasGroup.alpha = 0; // Make sure it's invisible at start

        if (basketSprite != null)
        {
            originalBasketColor = basketSprite.color; // ✅ Store original color
        }

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
            yield return new WaitForSeconds(5f); // Allow time for the player to read.

            HideQuestion();
            isTimerRunning = true;
            yield return new WaitForSeconds(1f); // Brief delay before spawning flags.

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
                ShowFeedback(true); // ✅ Show "Correct!"
                if (correctSound) correctSound.Play();
            }
            else
            {
                incorrectCount++;
                ShowFeedback(false); // ✅ Show "Wrong!"
                if (wrongSound) wrongSound.Play();
            }

            roundCompleted = true;
            isTimerRunning = false;
            DestroyAllFlags();
        }
    }

    private void ShowFeedback(bool isCorrect)
    {
        feedbackText.gameObject.SetActive(true);
        feedbackCanvasGroup.alpha = 1; // Make it fully visible

        if (isCorrect)
        {
            feedbackText.text = "Correct!";
            feedbackText.color = Color.green;
            if (basketSprite != null) StartCoroutine(FlashBasketColor(correctColor));
        }
        else
        {
            feedbackText.text = "Wrong!";
            feedbackText.color = Color.red;
            if (basketSprite != null) StartCoroutine(FlashBasketColor(wrongColor));
        }

        // Start the fade-out animation
        StartCoroutine(FadeOutFeedback());
    }

    private IEnumerator FadeOutFeedback()
    {
        float duration = 1f; // Fade duration (1 second)
        float startAlpha = feedbackCanvasGroup.alpha;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            feedbackCanvasGroup.alpha = Mathf.Lerp(startAlpha, 0, elapsed / duration);
            yield return null;
        }

        feedbackCanvasGroup.alpha = 0; // Ensure it's fully invisible
        feedbackText.gameObject.SetActive(false);
    }

    private IEnumerator FlashBasketColor(Color flashColor)
    {
        basketSprite.color = flashColor;
        yield return new WaitForSeconds(0.5f);
        basketSprite.color = originalBasketColor;
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

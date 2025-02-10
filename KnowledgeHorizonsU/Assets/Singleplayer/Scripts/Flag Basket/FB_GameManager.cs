using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic; // For using List

public class FB_GameManager : MonoBehaviour
{
    [Header("Game Timer")]
    public float gameTime = 60f;  // Total game time
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
    public float spawnRangeX = 8f;

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

    void Start()
    {
        currentTime = gameTime;
        questionPanel.SetActive(false);
        endGamePanel.SetActive(false);
        StartCoroutine(GameLoop());
    }

    void Update()
    {
        if (isTimerRunning && currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            timerText.text = "Time: " + Mathf.Ceil(currentTime).ToString();
        }
    }

    IEnumerator GameLoop()
    {
        while (currentTime > 0 && currentQuestionIndex < questions.Length)
        {
            ShowQuestion(questions[currentQuestionIndex]);

            // Wait for 5 seconds while the question is visible (player is reading)
            yield return new WaitForSeconds(5f);

            // Hide the question panel after reading time
            HideQuestion();

            // Start the timer AFTER question disappears
            isTimerRunning = true;

            // Wait 1 second before spawning flags (to prevent overlapping)
            yield return new WaitForSeconds(1f);

            // Now spawn the flags
            SpawnFlagsForQuestion(questions[currentQuestionIndex].correctCountry);

            // Wait until player catches a flag
            yield return new WaitUntil(() => isTimerRunning == false);

            currentQuestionIndex++;
        }

        EndGame();
    }

    void ShowQuestion(Question question)
    {
        isTimerRunning = false; // Pause the timer
        questionPanel.SetActive(true);
        questionText.text = question.questionText;
    }

    void HideQuestion()
    {
        questionPanel.SetActive(false);
    }

    // Spawn flags for a given question
    void SpawnFlagsForQuestion(string correctCountry)
    {
        List<string> selectedFlags = new List<string>(); // List to store selected flag names

        selectedFlags.Add(correctCountry); // Add correct flag first

        // Get 3 random incorrect flags
        while (selectedFlags.Count < 4)
        {
            string randomCountry = GetRandomCountryExcluding(selectedFlags);
            selectedFlags.Add(randomCountry);
        }

        // Shuffle flags for randomness
        selectedFlags = ShuffleList(selectedFlags);

        for (int i = 0; i < 4; i++)
        {
            Vector3 spawnPos = new Vector3(
                Random.Range(-spawnRangeX, spawnRangeX),
                flagSpawnPoint.position.y,
                0f
            );

            GameObject flagObj = Instantiate(flagPrefab, spawnPos, Quaternion.identity);
            FB_Flag flagScript = flagObj.GetComponent<FB_Flag>();

            flagScript.FlagCountry = selectedFlags[i]; // Assign different country name to each flag

            // Load sprite dynamically
            Sprite flagSprite = Resources.Load<Sprite>("Flags/" + selectedFlags[i]);
            if (flagSprite != null)
                flagObj.GetComponent<SpriteRenderer>().sprite = flagSprite;
            else
                Debug.LogWarning("Sprite for " + selectedFlags[i] + " not found in Resources/Flags/");
        }
    }

    // Helper function to get a random country excluding the ones in the list
    string GetRandomCountryExcluding(List<string> excludeCountries)
    {
        string[] allCountries = new string[] { "USA", "Canada", "Mexico", "France", "Germany", "Brazil", "India", "China", "Japan", "Australia", "Russia", "Italy" }; // Add all available country names here

        string randomCountry;
        do
        {
            randomCountry = allCountries[Random.Range(0, allCountries.Length)];
        } while (excludeCountries.Contains(randomCountry)); // Ensure uniqueness

        return randomCountry;
    }

    // Shuffle a list of items randomly
    List<T> ShuffleList<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int randomIndex = Random.Range(i, list.Count);
            (list[i], list[randomIndex]) = (list[randomIndex], list[i]); // Swap elements
        }
        return list;
    }

    public void RegisterFlagCaught(string flagCountry)
    {
        if (currentQuestionIndex < questions.Length)
        {
            if (flagCountry == questions[currentQuestionIndex].correctCountry)
            {
                correctCount++;
                Debug.Log("Correct!");
            }
            else
            {
                incorrectCount++;
                Debug.Log("Incorrect!");
            }

            isTimerRunning = false; // Pause the timer when a flag is caught
            HideQuestion(); // Hide the question after flag is caught
        }
    }

    void EndGame()
    {
        isTimerRunning = false;
        endGamePanel.SetActive(true);
        scoreText.text = "Correct: " + correctCount + "\nIncorrect: " + incorrectCount;
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
}

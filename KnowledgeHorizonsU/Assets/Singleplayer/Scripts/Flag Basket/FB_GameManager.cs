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
    public float spawnSpacing = 10.0f;

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
    private int missedCount = 0; // Tracks number of missed flags

    // ✅ Country name to ISO code mapping
    private Dictionary<string, string> countryCodeMap = new Dictionary<string, string>
    {
        { "India", "IN" }, { "Afghanistan", "AF" }, { "Argentina", "AR" },
        { "Brazil", "BR" }, { "Canada", "CA" }, { "China", "CN" },
        { "France", "FR" }, { "Germany", "DE" }, { "Italy", "IT" },
        { "Japan", "JP" }, { "Mexico", "MX" }, { "Russia", "RU" },
        { "United States", "US" }, { "United Kingdom", "GB" }, { "Australia", "AU" }
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
            // Show the question panel
            ShowQuestion(questions[currentQuestionIndex]);

            yield return new WaitForSeconds(5f); // Wait for player to read the question

            // Hide the question panel
            HideQuestion();
            isTimerRunning = true; // Start the timer

            yield return new WaitForSeconds(1f); // Small delay before spawning flags

            // Spawn flags for this round
            SpawnFlagsForQuestion(questions[currentQuestionIndex].correctCountry);

            // Wait for the player to interact with the flags
            yield return new WaitUntil(() => isTimerRunning == false); // Wait until the flag is caught or missed

            // Proceed to the next question after the flags have been caught or missed
            currentQuestionIndex++;
        }

        EndGame();
    }

    void ShowQuestion(Question question)
    {
        isTimerRunning = false; // Pause the timer when showing the question
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

        // Calculate total width available for the flags to spread across (e.g., width of the screen or spawn area)
        float totalWidth = spawnSpacing * 3f; // Total width for the flags, based on the spacing

        // Calculate the starting x position (half the total width, so flags are centered on the screen)
        float startX = -totalWidth / 2f;

        // Spawn the flags evenly distributed
        for (int i = 0; i < 4; i++)
        {
            // Evenly distribute flags based on total width and spawn spacing
            float spawnPosX = startX + (i * spawnSpacing); // Adjust x position for each flag

            Vector3 spawnPos = new Vector3(spawnPosX, flagSpawnPoint.position.y, 0f); // Set flag position

            GameObject flagObj = Instantiate(flagPrefab, spawnPos, Quaternion.identity);
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




    // ✅ Get a random country excluding already selected countries
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

    // ✅ Shuffle the list of selected flags
    List<T> ShuffleList<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int randomIndex = Random.Range(i, list.Count);
            (list[i], list[randomIndex]) = (list[randomIndex], list[i]);
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

            isTimerRunning = false;
            HideQuestion(); // Hide the question panel to move to the next question
        }
    }

    // ✅ Handle a missed flag
    public void FlagMissed()
    {
        Debug.Log("Flag missed! Moving to next question.");
        missedCount++; // Increase missed flag count
        isTimerRunning = false; // Stop the timer when a flag is missed
        HideQuestion(); // Hide the question panel to prepare for the next question
        StartCoroutine(ProceedToNextQuestion()); // Proceed to the next question after the delay
    }

    // ✅ Method to handle the transition to the next question
    IEnumerator ProceedToNextQuestion()
    {
        yield return new WaitForSeconds(1f); // Small delay to simulate "waiting" for the player to see the missed flag

        // Continue to the next question by re-triggering the GameLoop coroutine
        if (currentQuestionIndex < questions.Length)
        {
            // Show next question after the miss and move on
            ShowQuestion(questions[currentQuestionIndex]);

            yield return new WaitForSeconds(5f); // Wait for the player to read the next question
            HideQuestion();

            // Reset timer and flags for next round
            isTimerRunning = true;
            yield return new WaitForSeconds(1f);

            // Spawn flags for the next round
            SpawnFlagsForQuestion(questions[currentQuestionIndex].correctCountry);

            // Wait until the next round completes
            yield return new WaitUntil(() => isTimerRunning == false);
        }
    }

    void EndGame()
    {
        isTimerRunning = false;
        endGamePanel.SetActive(true);
        scoreText.text = "Correct: " + correctCount + "\nIncorrect: " + incorrectCount + "\nFlags Dropped: " + missedCount; // Show missed flags
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

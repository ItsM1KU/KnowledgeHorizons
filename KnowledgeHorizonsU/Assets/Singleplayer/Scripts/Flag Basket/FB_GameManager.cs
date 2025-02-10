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
    public float spawnSpacing = 5.0f;

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

    private Dictionary<string, string> countryCodeMap = new Dictionary<string, string>
    {
        { "India", "IN" }, { "Afghanistan", "AF" }, { "Argentina", "AR" },
        { "Brazil", "BR" }, { "Canada", "CA" }, { "China", "CN" },
        { "France", "FR" }, { "Germany", "DE" }, { "Italy", "IT" },
        { "Japan", "JP" }, { "Mexico", "MX" }, { "Russia", "RU" },
        { "United States", "US" }, { "United Kingdom", "GB" }, { "Australia", "AU" },
        { "Bangladesh", "BD" }, {"Korea", "KR" }, {"Turkey", "TR"}
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
            ShowQuestion(questions[currentQuestionIndex]);

            yield return new WaitForSeconds(5f);

            HideQuestion();
            isTimerRunning = true;

            yield return new WaitForSeconds(1f);

            SpawnFlagsForQuestion(questions[currentQuestionIndex].correctCountry);

            yield return new WaitUntil(() => isTimerRunning == false);

            currentQuestionIndex++;
        }

        EndGame();
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

        float startX = -spawnSpacing * 1.5f;
        for (int i = 0; i < 4; i++)
        {
            Vector3 spawnPos = new Vector3(startX + (i * spawnSpacing), flagSpawnPoint.position.y, 0f);

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
            HideQuestion();
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

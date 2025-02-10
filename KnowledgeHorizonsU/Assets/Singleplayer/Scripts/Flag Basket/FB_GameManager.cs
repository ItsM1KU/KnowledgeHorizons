using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using static Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics;

public class FB_GameManager : MonoBehaviour
{
    [Header("Game Timer")]
    public float gameTime = 60f;
    private float currentTime;

    [Header("UI Elements")]
    public Text timerText;
    public GameObject questionPanel;
    public Text questionText;
    public GameObject endGamePanel;
    public Text scoreText;

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

    [Header("Questions Setup")] // Place it here instead
    public Question[] questions;

    // Score tracking
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
        if (currentTime > 0)
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
            yield return new WaitForSeconds(10f);
            HideQuestion();

            SpawnFlagsForQuestion(questions[currentQuestionIndex].correctCountry);
            yield return new WaitForSeconds(5f);

            currentQuestionIndex++;
        }

        EndGame();
    }

    void ShowQuestion(Question question)
    {
        questionPanel.SetActive(true);
        questionText.text = question.questionText;
    }

    void HideQuestion()
    {
        questionPanel.SetActive(false);
    }

    void SpawnFlagsForQuestion(string correctCountry)
    {
        int correctFlagIndex = Random.Range(0, 4);

        for (int i = 0; i < 4; i++)
        {
            Vector3 spawnPos = new Vector3(
                Random.Range(-spawnRangeX, spawnRangeX),
                flagSpawnPoint.position.y,
                0f
            );

            GameObject flagObj = Instantiate(flagPrefab, spawnPos, Quaternion.identity);
            FB_Flag flagScript = flagObj.GetComponent<FB_Flag>();

            if (i == correctFlagIndex)
            {
                flagScript.FlagCountry = correctCountry;
                Sprite correctSprite = Resources.Load<Sprite>("Flags/" + correctCountry);
                if (correctSprite != null)
                    flagObj.GetComponent<SpriteRenderer>().sprite = correctSprite;
                else
                    Debug.LogWarning("Sprite for " + correctCountry + " not found in Resources/Flags/");
            }
            else
            {
                string randomCountry = GetRandomCountryExcluding(correctCountry);
                flagScript.FlagCountry = randomCountry;
                Sprite randomSprite = Resources.Load<Sprite>("Flags/" + randomCountry);
                if (randomSprite != null)
                    flagObj.GetComponent<SpriteRenderer>().sprite = randomSprite;
                else
                    Debug.LogWarning("Sprite for " + randomCountry + " not found in Resources/Flags/");
            }
        }
    }

    string GetRandomCountryExcluding(string excludeCountry)
    {
        string[] allCountries = new string[] { "USA", "Canada", "Mexico", "France", "Germany", "Brazil" };
        string randomCountry = excludeCountry;

        while (randomCountry == excludeCountry)
        {
            randomCountry = allCountries[Random.Range(0, allCountries.Length)];
        }
        return randomCountry;
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
        }
    }

    void EndGame()
    {
        endGamePanel.SetActive(true);
        scoreText.text = "Correct: " + correctCount + "\nIncorrect: " + incorrectCount;
        Time.timeScale = 0f;
    }
}

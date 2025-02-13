using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Linq;

public class FB_GameManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject questionPanel;
    public TMP_Text questionText;
    public TMP_Text remainingQuestionsText; // ✅ Display remaining questions
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

    [System.Serializable]
    public class Question
    {
        public string questionText;
        public string correctCountry;
    }

    [Header("Questions Setup")]
    public List<Question> questions;

    private List<Question> shuffledQuestions;
    private int currentQuestionIndex = 0;
    private int correctCount = 0;
    private int incorrectCount = 0;
    private bool gameOver = false;
    private bool roundCompleted = false;
    private const int totalQuestions = 20; // ✅ Game ends after 20 questions

    void Start()
    {
        questionPanel.SetActive(false);
        endGamePanel.SetActive(false);

        feedbackCanvasGroup = feedbackText.GetComponent<CanvasGroup>();
        if (feedbackCanvasGroup == null)
        {
            feedbackCanvasGroup = feedbackText.gameObject.AddComponent<CanvasGroup>();
        }
        feedbackCanvasGroup.alpha = 0;

        if (basketSprite != null)
        {
            originalBasketColor = basketSprite.color;
        }

        shuffledQuestions = new List<Question>(questions);
        shuffledQuestions = ShuffleList(shuffledQuestions);

        StartCoroutine(GameLoop());
    }

    IEnumerator GameLoop()
    {
        while (currentQuestionIndex < totalQuestions && !gameOver)
        {
            roundCompleted = false;
            Question currentQuestion = shuffledQuestions[currentQuestionIndex];

            ShowQuestion(currentQuestion);
            yield return new WaitForSeconds(5f);

            HideQuestion();
            yield return new WaitForSeconds(1f);

            SpawnFlagsForQuestion(currentQuestion.correctCountry);
            yield return new WaitUntil(() => roundCompleted || gameOver);

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
        questionPanel.SetActive(true);
        questionText.text = question.questionText;

        // ✅ Hide "Questions Left" when the last question is answered
        int questionsLeft = totalQuestions - currentQuestionIndex;
        if (questionsLeft > 0)
        {
            remainingQuestionsText.text = "Questions Left: " + questionsLeft;
        }
        else
        {
            remainingQuestionsText.gameObject.SetActive(false);
        }
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

            flagScript.SetFlag(selectedFlags[i], countryCodeMap[selectedFlags[i]]);
        }
    }

    public void RegisterFlagCaught(string flagCountry)
    {
        if (!gameOver && currentQuestionIndex < totalQuestions)
        {
            Question currentQuestion = shuffledQuestions[currentQuestionIndex];

            if (flagCountry == currentQuestion.correctCountry)
            {
                correctCount++;
                ShowFeedback(true);
                if (correctSound) correctSound.Play();
            }
            else
            {
                incorrectCount++;
                ShowFeedback(false);
                if (wrongSound) wrongSound.Play();
            }

            roundCompleted = true;
            DestroyAllFlags();
        }
    }

    private void ShowFeedback(bool isCorrect)
    {
        feedbackText.gameObject.SetActive(true);
        feedbackCanvasGroup.alpha = 1;

        feedbackText.text = isCorrect ? "Correct!" : "Wrong!";
        feedbackText.color = isCorrect ? Color.green : Color.red;

        if (basketSprite != null)
        {
            StartCoroutine(FlashBasketColor(isCorrect ? correctColor : wrongColor));
        }

        StartCoroutine(FadeOutFeedback());
    }

    private IEnumerator FadeOutFeedback()
    {
        float duration = 1f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            feedbackCanvasGroup.alpha = Mathf.Lerp(1, 0, elapsed / duration);
            yield return null;
        }

        feedbackCanvasGroup.alpha = 0;
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
            StopAllCoroutines(); // ✅ Stop the game loop
            EndGame();
        }
    }

    void EndGame()
    {
        gameOver = true;
        DestroyAllFlags();
        endGamePanel.SetActive(true);
        remainingQuestionsText.gameObject.SetActive(false); // ✅ Hide Questions Left Text
        scoreText.text = "Game Over!\nCorrect: " + correctCount + "\nIncorrect: " + incorrectCount;
    }


    public void RestartGame() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    public void GoToMainMenu() => SceneManager.LoadScene("Islands");

    string GetRandomCountryExcluding(List<string> excludeCountries) =>
        ShuffleList(new List<string>(countryCodeMap.Keys)).Find(c => !excludeCountries.Contains(c));

    List<T> ShuffleList<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
        return list;
    }

    void DestroyAllFlags() => GameObject.FindGameObjectsWithTag("Flag").ToList().ForEach(Destroy);
}

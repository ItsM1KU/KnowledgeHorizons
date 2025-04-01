using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class Question
{
    public string question;
    public string[] answers;
    public int correctAnswer;
}

public class GameManagerrr : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject continentSelectionPanel;
    public GameObject quizPanel;
    public GameObject finalScorePanel;

    [Header("Continent Selection UI")]
    public TextMeshProUGUI continentQuestionText;
    public TextMeshProUGUI asiaText;
    public TextMeshProUGUI europeText;
    public TextMeshProUGUI americasText;
    public GameObject asiaButton;
    public GameObject europeButton;
    public GameObject americasButton;

    [Header("Question UI")]
    public TextMeshProUGUI questionText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI finalScoreText;
    
    public Image[] answerButtons; // Image GameObjects acting as buttons

    [Header("Quiz Data")]
    public List<Question> asiaQuestions;
    public List<Question> europeQuestions;
    public List<Question> americasQuestions;

    private List<Question> currentQuestions;
    private int currentQuestionIndex;
    private int score;
    private float timer;
    private bool isGameActive = false;
    
    private int scoreFactor; // Unique score adder per continent
    private float timePerQuestion; // Unique time per question based on continent

    private void Start()
    {
        quizPanel.SetActive(false);
        finalScorePanel.SetActive(false);
        continentSelectionPanel.SetActive(true);

        // Set up the continent selection screen
        continentQuestionText.text = "Which continent do you want to choose?";
        asiaText.text = "Asia";
        europeText.text = "Europe";
        americasText.text = "Americas";

        scoreText.text = "Score: 0"; // ✅ Ensure initial score is displayed
    }

    public void SelectContinent(string continent)
    {
        continentSelectionPanel.SetActive(false);
        quizPanel.SetActive(true);

        if (continent == "Asia")
        {
            currentQuestions = new List<Question>(asiaQuestions);
            timePerQuestion = 20f;
            scoreFactor = 10;
        }
        else if (continent == "Europe")
        {
            currentQuestions = new List<Question>(europeQuestions);
            timePerQuestion = 25f;
            scoreFactor = 15;
        }
        else if (continent == "Americas")
        {
            currentQuestions = new List<Question>(americasQuestions);
            timePerQuestion = 30f;
            scoreFactor = 20;
        }

        currentQuestionIndex = 0;
        score = 0;
        scoreText.text = "Score: 0"; // ✅ Ensure score resets when starting a new game
        isGameActive = true;
        timer = timePerQuestion; // ✅ Reset timer based on continent
        ShowNextQuestion();
    }

    void ShowNextQuestion()
    {
        if (currentQuestionIndex >= currentQuestions.Count)
        {
            EndGame();
            return;
        }

        Question q = currentQuestions[currentQuestionIndex];
        questionText.text = q.question;

        for (int i = 0; i < answerButtons.Length; i++)
        {
            TextMeshProUGUI answerText = answerButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            if (answerText != null) 
            {
                answerText.text = q.answers[i];
            }
        }

        timer = timePerQuestion; // ✅ Reset timer per question
        timerText.text = "Time: " + timer.ToString("F1");
    }

    public void SelectAnswer(int index)
    {
        if (!isGameActive) return;

        Question q = currentQuestions[currentQuestionIndex];

        if (index == q.correctAnswer)
        {
            score += scoreFactor; // ✅ Add points based on continent
            scoreText.text = "Score: " + score;
        }

        currentQuestionIndex++;
        ShowNextQuestion();
    }

    private void Update()
    {
        if (!isGameActive) return;

        timer -= Time.deltaTime;
        timerText.text = "Time: " + timer.ToString("F1");

        if (timer <= 0)
        {
            EndGame();
        }
    }

    void EndGame()
    {
        isGameActive = false;
        quizPanel.SetActive(false);
        finalScorePanel.SetActive(true);
        finalScoreText.text = "Final Score: " + score; // ✅ Ensure final score is displayed

        // ✅ Hide continent selection elements
        continentQuestionText.gameObject.SetActive(false);
        asiaButton.SetActive(false);
        europeButton.SetActive(false);
        americasButton.SetActive(false);
    }

    public void RestartGame()
    {
        isGameActive = false;
        currentQuestions = null;
        currentQuestionIndex = 0;
        score = 0;
        timer = 0f;

        quizPanel.SetActive(false);
        finalScorePanel.SetActive(false);
        continentSelectionPanel.SetActive(true);

        questionText.text = "";
        timerText.text = "Time: 0";
        scoreText.text = "Score: 0"; // ✅ Reset score UI properly
        finalScoreText.text = "";

        foreach (Image button in answerButtons)
        {
            TextMeshProUGUI answerText = button.GetComponentInChildren<TextMeshProUGUI>();
            if (answerText != null)
            {
                answerText.text = "";
            }
        }

        // ✅ Show continent selection elements again
        continentQuestionText.gameObject.SetActive(true);
        asiaButton.SetActive(true);
        europeButton.SetActive(true);
        americasButton.SetActive(true);

        continentQuestionText.text = "Which continent do you want to choose?";
        asiaText.text = "Asia";
        europeText.text = "Europe";
        americasText.text = "Americas";
    }
}

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
    }

    public void SelectContinent(string continent)
    {
        continentSelectionPanel.SetActive(false);
        quizPanel.SetActive(true);

        if (continent == "Asia")
        {
            currentQuestions = new List<Question>(asiaQuestions);
            timer = 20f;
        }
        else if (continent == "Europe")
        {
            currentQuestions = new List<Question>(europeQuestions);
            timer = 25f;
        }
        else if (continent == "Americas")
        {
            currentQuestions = new List<Question>(americasQuestions);
            timer = 30f;
        }

        currentQuestionIndex = 0;
        score = 0;
        isGameActive = true;
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

        timerText.text = "Time: " + timer.ToString("F1");
    }

    public void SelectAnswer(int index)
    {
        if (!isGameActive) return;

        Question q = currentQuestions[currentQuestionIndex];

        if (index == q.correctAnswer)
        {
            score += 10;
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
        finalScoreText.text = "Final Score: " + score;
    }

    public void RestartGame()
    {
        finalScorePanel.SetActive(false);
        quizPanel.SetActive(false);
        continentSelectionPanel.SetActive(true);

        currentQuestionIndex = 0;
        score = 0;
        timer = 0f;
        isGameActive = false;
        
        currentQuestions = null;
        questionText.text = "";
        timerText.text = "Time: 0";
        scoreText.text = "Score: 0";

        // Reset continent selection text
        continentQuestionText.text = "Which continent do you want to choose?";
        asiaText.text = "Asia";
        europeText.text = "Europe";
        americasText.text = "Americas";
    }
}
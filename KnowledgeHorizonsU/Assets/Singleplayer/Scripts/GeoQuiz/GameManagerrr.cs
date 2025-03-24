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
    public int correctAnswer; // Correct answer index before shuffling
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
    public GameObject resetButton;

    [Header("Question UI")]
    public TextMeshProUGUI questionText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI finalScoreText;

    public Image[] answerButtons; // Assign four Image buttons in the inspector

    [Header("Quiz Data")]
    private List<Question> asiaQuestions = new List<Question>();
    private List<Question> europeQuestions = new List<Question>();
    private List<Question> americasQuestions = new List<Question>();

    private List<Question> currentQuestions;
    private int currentQuestionIndex;
    private int score;
    private float timer;
    private bool isGameActive = false;

    private int scoreFactor;
    private float timePerQuestion;

    private void Start()
    {
        InitializeQuestions();
        quizPanel.SetActive(false);
        finalScorePanel.SetActive(false);
        continentSelectionPanel.SetActive(true);

        asiaText.text = "Asia";
        europeText.text = "Europe";
        americasText.text = "Americas";

        resetButton.SetActive(false);
        finalScoreText.gameObject.SetActive(false);

        scoreText.text = "Score: 0";
    }

    void InitializeQuestions()
    {
        // Asia Questions
        asiaQuestions = new List<Question>
        {
            new Question { question = "What is the capital of Japan?", answers = new string[] {"Tokyo", "Beijing", "Seoul", "Bangkok"}, correctAnswer = 0 },
            new Question { question = "Which desert is found in China?", answers = new string[] {"Gobi", "Sahara", "Mojave", "Kalahari"}, correctAnswer = 0 },
            new Question { question = "What is the longest river in Asia?", answers = new string[] {"Yangtze", "Ganges", "Mekong", "Indus"}, correctAnswer = 0 },
            new Question { question = "Which mountain range separates India and China?", answers = new string[] {"Himalayas", "Andes", "Rockies", "Alps"}, correctAnswer = 0 },
            new Question { question = "What is the most populous country in Asia?", answers = new string[] {"China", "India", "Japan", "Indonesia"}, correctAnswer = 0 },
            new Question { question = "Which Asian country has the highest number of islands?", answers = new string[] {"Indonesia", "Philippines", "Japan", "Malaysia"}, correctAnswer = 0 },
            new Question { question = "Which Asian city is known as the 'Pearl of the Orient'?", answers = new string[] {"Hong Kong", "Bangkok", "Tokyo", "Manila"}, correctAnswer = 0 },
            new Question { question = "What is the currency of South Korea?", answers = new string[] {"Won", "Yen", "Baht", "Rupee"}, correctAnswer = 0 }
        };

        // Europe Questions
        europeQuestions = new List<Question>
        {
            new Question { question = "What is the capital of France?", answers = new string[] {"Paris", "Berlin", "Madrid", "Rome"}, correctAnswer = 0 },
            new Question { question = "Which river runs through London?", answers = new string[] {"Thames", "Seine", "Danube", "Rhine"}, correctAnswer = 0 },
            new Question { question = "Which European country has the most islands?", answers = new string[] {"Sweden", "Norway", "Greece", "Italy"}, correctAnswer = 0 },
            new Question { question = "Which is the smallest country in Europe?", answers = new string[] {"Vatican City", "Monaco", "San Marino", "Liechtenstein"}, correctAnswer = 0 },
            new Question { question = "Which European city is known as 'The Eternal City'?", answers = new string[] {"Rome", "Athens", "London", "Paris"}, correctAnswer = 0 },
            new Question { question = "What is the highest mountain in Europe?", answers = new string[] {"Mount Elbrus", "Mont Blanc", "Matterhorn", "Ben Nevis"}, correctAnswer = 0 },
            new Question { question = "Which sea borders Sweden and Finland?", answers = new string[] {"Baltic Sea", "North Sea", "Mediterranean Sea", "Black Sea"}, correctAnswer = 0 },
            new Question { question = "What currency is used in Switzerland?", answers = new string[] {"Swiss Franc", "Euro", "Pound", "Krone"}, correctAnswer = 0 }
        };

        // Americas Questions
        americasQuestions = new List<Question>
        {
            new Question { question = "What is the capital of Canada?", answers = new string[] {"Ottawa", "Toronto", "Montreal", "Vancouver"}, correctAnswer = 0 },
            new Question { question = "Which US state is the Grand Canyon located in?", answers = new string[] {"Arizona", "Nevada", "Utah", "California"}, correctAnswer = 0 },
            new Question { question = "What is the largest country in South America?", answers = new string[] {"Brazil", "Argentina", "Colombia", "Chile"}, correctAnswer = 0 },
            new Question { question = "Which US state is closest to Russia?", answers = new string[] {"Alaska", "Hawaii", "Washington", "Oregon"}, correctAnswer = 0 },
            new Question { question = "What is the longest river in the Americas?", answers = new string[] {"Amazon", "Mississippi", "Colorado", "Orinoco"}, correctAnswer = 0 },
            new Question { question = "Which Caribbean country is the largest by land area?", answers = new string[] {"Cuba", "Haiti", "Jamaica", "Dominican Republic"}, correctAnswer = 0 },
            new Question { question = "Which US state is known as the 'Sunshine State'?", answers = new string[] {"Florida", "California", "Texas", "Hawaii"}, correctAnswer = 0 },
            new Question { question = "Which mountain range runs along the western coast of South America?", answers = new string[] {"Andes", "Rockies", "Appalachians", "Sierra Madre"}, correctAnswer = 0 }
        };
    }

    void ShuffleAnswers(Question q)
    {
        List<string> shuffledAnswers = new List<string>(q.answers);
        int correctIndex = q.correctAnswer;

        for (int i = shuffledAnswers.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (shuffledAnswers[i], shuffledAnswers[j]) = (shuffledAnswers[j], shuffledAnswers[i]);

            if (correctIndex == i)
                correctIndex = j;
            else if (correctIndex == j)
                correctIndex = i;
        }

        q.answers = shuffledAnswers.ToArray();
        q.correctAnswer = correctIndex;
    }
}

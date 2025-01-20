using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Text sequenceText;
    public Text feedbackText;
    public Button[] balloons;
    public Text timerText; // Reference to display the timer
    public Text scoreText; // Reference to display the score
    public Text gameOverText; // Text to display "Game Over"
    public GameObject pauseMenu; // Reference to the pause menu UI

    private float totalTime = 30f; // Total time for the game
    private float remainingTime; // Time left for the game
    private int score = 0; // Player's score
    private int questionCount = 0; // Number of questions asked
    private int maxQuestions = 5; // Total questions per game
    private bool isGameRunning = true; // To control the game state

    private List<int> currentSequence = new List<int>();
    private int correctAnswer;
    private HashSet<string> usedQuestions = new HashSet<string>(); // To track unique questions

    private void Start()
    {
        InitializeGame();
    }

    private void InitializeGame()
    {
        // Initialize game values
        score = 0;
        questionCount = 0;
        remainingTime = totalTime;
        isGameRunning = true;
        gameOverText.text = ""; // Clear game over text
        usedQuestions.Clear(); // Clear previously used questions

        GenerateNewSequence();
        AssignBalloonNumbers();

        feedbackText.text = ""; // Clear feedback on start
        scoreText.text = "Score: " + score; // Initialize score text
        timerText.text = "Time Left: " + Mathf.Ceil(remainingTime).ToString(); // Initialize timer text
    }

    private void GenerateNewSequence()
    {
        currentSequence.Clear(); // Clear current sequence

        do
        {
            // Randomly choose a sequence type
            int sequenceType = Random.Range(0, 4); // 0 = Odd, 1 = Even, 2 = Fibonacci, 3 = Squares
            int start = Random.Range(1, 10); // Random starting point

            switch (sequenceType)
            {
                case 0: // Odd numbers
                    for (int i = 0; i < 3; i++) currentSequence.Add(start + i * 2);
                    correctAnswer = currentSequence[currentSequence.Count - 1] + 2;
                    break;

                case 1: // Even numbers
                    start = (start % 2 == 0) ? start : start + 1; // Ensure start is even
                    for (int i = 0; i < 3; i++) currentSequence.Add(start + i * 2);
                    correctAnswer = currentSequence[currentSequence.Count - 1] + 2;
                    break;

                case 2: // Fibonacci sequence
                    int a = 1, b = 1;
                    currentSequence.Add(a);
                    currentSequence.Add(b);
                    for (int i = 2; i < 3; i++)
                    {
                        int next = a + b;
                        currentSequence.Add(next);
                        a = b;
                        b = next;
                    }
                    correctAnswer = a + b;
                    break;

                case 3: // Squares
                    for (int i = 0; i < 3; i++) currentSequence.Add((start + i) * (start + i));
                    correctAnswer = (start + 3) * (start + 3);
                    break;
            }
        } while (usedQuestions.Contains(SequenceKey())); // Ensure the question is unique

        usedQuestions.Add(SequenceKey()); // Mark the sequence as used
        UpdateSequenceText();
    }

    private string SequenceKey()
    {
        return string.Join(",", currentSequence) + "|" + correctAnswer; // Unique key for the sequence
    }

    private void UpdateSequenceText()
    {
        string sequenceDisplay = "";
        for (int i = 0; i < currentSequence.Count; i++)
        {
            sequenceDisplay += currentSequence[i] + ", ";
        }
        sequenceDisplay += "?"; // Add the missing number
        sequenceText.text = "Sequence: " + sequenceDisplay;
    }

    private void AssignBalloonNumbers()
    {
        List<int> options = new List<int> { correctAnswer };

        // Add 3 incorrect options
        while (options.Count < 4)
        {
            int wrongAnswer = correctAnswer + Random.Range(-5, 6);
            if (!options.Contains(wrongAnswer)) options.Add(wrongAnswer);
        }

        // Shuffle the options
        for (int i = 0; i < options.Count; i++)
        {
            int randomIndex = Random.Range(0, options.Count);
            int temp = options[i];
            options[i] = options[randomIndex];
            options[randomIndex] = temp;
        }

        // Assign options to balloons
        for (int i = 0; i < balloons.Length; i++)
        {
            int index = i; // Prevent closure issue
            balloons[i].GetComponentInChildren<Text>().text = options[i].ToString();
            balloons[i].onClick.RemoveAllListeners();
            balloons[i].onClick.AddListener(() => OnBalloonClicked(options[index]));
        }
    }

    private void OnBalloonClicked(int selectedNumber)
    {
        if (!isGameRunning) return;

        if (selectedNumber == correctAnswer)
        {
            feedbackText.text = "Correct!";
            // Score based on remaining time
            score += Mathf.CeilToInt(remainingTime);
            scoreText.text = "Score: " + score; // Update the score text
        }
        else
        {
            feedbackText.text = "Wrong!";
        }

        questionCount++;

        if (questionCount >= maxQuestions || remainingTime <= 0)
        {
            EndGame();
        }
        else
        {
            GenerateNewSequence();
            AssignBalloonNumbers();
        }
    }

    private void Update()
    {
        if (!isGameRunning) return;

        // Decrease the total game timer
        remainingTime -= Time.deltaTime;
        timerText.text = "Time Left: " + Mathf.Ceil(remainingTime).ToString(); // Update the timer text

        if (remainingTime <= 0)
        {
            EndGame();
        }

        if (PauseManager.instance.isPaused)
        {
            for(int i = 0; i < balloons.Length; i++)
            {
                balloons[i].enabled = false;
            }
        }
        else
        {
            for (int i = 0; i < balloons.Length; i++)
            {
                balloons[i].enabled = true;
            }
        }
        
    }

    private void EndGame()
    {
        isGameRunning = false;

        // Display final score and game over message
        gameOverText.text = $"Game Over!\nFinal Score: {score}";
        gameOverText.gameObject.SetActive(true); // Ensure the text is visible

        // Disable other UI elements to avoid confusion
        sequenceText.gameObject.SetActive(false);
        feedbackText.gameObject.SetActive(false);
        timerText.gameObject.SetActive(false);
        scoreText.gameObject.SetActive(false);

        // Stop player interactions
        foreach (Button balloon in balloons)
        {
            balloon.interactable = false;
        }

        // Activate pause menu
        pauseMenu.SetActive(true); // Show the pause menu
        Time.timeScale = 0f; // Pause the game
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; // Ensure the game is not paused
        InitializeGame(); // Restart the game logic
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f; // Resume game
        pauseMenu.SetActive(false); // Hide pause menu
    }

    public void GoToIslands()
    {
        Time.timeScale = 1f; // Resume time if paused
        SceneManager.LoadScene("IslandsScene"); // Replace with your actual scene name
    }
}

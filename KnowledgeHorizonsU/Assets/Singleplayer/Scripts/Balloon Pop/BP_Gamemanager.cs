using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text sequenceText;
    public Text feedbackText;
    public Button[] balloons;

    private List<int> currentSequence = new List<int>();
    private int correctAnswer;
    private int wrongAttempts = 0;

    private void Start()
    {
        GenerateNewSequence();
        AssignBalloonNumbers();
        feedbackText.text = ""; // Clear feedback on start
    }

    private void GenerateNewSequence()
    {
        // Clear current sequence
        currentSequence.Clear();

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

        UpdateSequenceText();
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
        // Create a list of options
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
        if (selectedNumber == correctAnswer)
        {
            feedbackText.text = "Correct!";
            GenerateNewSequence();
            AssignBalloonNumbers();
        }
        else
        {
            feedbackText.text = "Wrong!";
            wrongAttempts++;
            if (wrongAttempts >= 4)
            {
                feedbackText.text = "Resetting Sequence...";
                wrongAttempts = 0;
                GenerateNewSequence();
                AssignBalloonNumbers();
            }
        }
    }
}

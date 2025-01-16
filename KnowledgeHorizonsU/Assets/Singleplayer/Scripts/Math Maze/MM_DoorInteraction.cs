using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MM_DoorInteraction : MonoBehaviour
{
    public Button interactButton; // Reference to the interact button
    public GameObject mathQuestionPanel; // Reference to the math question panel
    public TMP_Text questionText; // TMP_Text for displaying the question
    public TMP_InputField answerInputField; // TMP_InputField for user answer
    public Button submitButton; // Button to submit the answer

    public GameObject closedDoor; // Reference to the closed door GameObject
    public GameObject openDoor; // Reference to the open door GameObject
    public GameObject obstacle; // Optional obstacle to deactivate

    public int minRange = 1; // Minimum number range for math question
    public int maxRange = 10; // Maximum number range for math question
    public string[] operators = { "+", "-", "*", "/" }; // Operators for math questions

    private bool isPlayerNearby = false; // Check if player is near the door
    private bool isQuestionAnswered = false; // Check if the question is answered correctly

    private int correctAnswer; // Correct answer for the math question

    private void Start()
    {
        interactButton.gameObject.SetActive(false); // Hide interact button initially
        mathQuestionPanel.SetActive(false); // Hide math question panel initially
        if (openDoor != null) openDoor.SetActive(false); // Ensure the open door is initially inactive

        // Add listener to the submit button
        submitButton.onClick.AddListener(CheckAnswer);
    }

    private void Update()
    {
        // Check if the player presses 'E' near the door
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E) && !isQuestionAnswered)
        {
            DisplayMathQuestion();
        }
    }

    private void DisplayMathQuestion()
    {
        // Randomly generate numbers and an operator
        int num1 = Random.Range(minRange, maxRange + 1);
        int num2 = Random.Range(minRange, maxRange + 1);
        string selectedOperator = operators[Random.Range(0, operators.Length)];

        // Avoid division by zero
        if (selectedOperator == "/" && num2 == 0)
        {
            num2 = 1;
        }

        // Calculate the correct answer based on the selected operator
        switch (selectedOperator)
        {
            case "+":
                correctAnswer = num1 + num2;
                break;
            case "-":
                correctAnswer = num1 - num2;
                break;
            case "*":
                correctAnswer = num1 * num2;
                break;
            case "/":
                correctAnswer = num1 / num2; // Integer division
                break;
        }

        // Display the question
        questionText.text = $"What is {num1} {selectedOperator} {num2}?";
        mathQuestionPanel.SetActive(true);
    }

    private void CheckAnswer()
    {
        // Get the player's input and check if it's correct
        int playerAnswer;
        if (int.TryParse(answerInputField.text, out playerAnswer))
        {
            if (playerAnswer == correctAnswer)
            {
                isQuestionAnswered = true; // Mark the question as answered
                mathQuestionPanel.SetActive(false); // Hide the panel
                OpenDoor(); // Open the door
                Debug.Log("Correct answer! Door is now open.");
            }
            else
            {
                // Provide feedback for wrong answer
                questionText.text = "Wrong answer!\nTry again.";
                answerInputField.text = ""; // Clear the input field
                Debug.Log("Wrong answer! Door remains closed.");
            }
        }
        else
        {
            questionText.text = "Invalid input! Please enter a number.";
            Debug.Log("Invalid input received.");
        }
    }

    private void OpenDoor()
    {
        if (closedDoor != null) closedDoor.SetActive(false); // Deactivate closed door
        if (openDoor != null) openDoor.SetActive(true); // Activate open door
        if (obstacle != null) obstacle.SetActive(false); // Deactivate optional obstacle
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerNearby = true; // Player is near the door
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerNearby = false; // Player left the door area
            mathQuestionPanel.SetActive(false); // Hide the panel if active
        }
    }
}

using UnityEngine;
using TMPro; // Include TextMeshPro namespace
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
    public GameObject obstacle;

    private bool isPlayerNearby = false; // Check if player is near the door
    private bool isQuestionAnswered = false; // Check if question is answered correctly

    private int correctAnswer; // Correct answer for the math question

    private void Start()
    {
        interactButton.gameObject.SetActive(false); // Hide interact button initially
        mathQuestionPanel.SetActive(false); // Hide math question panel initially
        openDoor.SetActive(false); // Ensure the open door is initially inactive

        // Add listener to the submit button
        submitButton.onClick.AddListener(CheckAnswer);

        // Add listener to the interact button
        interactButton.onClick.AddListener(OpenDoor);
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
        // Generate a simple math question
        int num1 = Random.Range(1, 10);
        int num2 = Random.Range(1, 10);
        correctAnswer = num1 + num2;

        // Display the question and enable the panel
        questionText.text = $"What is {num1} + {num2}?";
        mathQuestionPanel.SetActive(true);
    }

    private void CheckAnswer()
    {
        // Get the player's input and check if it's correct
        int playerAnswer;
        if (int.TryParse(answerInputField.text, out playerAnswer) && playerAnswer == correctAnswer)
        {
            isQuestionAnswered = true; // Mark the question as answered
            mathQuestionPanel.SetActive(false); // Hide the panel
            interactButton.gameObject.SetActive(true); // Show the interact button

            // Open the door: deactivate closed door and activate open door
            closedDoor.SetActive(false);
            openDoor.SetActive(true);
            obstacle.SetActive(false);

            Debug.Log("Correct answer! Door is now open.");
        }
        else
        {
            // Provide feedback for wrong answer (optional)
            questionText.text = "Wrong answer!\n Press 'E' to try again!";
            answerInputField.text = ""; // Clear the input field

            Debug.Log("Wrong answer! Door remains closed.");
        }
    }

    private void OpenDoor()
    {
        // Optional: If you want to add extra actions for when the door is opened
        Debug.Log("Interacting with the open door.");
        interactButton.gameObject.SetActive(false); // Hide the interact button
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
            interactButton.gameObject.SetActive(false); // Hide the button
        }
    }
}
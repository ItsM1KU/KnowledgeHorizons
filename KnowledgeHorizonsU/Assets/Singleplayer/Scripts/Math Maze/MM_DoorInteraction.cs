using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class MM_DoorInteraction : MonoBehaviour
{
    public GameObject mathQuestionPanel; // Shared math question panel
    public TMP_Text questionText; // TMP_Text for displaying the question
    public TMP_InputField answerInputField; // TMP_InputField for user answer
    public Button submitButton; // Button to submit the answer

    public GameObject closedDoor; // Reference to the closed door GameObject
    public GameObject openDoor; // Reference to the open door GameObject
    public GameObject obstacle; // Optional obstacle to deactivate

    [System.Serializable]
    public class Question
    {
        public string questionText; // The question text
        public int answer; // The correct answer
    }

    public List<Question> questions = new List<Question>(); // List of questions for this door

    private bool isPlayerNearby = false; // Check if player is near the door
    private bool isQuestionAnswered = false; // Check if the question is answered correctly

    private int currentQuestionIndex = 0; // Tracks the current question index
    private static MM_DoorInteraction currentDoor; // Tracks the currently active door

    private void Start()
    {
        if (openDoor != null) openDoor.SetActive(false); // Ensure the open door is initially inactive
        if (questions.Count == 0)
        {
            Debug.LogError($"No questions assigned to {gameObject.name}! Please add questions.");
        }
    }

    private void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E) && !isQuestionAnswered)
        {
            ActivateQuestionPanel();
        }
    }

    private void ActivateQuestionPanel()
    {
        if (questions.Count == 0 || currentQuestionIndex >= questions.Count)
        {
            Debug.LogError($"No questions available or all questions have been answered for {gameObject.name}!");
            return;
        }

        // Set the current door
        currentDoor = this;

        // Update the question text and display the panel
        Question currentQuestion = questions[currentQuestionIndex];
        questionText.text = currentQuestion.questionText;
        mathQuestionPanel.SetActive(true);

        // Ensure only one listener for the submit button
        submitButton.onClick.RemoveAllListeners();
        submitButton.onClick.AddListener(CheckAnswer);
    }

    private void CheckAnswer()
    {
        if (questions.Count == 0 || currentQuestionIndex >= questions.Count)
        {
            Debug.LogError($"No questions available or all questions have been answered for {gameObject.name}!");
            return;
        }

        // Get the current question and check the answer
        Question currentQuestion = questions[currentQuestionIndex];
        int playerAnswer;
        if (int.TryParse(answerInputField.text, out playerAnswer))
        {
            if (playerAnswer == currentQuestion.answer)
            {
                Debug.Log($"Correct answer for {gameObject.name}! Proceeding to the next question...");
                currentQuestionIndex++; // Move to the next question

                if (currentQuestionIndex >= questions.Count)
                {
                    // All questions answered, open the door
                    isQuestionAnswered = true;
                    mathQuestionPanel.SetActive(false); // Hide the panel
                    OpenDoor();
                }
                else
                {
                    // Move to the next question
                    ActivateQuestionPanel();
                }
            }
            else
            {
                // Provide feedback for a wrong answer
                questionText.text = "Wrong answer!\nTry again.";
                answerInputField.text = ""; // Clear the input field
                Debug.Log($"Wrong answer for {gameObject.name}! Try again.");
            }
        }
        else
        {
            questionText.text = "Invalid input! Please enter a number.";
            Debug.Log($"Invalid input received for {gameObject.name}.");
        }
    }

    private void OpenDoor()
    {
        if (closedDoor != null) closedDoor.SetActive(false); // Deactivate closed door
        if (openDoor != null) openDoor.SetActive(true); // Activate open door
        if (obstacle != null) obstacle.SetActive(false); // Deactivate optional obstacle
        Debug.Log($"All questions answered for {gameObject.name}! Door is now open.");
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
            if (currentDoor == this)
            {
                mathQuestionPanel.SetActive(false); // Hide the panel if this door is active
            }
        }
    }
}

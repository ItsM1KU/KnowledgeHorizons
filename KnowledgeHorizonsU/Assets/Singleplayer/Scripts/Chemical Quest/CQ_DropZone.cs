using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class DropZone : MonoBehaviour, IDropHandler
{
    public Image beakerImage;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI questionText;
    public TextMeshProUGUI feedbackText;
    public Button hintButton;  // Reference for the Hint Button
    public GameObject reactionTrackerEntryPrefab;  // Prefab for reaction entry
    public Transform reactionTrackerPanel;  // Panel to hold reaction entries
    public float reactionTime = 10f;
    
    private List<string> droppedElements = new List<string>();
    private Dictionary<(string, string), Color> reactions = new Dictionary<(string, string), Color>();
    private float timer;
    private int score = 0;
    private bool isReacting = false;
    
    private List<(string, string, string)> allQuestions = new List<(string, string, string)>();
    private List<(string, string, string)> questions = new List<(string, string, string)>();
    private int currentQuestionIndex = 0;

    void Start() {
        // Define valid reactions and their colors
        reactions.Add(("Hydrogen", "Oxygen"), Color.cyan);
        reactions.Add(("Oxygen", "Hydrogen"), Color.cyan);

        reactions.Add(("Sodium", "Chlorine"), Color.yellow);
        reactions.Add(("Chlorine", "Sodium"), Color.yellow);

        reactions.Add(("HCl", "NaOH"), Color.green);
        reactions.Add(("NaOH", "HCl"), Color.green);

        // Define all tricky questions
        allQuestions.Add(("Which two elements combine to form a molecule that supports combustion?", "Hydrogen", "Oxygen"));
        allQuestions.Add(("This acidic and basic substance react to neutralize each other. Who are they?", "HCl", "NaOH"));
        allQuestions.Add(("Which two elements form a compound commonly used for food preservation?", "Sodium", "Chlorine"));
        allQuestions.Add(("What two elements together form a polar molecule vital for life?", "Hydrogen", "Oxygen"));
        allQuestions.Add(("Identify the pair that reacts to form a compound with a salty taste.", "Sodium", "Chlorine"));
        allQuestions.Add(("Which elements, when mixed, produce a product used for cleaning wounds?", "Hydrogen", "Oxygen"));
        allQuestions.Add(("Choose the acid and base that neutralize each other forming a green product.", "HCl", "NaOH"));
        allQuestions.Add(("Which two reactants would likely release energy and form H₂O?", "Hydrogen", "Oxygen"));
        allQuestions.Add(("This metal and non-metal form a compound essential to nerve function.", "Sodium", "Chlorine"));
        allQuestions.Add(("Which two substances react to reduce both acidity and alkalinity?", "NaOH", "HCl"));

        // Pick 5 random questions
        ShuffleQuestions(allQuestions);
        questions = allQuestions.GetRange(0, 5);

        DisplayQuestion();
        StartNewReaction();
        feedbackText.text = "";

        // Setup hint button
        hintButton.onClick.AddListener(ShowHint);
    }

    void Update() {
        if (!isReacting && currentQuestionIndex < questions.Count) {
            timer -= Time.deltaTime;
            timerText.text = "Time: " + Mathf.Ceil(timer).ToString();
            if (timer <= 0f) {
                ShowFeedback("Wrong!");
                Invoke(nameof(NextQuestion), 1.5f);
            }
        }
    }

    public void OnDrop(PointerEventData eventData) {
        GameObject dropped = eventData.pointerDrag;
        if (dropped != null && droppedElements.Count < 2) {
            string elementName = dropped.name.Replace("Element_", "");
            droppedElements.Add(elementName);
            dropped.GetComponent<ElementDrag>().ReturnToOriginal();

            if (droppedElements.Count == 2) {
                React();
            }
        }
    }

    void React() {
        isReacting = true;
        string e1 = droppedElements[0];
        string e2 = droppedElements[1];

        var expected = questions[currentQuestionIndex];

        // Check if the reaction is correct
        if ((e1 == expected.Item2 && e2 == expected.Item3) || (e1 == expected.Item3 && e2 == expected.Item2)) {
            if (reactions.ContainsKey((e1, e2))) {
                beakerImage.color = reactions[(e1, e2)];
            } else if (reactions.ContainsKey((e2, e1))) {
                beakerImage.color = reactions[(e2, e1)];
            }

            score += 10;
            scoreText.text = "Score: " + score;
            ShowFeedback("Correct!");

            // Log this reaction in the tracker
            LogReaction(e1, e2, true);
        } else {
            beakerImage.color = Color.black;
            ShowFeedback("Wrong!");
            
            // Log this reaction in the tracker
            LogReaction(e1, e2, false);
        }

        Invoke(nameof(NextQuestion), 1.5f);
    }

    void LogReaction(string element1, string element2, bool correct) {
        // Instantiate a new reaction entry
        GameObject entry = Instantiate(reactionTrackerEntryPrefab, reactionTrackerPanel);
        TextMeshProUGUI entryText = entry.GetComponentInChildren<TextMeshProUGUI>();
        entryText.text = $"{element1} + {element2} → {(correct ? "Correct" : "Wrong")}";
    }

    void NextQuestion() {
        currentQuestionIndex++;
        if (currentQuestionIndex >= questions.Count) {
            questionText.text = "Quiz complete!";
            timerText.text = "Time: 0";
            feedbackText.text = "";
            return;
        }
        StartNewReaction();
        DisplayQuestion();
    }

    void StartNewReaction() {
        timer = reactionTime;
        droppedElements.Clear();
        beakerImage.color = Color.white;
        isReacting = false;
    }

    void DisplayQuestion() {
        if (currentQuestionIndex < questions.Count) {
            questionText.text = questions[currentQuestionIndex].Item1;
        }
    }

    void ShowFeedback(string message) {
        feedbackText.text = message;
        Invoke(nameof(HideFeedback), 1.5f);
    }

    void HideFeedback() {
        feedbackText.text = "";
    }

    void ShuffleQuestions(List<(string, string, string)> list) {
        for (int i = 0; i < list.Count; i++) {
            int randIndex = Random.Range(i, list.Count);
            var temp = list[i];
            list[i] = list[randIndex];
            list[randIndex] = temp;
        }
    }

    void ShowHint() {
        // Display a hint for the current question
        var expected = questions[currentQuestionIndex];
        feedbackText.text = $"Hint: Try mixing {expected.Item2} and {expected.Item3}.";
        Invoke(nameof(HideFeedback), 3f);
    }
}

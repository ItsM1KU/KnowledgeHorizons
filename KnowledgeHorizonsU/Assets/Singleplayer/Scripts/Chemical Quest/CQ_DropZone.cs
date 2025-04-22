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
    public float reactionTime = 10f;

    private List<string> droppedElements = new List<string>();
    private Dictionary<(string, string), Color> reactions = new Dictionary<(string, string), Color>();
    private float timer;
    private int score = 0;
    private bool isReacting = false;

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

        // Questions based on available elements
        questions.Add(("What elements make water?", "Hydrogen", "Oxygen"));
        questions.Add(("What elements make salt?", "Sodium", "Chlorine"));
        questions.Add(("Which form a neutralization product?", "HCl", "NaOH"));
        questions.Add(("What combines with Hydrogen to form a product?", "Hydrogen", "Oxygen"));
        questions.Add(("Which pair creates a green reaction?", "NaOH", "HCl"));

        DisplayQuestion();
        StartNewReaction();
    }

    void Update() {
        if (!isReacting && currentQuestionIndex < questions.Count) {
            timer -= Time.deltaTime;
            timerText.text = "Time: " + Mathf.Ceil(timer).ToString();
            if (timer <= 0f) {
                NextQuestion();
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

        if ((e1 == expected.Item2 && e2 == expected.Item3) || (e1 == expected.Item3 && e2 == expected.Item2)) {
            // Correct reaction
            if (reactions.ContainsKey((e1, e2))) {
                beakerImage.color = reactions[(e1, e2)];
            } else if (reactions.ContainsKey((e2, e1))) {
                beakerImage.color = reactions[(e2, e1)];
            }

            score += 10;
            scoreText.text = "Score: " + score;
        } else {
            // Wrong reaction
            beakerImage.color = Color.black;
        }

        Invoke(nameof(NextQuestion), 2f);
    }

    void NextQuestion() {
        currentQuestionIndex++;
        if (currentQuestionIndex >= questions.Count) {
            questionText.text = "Quiz complete!";
            timerText.text = "Time: 0";
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
}

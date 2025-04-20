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
    public float reactionTime = 10f;

    private List<string> droppedElements = new List<string>();
    private Dictionary<(string, string), Color> reactions = new Dictionary<(string, string), Color>();
    private float timer;
    private int score = 0;
    private bool isReacting = false;

    void Start() {
        reactions.Add(("Hydrogen", "Oxygen"), Color.cyan);
        reactions.Add(("Sodium", "Chlorine"), Color.yellow);
        reactions.Add(("Iron", "Sulfur"), Color.red);
        StartNewReaction();
    }

    void Update() {
        if (!isReacting) {
            timer -= Time.deltaTime;
            timerText.text = "Time: " + Mathf.Ceil(timer).ToString();
            if (timer <= 0f) {
                StartNewReaction();
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

        var key = (e1, e2);
        var reverseKey = (e2, e1);

        if (reactions.ContainsKey(key)) {
            beakerImage.color = reactions[key];
            score += 10;
        } else if (reactions.ContainsKey(reverseKey)) {
            beakerImage.color = reactions[reverseKey];
            score += 10;
        } else {
            beakerImage.color = Color.black; // wrong
        }

        scoreText.text = "Score: " + score;
        Invoke(nameof(StartNewReaction), 1f);
    }

    void StartNewReaction() {
        timer = reactionTime;
        droppedElements.Clear();
        beakerImage.color = Color.white;
        isReacting = false;
    }
}

using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BeakerZone : MonoBehaviour
{
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI scoreText;
    private ReactionQuestion.ReactionData currentReaction;
    private int score = 0;

    private string droppedElement1 = "";
    private string droppedElement2 = "";

    public void SetCurrentReaction(ReactionQuestion.ReactionData reaction)
    {
        currentReaction = reaction;
        droppedElement1 = "";
        droppedElement2 = "";
        resultText.text = "";  // Clear previous result
    }

    public void OnElementDropped(string elementName)
    {
        if (droppedElement1 == "")
        {
            droppedElement1 = elementName;
        }
        else if (droppedElement2 == "")
        {
            droppedElement2 = elementName;

            CheckReaction();
        }
    }

    void CheckReaction()
    {
        bool isCorrect = (droppedElement1 == currentReaction.element1 && droppedElement2 == currentReaction.element2) ||
                         (droppedElement1 == currentReaction.element2 && droppedElement2 == currentReaction.element1);

        if (isCorrect)
        {
            resultText.text = currentReaction.reactionText;
            resultText.color = currentReaction.resultColor;
            score += 10;
        }
        else
        {
            resultText.text = "Incorrect Reaction!";
            resultText.color = Color.red;
        }

        scoreText.text = "Score: " + score;

        // Reset for next round
        droppedElement1 = "";
        droppedElement2 = "";

        // Pick new question
        FindObjectOfType<ReactionQuestion>().PickRandomQuestion();
    }
}

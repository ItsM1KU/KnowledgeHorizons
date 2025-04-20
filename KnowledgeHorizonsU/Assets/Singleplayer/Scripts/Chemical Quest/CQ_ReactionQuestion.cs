using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class ReactionQuestion : MonoBehaviour
{
    public TextMeshProUGUI questionText;
    public BeakerZone beakerZone;  // renamed from DropZone

    [System.Serializable]
    public class ReactionData
    {
        public string element1;
        public string element2;
        public string question;
        public string reactionText;
        public Color resultColor;
    }

    public List<ReactionData> allReactions = new List<ReactionData>();
    private ReactionData currentReaction;

    void Start()
    {
        PickRandomQuestion();
    }

    public void PickRandomQuestion()
    {
        currentReaction = allReactions[Random.Range(0, allReactions.Count)];
        questionText.text = "Question: " + currentReaction.question;
        beakerZone.SetCurrentReaction(currentReaction); // updated
    }
}

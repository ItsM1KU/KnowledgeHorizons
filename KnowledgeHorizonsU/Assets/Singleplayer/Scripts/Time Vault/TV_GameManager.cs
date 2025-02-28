using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TV_GameManager : MonoBehaviour
{
    public static TV_GameManager Instance;
    [SerializeField] Transform playerTransform;

    // Artifact Details UI
    [SerializeField] GameObject artifactUI;
    [SerializeField] Image artifactImage;
    [SerializeField] TextMeshProUGUI artifactName;
    [SerializeField] TextMeshProUGUI artifactDescription;

    private int correctAnswerMin = 1950;
    private int correctAnswerMax = 1960;
    private int playerAnswer;

    //UI
    [SerializeField] TMP_InputField AnswerInputField;
    [SerializeField] GameObject QuestionUI;
 
    //Dialogs 
    [SerializeField] List<string> QuestionDialog;
    [SerializeField] List<string> AnswerDialog;
    [SerializeField] List<string> WrongDialog;

    [SerializeField] LayerMask DoorLayer;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Update()
    {
        if (TV_InputManager.Instance.isInteracting)
        {
            Debug.Log("this works");
            if (Physics2D.OverlapCircle(playerTransform.position, 0.5f, DoorLayer) && !TV_DialogManager.Instance.isPresenting)

            {
                Debug.Log("Answer the question");
                StartCoroutine(DoorDialog());
            }
        }
    }


    public void SetupArtifactUI(Collider2D artifactCollider)
    {
       if(artifactCollider != null)
        {
            TV_Artifact artifact = artifactCollider.GetComponent<TV_Artifact>();
            if (artifact != null)
            {
                artifactUI.SetActive(true);
                artifactImage.sprite = artifact.ArtifactSprite;
                artifactName.text = artifact.ArtifactName;
                artifactDescription.text = artifact.ArtifactDescription;
            }
        }
    }

    public void CloseArtifactUI()
    {
        artifactUI.SetActive(false);
    }

    public IEnumerator DoorDialog()
    {
        yield return TV_DialogManager.Instance.StartDialog(QuestionDialog);
        yield return new WaitForSeconds(1f);
        QuestionUI.SetActive(true);
    }

    public void AnswerSubmit()
    {
        if(int.TryParse(AnswerInputField.text, out playerAnswer))
        {
            if (playerAnswer >= correctAnswerMin && playerAnswer <= correctAnswerMax)
            {
                Debug.Log("Correct Answer");
                StartCoroutine(correctAnswer());
            }
            else
            {
                Debug.Log("Wrong Answer");
                StartCoroutine(wrongAnswer());
            }
        }
        else
        {
            Debug.Log("Please enter a valid number!!");
        }
    }

    public IEnumerator correctAnswer()
    {
        QuestionUI.SetActive(false);
        yield return TV_DialogManager.Instance.StartDialog(AnswerDialog);
    }

    public IEnumerator wrongAnswer()
    {
        QuestionUI.SetActive(false);
        yield return TV_DialogManager.Instance.StartDialog(WrongDialog);
    }


}

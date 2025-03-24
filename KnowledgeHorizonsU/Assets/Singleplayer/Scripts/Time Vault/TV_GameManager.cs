using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TV_GameManager : MonoBehaviour
{
    public static TV_GameManager Instance;
    [SerializeField] Transform playerTransform;
    [SerializeField] GameObject pauseMenu;

    // Artifact Details UI
    [SerializeField] GameObject artifactUI;
    [SerializeField] Image artifactImage;
    [SerializeField] TextMeshProUGUI artifactName;
    [SerializeField] TextMeshProUGUI artifactDescription;

    private int[] correctRange = new int[2];
    private int playerAnswer;

    //UI
    [SerializeField] TMP_InputField AnswerInputField;
    [SerializeField] GameObject QuestionUI;
 
    //Dialogs 
    [SerializeField] List<string> QuestionDialog;
    private List<string> AnswerDialog = new List<string>();
    [SerializeField] List<string> WrongDialog;

    //Dialogs for each Scenario
    [SerializeField] List<string> MusicDialog;
    [SerializeField] List<string> SpaceDialog;
    [SerializeField] List<string> WarDialog;

    //Layer Mask
    [SerializeField] LayerMask DoorLayer;

    //Rando Artifact List
    [SerializeField] List<GameObject[]> artifactList = new List<GameObject[]>();
    [SerializeField] GameObject[] musicArtifacts;
    [SerializeField] GameObject[] spaceArtifacts;
    [SerializeField] GameObject[] warArtifacts;
    [SerializeField]
    List<int[]> artifactYears = new List<int[]>{
            new int[] { 1950, 1960 },
            new int[] { 1968, 1969 },
            new int[] { 1914, 1918 }
    };
    [SerializeField] List<List<string>> artifactDialogs = new List<List<string>>();  


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        artifactList.Add(musicArtifacts);
        artifactList.Add(spaceArtifacts);
        artifactList.Add(warArtifacts);

        artifactDialogs.Add(MusicDialog);
        artifactDialogs.Add(SpaceDialog);
        artifactDialogs.Add(WarDialog);
    }

    private void Start()
    {
        setupRoom();
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

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("Islands");
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseMenu.activeSelf)
            {
                pauseMenu.SetActive(false);
                Time.timeScale = 1;
            }
            else
            {
                pauseMenu.SetActive(true);
                Time.timeScale = 0;
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
            if (playerAnswer >= correctRange[0] && playerAnswer <= correctRange[1])
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
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Islands");
    }

    public IEnumerator wrongAnswer()
    {
        QuestionUI.SetActive(false);
        yield return TV_DialogManager.Instance.StartDialog(WrongDialog);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Islands");
    }

    private void setupRoom()
    {
        int randomIndex = UnityEngine.Random.Range(0, 3);
        GameObject[] currentArtifactList = artifactList[randomIndex];
        correctRange = artifactYears[randomIndex];
        AnswerDialog = artifactDialogs[randomIndex];
        for (int i = 0; i < currentArtifactList.Length; i++)
        {
            currentArtifactList[i].SetActive(true);
        }
    }


    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("TV_gamescene"); 
    }

    public void Islands()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Islands");
    }
}

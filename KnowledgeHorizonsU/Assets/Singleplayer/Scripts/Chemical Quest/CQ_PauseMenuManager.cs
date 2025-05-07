using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PauseMenuManager : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public Button resumeButton;
    public Button restartButton;
    public Button backToIslandsButton;

    public GameObject[] objectsToHideWhenPaused;

    private bool isPaused = false;

    [SerializeField] GameObject endGamePanel;
    [SerializeField] Text endGameScoreText;
    public Button endIslandButton;
    public Button endRestartButton;
 
    void Start()
    {
        ResumeGame();
        pauseMenuUI.SetActive(false);
        endGamePanel.SetActive(false);
        resumeButton.onClick.AddListener(ResumeGame);
        restartButton.onClick.AddListener(RestartGame);
        backToIslandsButton.onClick.AddListener(BacktoIslands);
        endRestartButton.onClick.AddListener(RestartGame);
        endIslandButton.onClick.AddListener(BacktoIslands);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        foreach (GameObject obj in objectsToHideWhenPaused)
        {
            if (obj != null)
                obj.SetActive(false);
        }
        Time.timeScale = 0f;
        isPaused = true;
    }

    void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        foreach (GameObject obj in objectsToHideWhenPaused)
        {
            if (obj != null)
                obj.SetActive(true);
        }
        Time.timeScale = 1f;
        isPaused = false;
    }

    void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void BacktoIslands()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Islands");
    }

    public void EndGame(int score)
    {
        endGamePanel.SetActive(true);
        endGameScoreText.text = "Score: " + score.ToString();
        foreach (GameObject obj in objectsToHideWhenPaused)
        {
            if (obj != null)
                obj.SetActive(false);
        }
        Time.timeScale = 0f;
    }
}

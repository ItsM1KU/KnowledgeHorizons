using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public Button resumeButton;
    public Button restartButton;

    public GameObject[] objectsToHideWhenPaused;

    private bool isPaused = false;

    void Start()
    {
        pauseMenuUI.SetActive(false);
        resumeButton.onClick.AddListener(ResumeGame);
        restartButton.onClick.AddListener(RestartGame);
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
}

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject pausePanel; // Reference to the Pause Panel
    public GameManager gameManager; // Reference to the GameManager script

    private bool isPaused = false;

    private void Update()
    {
        // Toggle Pause when Escape is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        pausePanel.SetActive(true); // Show the pause panel
        Time.timeScale = 0; // Freeze game time
    }

    public void ResumeGame()
    {
        isPaused = false;
        pausePanel.SetActive(false); // Hide the pause panel
        Time.timeScale = 1; // Resume game time
    }

    public void RestartGame()
    {
        Time.timeScale = 1; // Ensure game time is running
        gameManager.RestartGame(); // Restart the game via GameManager
    }

    public void GoToIslands()
    {
        Time.timeScale = 1; // Ensure game time is running
        SceneManager.LoadScene("IslandsScene"); // Replace "IslandsScene" with your scene name
    }
}

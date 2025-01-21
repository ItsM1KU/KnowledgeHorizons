using UnityEngine;

public class MM_PauseMenuController : MonoBehaviour
{
    public GameObject pauseMenuPanel; // Reference to the Pause Menu Panel
    public GameObject gameplayUI; // Reference to the gameplay UI (if any, optional)
    private bool isPaused = false; // Tracks whether the game is paused

    private void Update()
    {
        // Check if the "ESC" key is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame(); // Resume if already paused
            }
            else
            {
                PauseGame(); // Pause if currently playing
            }
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        pauseMenuPanel.SetActive(true); // Show the Pause Menu
        if (gameplayUI != null) gameplayUI.SetActive(false); // Hide gameplay UI (optional)
        Time.timeScale = 0f; // Stop the game by freezing time
    }

    public void ResumeGame()
    {
        isPaused = false;
        pauseMenuPanel.SetActive(false); // Hide the Pause Menu
        if (gameplayUI != null) gameplayUI.SetActive(true); // Show gameplay UI (optional)
        Time.timeScale = 1f; // Resume the game by unfreezing time
    }
}

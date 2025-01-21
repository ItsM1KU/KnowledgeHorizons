using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MM_GameCompletionManager : MonoBehaviour
{
    public GameObject gameClearedUI; // Reference to the Game Cleared UI
    public GameObject pauseMenuUI; // Reference to the Pause Menu UI

    private bool gameCleared = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // Check if the player collides with the finish point
        {
            ShowGameClearedUI();
        }
    }

    private void Update()
    {
        if (gameCleared && Input.GetKeyDown(KeyCode.Return)) // Check if 'Enter' is pressed after clearing the game
        {
            ShowPauseMenu();
        }
    }

    private void ShowGameClearedUI()
    {
        gameCleared = true; // Mark the game as cleared
        gameClearedUI.SetActive(true); // Show the "Game Cleared" UI
        Time.timeScale = 0f; // Pause the game
    }

    private void ShowPauseMenu()
    {
        gameClearedUI.SetActive(false); // Hide the "Game Cleared" UI
        pauseMenuUI.SetActive(true); // Show the pause menu
    }

    // Pause Menu Button Functions
    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false); // Hide the pause menu
        Time.timeScale = 1f; // Resume the game
    }

    public void RetryGame()
    {
        Time.timeScale = 1f; // Resume the game
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reload the current scene
    }

    public void ReturnToIsland()
    {
        Time.timeScale = 1f; // Resume the game
        SceneManager.LoadScene("Islands"); // Replace with your island scene name
    }
}

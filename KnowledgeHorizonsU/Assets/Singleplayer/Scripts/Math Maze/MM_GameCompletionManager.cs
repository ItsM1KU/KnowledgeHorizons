using UnityEngine;
using UnityEngine.SceneManagement;

public class MM_GameCompletionManager : MonoBehaviour
{
    public GameObject gameClearedUI; // Reference to the Game Cleared UI
    public GameObject returnToIslandsButton; // Reference to the 'Return to Islands' button

    private bool gameCleared = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // Check if the player collides with the finish point
        {
            ShowGameClearedUI();
        }
    }

    private void ShowGameClearedUI()
    {
        gameCleared = true; // Mark the game as cleared
        gameClearedUI.SetActive(true); // Show the "Game Cleared" UI
        returnToIslandsButton.SetActive(true); // Show the 'Return to Islands' button
        Time.timeScale = 0f; // Pause the game
    }

    // 'Return to Islands' Button Function
    public void ReturnToIsland()
    {
        Time.timeScale = 1f; // Resume the game
        SceneManager.LoadScene("Islands"); // Replace with your island scene name
    }
}

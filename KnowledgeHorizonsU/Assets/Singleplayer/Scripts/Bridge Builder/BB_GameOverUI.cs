using UnityEngine;
using UnityEngine.SceneManagement;

public class BB_GameOverUI : MonoBehaviour
{
    public GameObject gameOverPanel;

    private void Start()
    {
        gameOverPanel.SetActive(false);  // Hide panel initially
    }

    public void ShowGameOver()
    {
        gameOverPanel.SetActive(true);  // Show panel when game over
        Time.timeScale = 0f;  // Pause the game
    }

    public void Retry()
    {
        Time.timeScale = 1f;  // Resume time
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);  // Restart current scene
    }

    public void ReturnToIsland()
    {
        Time.timeScale = 1f;  // Resume time
        SceneManager.LoadScene("Islands");  // Load island selection scene
    }
}

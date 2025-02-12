using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FB_PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuPanel; // ✅ Reference to the Pause Menu UI Panel
    public Slider volumeSlider; // ✅ Volume control slider
    private bool isPaused = false;

    void Start()
    {
        // Ensure the pause menu is hidden at the start
        pauseMenuPanel.SetActive(false);

        // Load saved volume setting
        float savedVolume = PlayerPrefs.GetFloat("GameVolume", 1f);
        volumeSlider.value = savedVolume;
        AudioListener.volume = savedVolume;

        // Add listener for volume changes
        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    void Update()
    {
        // ✅ Ensure Escape key always works, even during question panel phase
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }
    }

    public void TogglePauseMenu()
    {
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        Debug.Log("Pause Menu Activated"); // ✅ Debugging line

        isPaused = true;
        pauseMenuPanel.SetActive(true); // ✅ Show the pause menu
        Time.timeScale = 0f; // ✅ Pause the game
    }

    public void ResumeGame()
    {
        Debug.Log("Game Resumed"); // ✅ Debugging line

        isPaused = false;
        pauseMenuPanel.SetActive(false); // ✅ Hide the pause menu
        Time.timeScale = 1f; // ✅ Resume the game
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f; // ✅ Reset time before leaving
        SceneManager.LoadScene("Islands"); // ✅ Change this to your main menu scene
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume; // ✅ Adjust global game volume
        PlayerPrefs.SetFloat("GameVolume", volume); // ✅ Save volume setting
    }
}

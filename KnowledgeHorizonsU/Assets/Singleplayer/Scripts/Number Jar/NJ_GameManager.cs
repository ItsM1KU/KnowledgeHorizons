using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NJ_GameManager : MonoBehaviour
{
    public static NJ_GameManager instance;

    public int currentScore { get; set; }

    public AudioSource popSFX;

    public float timeOverLimit = 2f;

    public TextMeshProUGUI scoreText;

    [SerializeField] GameObject PauseMenu;

    [SerializeField] GameObject gameOverMenu;

    [SerializeField] public TextMeshProUGUI scoreGO;

    private bool ispaused;
    private void Awake()
    {
        if (instance == null) { 
            instance = this;
        }
        scoreText.text = "0";

        Time.timeScale = 1f;
        PauseMenu.SetActive(false);
        ispaused = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !ispaused)
        {   
            PauseMenu.SetActive(true);
            Time.timeScale = 0f;
            ispaused = true;
        }
    }

    public void IncreaseScore(int amount)
    {
        currentScore += amount;
        scoreText.text = currentScore.ToString();
    }

    public void GameOver()
    {
        Time.timeScale = 0f;
        gameOverMenu.SetActive(true);
        scoreGO.text = "Score · " + currentScore.ToString();
    }

    public void Resume()
    {
        PauseMenu.SetActive(false);
        Time.timeScale = 1f;
        ispaused = false;
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("NJ_gamescene");
    }

    public void LoadIslands()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Islands");
    }
    

    
}

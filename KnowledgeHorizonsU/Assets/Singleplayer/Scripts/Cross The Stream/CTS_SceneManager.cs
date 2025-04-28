using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CTS_SceneManager : MonoBehaviour
{
    public static CTS_SceneManager instance;

    [SerializeField] Text endScoreText;

    private bool isPaused;
    private bool canPause;

    [SerializeField] GameObject PauseMenu;
    [SerializeField] GameObject GameOverMenu;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    private void Start()
    {
        isPaused = false;
        canPause = true;
        Time.timeScale = 1f;

        PauseMenu.SetActive(false);
        GameOverMenu.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && canPause)
        {
            Time.timeScale = 0f;
            isPaused = true;
            PauseMenu.SetActive(true);
        }
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        isPaused = false;
        PauseMenu.SetActive(false);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Islands()
    {
        SceneManager.LoadScene("Islands");
    }

    public void EndGame()
    {
        canPause = false;
        Time.timeScale = 0f;
        endScoreText.text = "Score: " + CTS_Score.score.ToString();
        GameOverMenu.SetActive(true);

    }
}

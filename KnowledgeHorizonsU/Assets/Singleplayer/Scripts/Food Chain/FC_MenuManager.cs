using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FC_MenuManager : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseMenu.activeSelf)
            {
                pauseMenu.SetActive(false);
                Time.timeScale = 1;
            }
            else
            {
                pauseMenu.SetActive(true);
                Time.timeScale = 0;
            }
        }
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("FC_gamescene");       
    }

    public void Islands()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Islands");
    }

}

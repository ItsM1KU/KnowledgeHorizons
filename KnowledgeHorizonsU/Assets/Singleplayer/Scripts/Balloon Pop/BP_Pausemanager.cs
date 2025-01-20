using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{

    public static PauseManager instance;

    [SerializeField] GameObject PausePanel;
    public bool isPaused;
    public bool canPause;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        isPaused = false;
        PausePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape) && !isPaused && canPause)
        { 
            PausePanel.SetActive(true);
            isPaused = true;
            Time.timeScale = 0;
        }
    }

    public void resume()
    {
        isPaused = false;
        PausePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void restart()
    {
        SceneManager.LoadScene("BP_gamescene");
    }

    public void islands()
    {
        SceneManager.LoadScene("islands");
    }
}

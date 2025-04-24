using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ERT_GameManager : MonoBehaviour
{
    public static ERT_GameManager instance;

    [SerializeField] int FruitsToCollect;
    [SerializeField] TMP_Text FruitText;

    private int FruitsCollected;
    private int enemiesKilled;

    [SerializeField] GameObject endScreen;
    [SerializeField] TMP_Text endText;

    [SerializeField] GameObject PauseMenu;
    private bool isPaused;
    private bool canPause = true;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        isPaused = false;
        PauseMenu.SetActive(false);
        endScreen.SetActive(false);
        Time.timeScale = 1f;
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isPaused && canPause)
        {
            PauseMenu.SetActive(true);
            isPaused = true;
            Time.timeScale = 0;
        }
    }

    public void EndGame()
    {
        Time.timeScale = 0;
        canPause = false;
        endScreen.SetActive(true);
        endText.text = "You collected " + FruitsCollected.ToString() + " fruits and killed " + enemiesKilled.ToString() + " enemies";
        StartCoroutine(ending());
    }

    public void EnemyKilled()
    {
        enemiesKilled++;
    }

    public void FruitCollection()
    {
        FruitsCollected++;
        FruitText.text = (FruitsCollected.ToString() + "/" + FruitsToCollect.ToString());
        if(FruitsCollected >= FruitsToCollect)
        {
            Debug.Log("Fruits collededt");
        }
    }

    public void resume()
    {
        isPaused = false;
        PauseMenu.SetActive(false);
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

    public IEnumerator ending()
    {
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("islands");
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NJ_GameManager : MonoBehaviour
{
    public static NJ_GameManager instance;

    public int currentScore { get; set; }

    public float timeOverLimit = 2f;

    public TextMeshProUGUI scoreText;
    private void Awake()
    {
        if (instance == null) { 
            instance = this;
        }
        scoreText.text = "0";
    }

    public void IncreaseScore(int amount)
    {
        currentScore += amount;
        scoreText.text = currentScore.ToString();
    }

    public void GameOver()
    {
        Debug.Log("Game Over");
    }
}

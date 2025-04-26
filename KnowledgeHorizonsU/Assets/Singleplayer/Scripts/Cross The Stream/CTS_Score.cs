using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CTS_Score : MonoBehaviour
{
    [SerializeField] TMP_Text scoreText;

    public static int score;

    private void Start()
    {
        scoreText.text = score.ToString();
    }
}

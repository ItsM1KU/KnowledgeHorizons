using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NJ_BallSelector : MonoBehaviour
{
    public static NJ_BallSelector Instance;

    public GameObject[] balls;
    public GameObject[] ballsNP;
    public int maxStartingBallIndex = 2;

    [SerializeField] private Image nextBallImage;
    [SerializeField] private Sprite[] ballSprites;

    public GameObject nextball { get; private set; }


    // Start is called before the first frame update
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    private void Start()
    {
        pickNextBall();
    }

    public GameObject PickStartingRandomBall()
    {
        int randomIndex = Random.Range(0, maxStartingBallIndex + 1);

        if (randomIndex < ballsNP.Length)
        {
            GameObject randomBall = ballsNP[randomIndex];
            return randomBall;
        }
        return null;
    }

    public void pickNextBall()
    {
        int randomIndex = Random.Range(0, maxStartingBallIndex + 1);

        if (randomIndex < balls.Length)
        {
            GameObject nextBall = ballsNP[randomIndex];
            nextball = nextBall;

            nextBallImage.sprite = ballSprites[randomIndex];
        }
    }
}

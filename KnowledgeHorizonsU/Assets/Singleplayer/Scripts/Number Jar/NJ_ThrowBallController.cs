using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class NJ_ThrowBallController : MonoBehaviour
{
    public static NJ_ThrowBallController Instance;
    public GameObject currentBall { get; set; }

    [SerializeField] private Transform ballTransform;
    [SerializeField] private Transform parentAfterThrow;
    [SerializeField] private NJ_BallSelector ballSelector;

    private NJ_PlayerController playerController;

    private Rigidbody2D rb;
    private CircleCollider2D circleCollider;

    public Bounds bounds { get; private set; }
    private const float extraWidth = 0.03f;

    public bool canthrow { get; set; } = true;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        playerController = GetComponent<NJ_PlayerController>();

        SpawnBall(ballSelector.PickStartingRandomBall());
    }

    private void Update()
    {
        if (NJ_UserInput.isThrowPressed && canthrow)
        {
            NJ_BallIndex index = currentBall.GetComponent<NJ_BallIndex>();
            quaternion rot = currentBall.transform.rotation;

            GameObject go = Instantiate(ballSelector.balls[index.ballIndex], currentBall.transform.position, rot);
            go.transform.SetParent(parentAfterThrow);

            Destroy(currentBall);

            canthrow = false;
        }
    }

    public void SpawnBall(GameObject ball)
    {
        GameObject gop = Instantiate(ball);
        gop.transform.position = ballTransform.position;
        gop.transform.rotation = ballTransform.rotation;
        gop.transform.SetParent(ballTransform);


        currentBall = gop;
        circleCollider = currentBall.GetComponent<CircleCollider2D>();
        float ballRadius = circleCollider.radius * currentBall.transform.localScale.x;
        playerController.UpdateBoundaries(ballRadius);
        bounds = circleCollider.bounds;

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NJ_BallInfo : MonoBehaviour
{
    public int BallIndex;
    public int pointsWhenCombined;
    public float ballMass;

    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.mass = ballMass;
    }
}

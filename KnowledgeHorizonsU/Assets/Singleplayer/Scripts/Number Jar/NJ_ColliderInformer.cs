using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NJ_ColliderInformer : MonoBehaviour
{
    public bool wasCombined { get; set; }

    private bool hasCollided;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(!hasCollided && !wasCombined)
        {
            hasCollided = true;
            NJ_ThrowBallController.Instance.canthrow = true;
            NJ_ThrowBallController.Instance.SpawnBall(NJ_BallSelector.Instance.nextball);
            NJ_BallSelector.Instance.pickNextBall();
            Destroy(this);
        }
    }
}

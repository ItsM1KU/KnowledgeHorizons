using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ERT_EnemyAI : MonoBehaviour
{
    [Header("Patrol References")]
    [SerializeField] Transform pointA;
    [SerializeField] Transform pointB;

    [Header("Enemy Settings")]
    [SerializeField] float speed;

    private Rigidbody2D rb;
    private Animator anim;
    private Transform currentPoint;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentPoint = pointA;
        anim.SetBool("isMoving", true);
    }

    private void Update()
    {
        if (currentPoint == pointB)
        {
            rb.velocity = new Vector2(speed, 0);
        }
        else if (currentPoint == pointA)
        {
            rb.velocity = new Vector2(-speed, 0);
        }

        if(Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == pointB)
        {
            StartCoroutine(Idle(pointA));
        }
        else if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == pointA)
        {
            StartCoroutine(Idle(pointB));
        }
    }

    private void flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(pointA.position, 0.5f);
        Gizmos.DrawWireSphere(pointB.position, 0.5f);
    }

    #region Enemy Coroutines

    public IEnumerator Idle(Transform point)
    {
        rb.velocity = Vector2.zero;
        anim.SetBool("isMoving", false);
        yield return new WaitForSeconds(2f);
        anim.SetBool("isMoving", true);
        if(point == pointA)
        {
            currentPoint = pointA;
            transform.localScale = new Vector3(1, 1, 1);
            rb.velocity = new Vector2(-speed, 0);
        }
        else if (point == pointB)
        {
            currentPoint = pointB;
            transform.localScale = new Vector3(-1, 1, 1);
            rb.velocity = new Vector2(speed, 0);
        }
        yield return null;
    }

    #endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ERT_MovingPlatform : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float speed = 5f;

    [SerializeField] Transform pointA;
    [SerializeField] Transform pointB;
    private Transform currentPoint;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        currentPoint = pointB;
    }

    private void Update()
    {
        if(currentPoint == pointB)
        {
            rb.velocity = new Vector2(speed, 0);
        }
        else if (currentPoint == pointA)
        {
            rb.velocity = new Vector2(-speed, 0 );
        }

        if(Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == pointB)
        {
            currentPoint = pointA;
            rb.velocity = new Vector2(-speed, 0);
        }
        else if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == pointA)
        {
            currentPoint = pointB;
            rb.velocity = new Vector2(speed, 0);
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(pointA.position, 0.5f);
        Gizmos.DrawSphere(pointB.position, 0.5f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(this.transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
}

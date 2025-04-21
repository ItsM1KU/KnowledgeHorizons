using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ERT_MovingPlatform : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float speed = 2f;

    [SerializeField] Transform pointA;
    [SerializeField] Transform pointB;
    private Transform targetPosition;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        targetPosition = pointB;
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition.position, speed * Time.deltaTime);

        if(transform.position == targetPosition.position)
        {
            targetPosition = (targetPosition == pointA) ? pointB : pointA;
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

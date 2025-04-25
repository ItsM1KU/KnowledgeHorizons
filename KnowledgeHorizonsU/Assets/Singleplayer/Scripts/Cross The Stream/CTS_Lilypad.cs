using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CTS_Lilypad : MonoBehaviour
{
    [SerializeField] float minSpeed = 1f;
    [SerializeField] float maxSpeed = 3f;

    private Rigidbody2D rb;
    private float speed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        speed = Random.Range(minSpeed, maxSpeed);
    }

    private void FixedUpdate()
    {
        Vector2 forward = new Vector2(transform.right.x, transform.right.y);
        rb.MovePosition(rb.position + forward * Time.fixedDeltaTime * speed);
    }
}

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
        rb.velocity = forward * speed;
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.transform.SetParent(transform);
            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
            collision.transform.position = transform.position;
            CTS_Frog frog = collision.GetComponent<CTS_Frog>();
            frog.isOnLilypad = true;
            if (rb != null)
            {
                rb.isKinematic = true; 
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
            CTS_Frog frog = collision.GetComponent<CTS_Frog>();
            frog.isOnLilypad = false;
            if (rb != null)
            {
                rb.isKinematic = false; 
            }
        }
    }
}

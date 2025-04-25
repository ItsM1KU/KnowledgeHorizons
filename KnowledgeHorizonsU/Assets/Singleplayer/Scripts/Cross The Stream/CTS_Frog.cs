using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CTS_Frog : MonoBehaviour
{

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            rb.MovePosition(rb.position + new Vector2(0, 2));
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            rb.MovePosition(rb.position + new Vector2(0, -2));
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            rb.MovePosition(rb.position + new Vector2(-2, 0));
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            rb.MovePosition(rb.position + new Vector2(2, 0));
        }
    }
}

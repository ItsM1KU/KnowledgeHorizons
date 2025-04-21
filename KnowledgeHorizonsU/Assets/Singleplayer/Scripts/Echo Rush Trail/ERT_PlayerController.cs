using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ERT_PlayerController : MonoBehaviour
{
    [Header("Player Related References")]
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Animator anim;
    [SerializeField] PlayerInput playerInput;

    [Header("Player Settings")]
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float jumpForce = 5f;

    [Header("Ground Check Settings")]
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayer;

    private Vector2 initialPosition;
    private float horizontalInput;
    private bool jumpPressed = false;

    private void Awake()
    {
        initialPosition = transform.position;
    }
    private void Update()
    {
        if (playerInput.actions["Jump"].triggered && isGrounded())
        {
            jumpPressed = true;
        }

        anim.SetBool("Jump", !isGrounded());
        anim.SetFloat("Speed", Mathf.Abs(horizontalInput));
    }

    private void FixedUpdate()
    {
        move(); 
        jump();
    }

    #region Movement

    private void move()
    {
        horizontalInput = playerInput.actions["Move"].ReadValue<Vector2>().x;
        rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);
        if(horizontalInput < 0)
        {
            rb.transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (horizontalInput > 0)
        {
            rb.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    private void jump()
    {
        if (jumpPressed)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpPressed = false; 
        }
    }

    private bool isGrounded()
    {
        return Physics2D.OverlapCapsule(groundCheck.position, new Vector2(0.64f, 0.06f), CapsuleDirection2D.Horizontal, 0f, groundLayer);
    }


    #endregion

    #region Collisions & Triggers

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Trap"))
        {
            transform.position = initialPosition;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Fruit")
        {
            Destroy(collision.gameObject);
            ERT_GameManager.instance.FruitCollection();
        }
    }

    #endregion

}

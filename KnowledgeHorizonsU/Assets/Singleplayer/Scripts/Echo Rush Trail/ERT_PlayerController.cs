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

    [Header("Firearm Settings")]
    [SerializeField] Transform muzzlePosition;
    [SerializeField] GameObject projectilePrefab;

    private float horizontalInput;
    private bool jumpPressed = false;
    private bool isShooting = false;

    private void Awake()
    {
        
    }
    private void Update()
    {
        if (playerInput.actions["Jump"].triggered && isGrounded())
        {
            jumpPressed = true;
        }

        if (playerInput.actions["Shoot"].triggered)
        {
            isShooting = true;
        }
        else
        {
            isShooting = false;
        }


        anim.SetBool("Jump", !isGrounded());
        anim.SetFloat("Speed", Mathf.Abs(horizontalInput));
        anim.SetBool("Shoot", isShooting);
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
}

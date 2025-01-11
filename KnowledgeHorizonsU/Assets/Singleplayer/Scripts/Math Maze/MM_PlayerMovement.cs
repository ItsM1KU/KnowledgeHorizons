using UnityEngine;

public class MM_PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed of movement

    private Vector2 input; // Store player input
    private Animator animator; // Reference to Animator component
    private Rigidbody2D rb; // Reference to Rigidbody2D component

    private void Awake()
    {
        // Get references to required components
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Capture player input for movement
        input.x = Input.GetAxisRaw("Horizontal"); // A/D or Left/Right Arrow
        input.y = Input.GetAxisRaw("Vertical");   // W/S or Up/Down Arrow

        // Prevent diagonal movement
        if (input.x != 0) input.y = 0;

        // Update the animator with movement data
        UpdateAnimator();
    }

    private void FixedUpdate()
    {
        // Apply movement to the Rigidbody2D
        MovePlayer();
    }

    private void UpdateAnimator()
    {
        // Determine if the player is moving
        bool isWalking = input != Vector2.zero;

        // Update Animator parameters
        animator.SetBool("isWalking", isWalking);

        if (isWalking)
        {
            animator.SetFloat("moveX", input.x);
            animator.SetFloat("moveY", input.y);
        }
    }

    private void MovePlayer()
    {
        // Normalize input and calculate movement vector
        Vector2 movement = input.normalized * moveSpeed * Time.fixedDeltaTime;

        // Update the Rigidbody2D position
        rb.MovePosition(rb.position + movement);
    }
}

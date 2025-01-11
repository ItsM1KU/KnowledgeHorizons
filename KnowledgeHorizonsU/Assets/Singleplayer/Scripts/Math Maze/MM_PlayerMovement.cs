using UnityEngine;

public class MM_PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed of movement

    private Vector2 input; // Store player input
    private Animator animator; // Reference to Animator component
    private Rigidbody2D rb; // Reference to Rigidbody2D component

    private Vector2 lastValidPosition; // Store the player's last valid position

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

        // Update animator
        UpdateAnimator();
    }

    private void FixedUpdate()
    {
        // Calculate movement vector
        Vector2 movement = input.normalized * moveSpeed * Time.fixedDeltaTime;

        // Check if the player is moving
        if (movement != Vector2.zero)
        {
            // Move the player
            Vector2 newPosition = rb.position + movement;

            // Use a Physics2D check to ensure the target position is not blocked
            if (!IsBlocked(newPosition))
            {
                rb.MovePosition(newPosition);
                lastValidPosition = rb.position; // Update the last valid position
            }
            else
            {
                // Reset to the last valid position to avoid getting stuck
                rb.MovePosition(lastValidPosition);
            }
        }
    }

    private bool IsBlocked(Vector2 targetPosition)
    {
        // Use Physics2D.OverlapCircle to check for obstacles at the target position
        Collider2D hit = Physics2D.OverlapCircle(targetPosition, 0.1f, LayerMask.GetMask("Obstacles"));
        return hit != null; // Return true if there is an obstacle
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
}

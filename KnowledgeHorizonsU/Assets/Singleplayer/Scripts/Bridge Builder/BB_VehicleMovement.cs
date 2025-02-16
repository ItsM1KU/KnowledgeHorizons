using UnityEngine;

public class BB_VehicleMovement : MonoBehaviour
{
    public float speed = 300f; // Force applied to move the vehicle
    public float offScreenX = 12f; // X position where the vehicle disappears
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (rb == null)
        {
            Debug.LogError("No Rigidbody2D found on the vehicle!");
        }
        else
        {
            rb.gravityScale = 0; // Disable gravity so it doesn’t fall
            rb.drag = 0; // Ensure no unwanted slowing down
            rb.angularDrag = 0; // Prevent unwanted rotation slowdown
            rb.AddForce(Vector2.right * speed);
        }
    }

    void Update()
    {
        // Destroy the vehicle when it reaches the right side of the screen
        if (transform.position.x > offScreenX)
        {
            Destroy(gameObject);
        }
    }
}

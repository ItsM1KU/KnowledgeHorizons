using UnityEngine;

public class BB_Buoyancy : MonoBehaviour
{
    [Header("Buoyancy Settings")]
    public float buoyancyForce = 10f;   // Adjust downward for softer sinking
    public float waterDrag = 5f;        // Increase to slow down movement
    public float waterAngularDrag = 5f; // Controls vehicle rotation
    public float submersionDepth = 1f;  // Determines when full buoyancy applies

    [Header("Water Boundaries")]
    public float waterSurfaceY = 0f;  // Y position of the water surface
    public float waterBottomY = -3f;  // Y position of the water bottom

    private void OnTriggerStay2D(Collider2D other)
    {
        Rigidbody2D rb = other.attachedRigidbody;
        if (rb != null)
        {
            float objectY = other.transform.position.y;

            // Check if the vehicle is inside the water
            if (objectY < waterSurfaceY)
            {
                float depthFactor = Mathf.Clamp01((waterSurfaceY - objectY) / submersionDepth);
                float adjustedBuoyancy = buoyancyForce * depthFactor;

                // Apply a gentler upward force instead of instant floating
                rb.AddForce(Vector2.up * adjustedBuoyancy * 0.5f, ForceMode2D.Force);
            }

            // Apply realistic water drag to slow down movement
            rb.drag = waterDrag;
            rb.angularDrag = waterAngularDrag;

            // If vehicle reaches the bottom, prevent it from sinking further
            if (objectY <= waterBottomY)
            {
                rb.velocity = Vector2.zero;  // Stops movement
                rb.gravityScale = 0f;       // Disables further sinking
                rb.angularVelocity = 0f;    // Stops spinning
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Reset vehicle physics when it leaves the water
        Rigidbody2D rb = other.attachedRigidbody;
        if (rb != null)
        {
            rb.drag = 0f;
            rb.angularDrag = 0.05f;
        }
    }
}

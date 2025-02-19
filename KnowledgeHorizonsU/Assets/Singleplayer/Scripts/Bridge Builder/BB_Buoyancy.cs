using UnityEngine;

public class BB_Buoyancy : MonoBehaviour
{
    [Header("Buoyancy Settings")]
    [Tooltip("Upward force applied to objects in water.")]
    public float buoyancyForce = 10f;

    [Tooltip("Linear drag applied to objects in water.")]
    public float waterDrag = 2f;

    [Tooltip("Angular drag applied to objects in water.")]
    public float waterAngularDrag = 2f;

    // Optionally, you can store default drag values (if you want to revert them on exit)
    // This example assumes most objects have a default drag of 0 and angularDrag of 0.05.
    private const float defaultDrag = 0f;
    private const float defaultAngularDrag = 0.05f;

    // Called every fixed update cycle when a collider stays in the trigger.
    void OnTriggerStay2D(Collider2D other)
    {
        Rigidbody2D rb = other.attachedRigidbody;
        if (rb != null)
        {
            // Apply an upward force to simulate buoyancy.
            rb.AddForce(Vector2.up * buoyancyForce);

            // Increase drag to simulate the resistance of water.
            rb.drag = waterDrag;
            rb.angularDrag = waterAngularDrag;
        }
    }

    // When an object exits the buoyancy zone, reset its drag values.
    void OnTriggerExit2D(Collider2D other)
    {
        Rigidbody2D rb = other.attachedRigidbody;
        if (rb != null)
        {
            // Reset drag values to defaults (adjust these defaults if your objects have different settings).
            rb.drag = defaultDrag;
            rb.angularDrag = defaultAngularDrag;
        }
    }
}

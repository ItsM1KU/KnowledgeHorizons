using UnityEngine;

public class BB_PlankController : MonoBehaviour
{
    [Header("Plank Settings")]
    [Tooltip("Maximum weight this plank can support (Green:30, Yellow:20, Red:10).")]
    public float maxSupportedWeight = 10f;

    [Tooltip("Multiplier for the collapse force (for dramatic effect).")]
    public float collapseForceMultiplier = 1f;

    // Flag to ensure we collapse only once.
    private bool hasCollapsed = false;

    // Flag indicating if this plank is supported by the bridge support.
    private bool isSupported = false;

    /// <summary>
    /// Called by the support zone to set the support status.
    /// </summary>
    public void SetSupported(bool supported)
    {
        isSupported = supported;
    }

    /// <summary>
    /// Called by the sensor when a vehicle is detected.
    /// </summary>
    /// <param name="vehicleWeight">The weight of the vehicle driving over.</param>
    public void CheckVehicleWeight(float vehicleWeight)
    {
        // Only collapse if not already collapsed, not supported,
        // and the vehicle's weight exceeds this plank's capacity.
        if (!hasCollapsed && !isSupported && vehicleWeight > maxSupportedWeight)
        {
            CollapsePlank(vehicleWeight);
        }
    }

    /// <summary>
    /// Collapse this plank by removing its hinge joint and applying extra force.
    /// </summary>
    /// <param name="vehicleWeight">The weight of the vehicle (used to scale the collapse force).</param>
    void CollapsePlank(float vehicleWeight)
    {
        hasCollapsed = true;
        Debug.Log($"{gameObject.name} is collapsing! Vehicle weight: {vehicleWeight}, Capacity: {maxSupportedWeight}");

        // Remove the hinge joint (so the plank disconnects from its neighbors).
        HingeJoint2D hinge = GetComponent<HingeJoint2D>();
        if (hinge != null)
        {
            Destroy(hinge);
        }

        // Get the Rigidbody2D component.
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            // Switch from kinematic to dynamic.
            rb.isKinematic = false;
            rb.gravityScale = 1f;

            float extraForce = (vehicleWeight - maxSupportedWeight) * collapseForceMultiplier;
            Vector2 forceDirection = new Vector2(extraForce * 0.5f, -extraForce);
            rb.AddForce(forceDirection, ForceMode2D.Impulse);
        }
    }
}

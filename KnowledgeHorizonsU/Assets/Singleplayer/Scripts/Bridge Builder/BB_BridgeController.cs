using UnityEngine;

public class BridgeController : MonoBehaviour
{
    [Header("Bridge Settings")]
    public float maxSupportedWeight = 15f;  // Maximum weight allowed on the bridge before collapse
    private bool hasCollapsed = false;

    // Detect when a vehicle enters the bridge trigger zone.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Vehicle") && !hasCollapsed)
        {
            BB_VehicleController vc = collision.GetComponent<BB_VehicleController>();
            if (vc != null)
            {
                if (vc.weight > maxSupportedWeight)
                {
                    // The vehicle is too heavy—trigger a collapse.
                    CollapseBridge();
                }
                else
                {
                    Debug.Log("Vehicle passed safely.");
                }
            }
        }
    }

    private void CollapseBridge()
    {
        hasCollapsed = true;
        Debug.Log("Bridge Collapse Triggered!");

        // Iterate over each child plank in the bridge.
        foreach (Transform plank in transform)
        {
            // Ensure the plank has a Rigidbody2D so it reacts to physics.
            Rigidbody2D rb = plank.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // Enable gravity if not already enabled.
                rb.gravityScale = 1;  // Adjust as needed for realism
            }

            // Break the hinge joints so the planks separate.
            HingeJoint2D hinge = plank.GetComponent<HingeJoint2D>();
            if (hinge != null)
            {
                // Option 1: Directly destroy the hinge joint.
                Destroy(hinge);

                // Option 2: Alternatively, you could lower the joint's breakForce to force a break.
                // hinge.breakForce = 0.1f;
            }
        }

        // Optionally: add sound effects, particle effects, or additional force to enhance the collapse.
    }
}

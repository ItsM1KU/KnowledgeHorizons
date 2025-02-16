using UnityEngine;

public class BridgeController : MonoBehaviour
{
    [Header("Bridge Settings")]
    public float maxSupportedWeight = 15f;  // Maximum weight allowed
    private bool hasCollapsed = false;

    // Detect when a vehicle enters the bridge area
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Vehicle") && !hasCollapsed)
        {
            BB_VehicleController vc = collision.GetComponent<BB_VehicleController>();
            if (vc != null)
            {
                if (vc.weight > maxSupportedWeight)
                {
                    // Vehicle too heavy: collapse the bridge.
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

        // Example collapse: Enable physics on each plank to let them fall apart.
        foreach (Transform plank in transform)
        {
            Rigidbody2D rb = plank.GetComponent<Rigidbody2D>();
            if (rb == null)
            {
                rb = plank.gameObject.AddComponent<Rigidbody2D>();
            }
        }

        // You can also trigger animations, sounds, or other effects here.
    }
}

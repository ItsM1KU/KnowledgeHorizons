using UnityEngine;

public class BB_PlankStability : MonoBehaviour
{
    private Rigidbody2D rb;
    // Set your plank's mass value (should match what you set in Rigidbody2D for consistency)
    public float plankMass = 50f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // Start with planks not affected by gravity.
        rb.isKinematic = true;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the colliding object is tagged as "Vehicle"
        if (collision.gameObject.CompareTag("Vehicle"))
        {
            Rigidbody2D vehicleRb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (vehicleRb != null)
            {
                // If the vehicle's mass is greater than the plank's mass, allow the plank to fall.
                if (vehicleRb.mass > plankMass)
                {
                    rb.isKinematic = false; // Now gravity will affect this plank

                    // Optional: You can also break the joint connection to simulate collapse.
                    HingeJoint2D hinge = GetComponent<HingeJoint2D>();
                    if (hinge != null)
                    {
                        hinge.breakForce = 0f;
                    }
                }
            }
        }
    }
}


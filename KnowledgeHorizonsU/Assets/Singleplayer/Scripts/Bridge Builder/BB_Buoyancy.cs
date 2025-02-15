using UnityEngine;

public class BB_Buoyancy : MonoBehaviour
{
    public float buoyancyForce = 10f;

    void OnTriggerStay2D(Collider2D other)
    {
        Rigidbody2D rb = other.attachedRigidbody;
        if (rb != null)
        {
            // Apply an upward force to simulate buoyancy
            rb.AddForce(Vector2.up * buoyancyForce);
        }
    }
}

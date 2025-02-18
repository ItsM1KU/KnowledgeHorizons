using UnityEngine;

public class BB_VehicleExitTrigger : MonoBehaviour
{
    // This method is called when another collider enters this trigger.
    void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the colliding object is tagged as "Vehicle".
        if (collision.CompareTag("Vehicle"))
        {
            // Increment the pass count.
            if (BB_VehiclePassCounter.Instance != null)
            {
                BB_VehiclePassCounter.Instance.IncrementCount();
            }
            else
            {
                Debug.LogWarning("VehiclePassCounter.Instance is null!");
            }

            // Destroy the vehicle.
            Destroy(collision.gameObject);
        }
    }
}

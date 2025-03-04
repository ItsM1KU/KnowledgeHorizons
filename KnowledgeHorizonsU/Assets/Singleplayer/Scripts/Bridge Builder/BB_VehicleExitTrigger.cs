using UnityEngine;

public class BB_VehicleExitTrigger : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
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

            Destroy(collision.gameObject);
        }
    }
}

using UnityEngine;

public class BB_PlankSupportZone : MonoBehaviour
{
    // Automatically get the parent's plank controller.
    private BB_PlankController plankController;

    void Start()
    {
        plankController = GetComponentInParent<BB_PlankController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // When the bridge support enters, mark the plank as supported.
        if (collision.CompareTag("BridgeSupport"))
        {
            plankController.SetSupported(true);
            Debug.Log($"{gameObject.name} now supported.");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // When the bridge support leaves, mark the plank as no longer supported.
        if (collision.CompareTag("BridgeSupport"))
        {
            plankController.SetSupported(false);
            Debug.Log($"{gameObject.name} no longer supported.");
        }
    }
}

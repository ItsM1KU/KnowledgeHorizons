using UnityEngine;

public class BB_PlankSupportZone : MonoBehaviour
{
    private BB_PlankController plankController;

    void Start()
    {
        plankController = GetComponentInParent<BB_PlankController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("BridgeSupport"))
        {
            plankController.SetSupported(true);
            Debug.Log($"{gameObject.name} now supported.");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("BridgeSupport"))
        {
            plankController.SetSupported(false);
            Debug.Log($"{gameObject.name} no longer supported.");
        }
    }
}

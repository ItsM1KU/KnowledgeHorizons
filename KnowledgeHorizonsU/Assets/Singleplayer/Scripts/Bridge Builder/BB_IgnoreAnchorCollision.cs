using UnityEngine;

public class BB_IgnoreAnchorCollision : MonoBehaviour
{
    private Collider2D vehicleCollider;

    void Start()
    {
        vehicleCollider = GetComponent<Collider2D>();

        if (vehicleCollider == null)
        {
            Debug.LogError("No Collider2D found on the vehicle!");
            return;
        }

        // Continuously ensure the vehicle ignores anchor collisions
        InvokeRepeating(nameof(IgnoreAnchors), 0f, 0.1f);
    }

    void IgnoreAnchors()
    {
        GameObject[] anchors = GameObject.FindGameObjectsWithTag("Anchor");

        foreach (GameObject anchor in anchors)
        {
            Collider2D anchorCollider = anchor.GetComponent<Collider2D>();

            if (anchorCollider != null)
            {
                Physics2D.IgnoreCollision(vehicleCollider, anchorCollider, true);
            }
        }
    }
}

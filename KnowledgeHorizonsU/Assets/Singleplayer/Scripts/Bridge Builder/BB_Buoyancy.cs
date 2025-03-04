using UnityEngine;

public class BB_Buoyancy : MonoBehaviour
{
    [Header("Buoyancy Settings")]
    public float buoyancyForce = 10f;
    public float waterDrag = 5f;
    public float waterAngularDrag = 5f;
    public float submersionDepth = 1f;

    [Header("Water Boundaries")]
    public float waterSurfaceY = 0f;
    public float waterBottomY = -3f; 

    private void OnTriggerStay2D(Collider2D other)
    {
        Rigidbody2D rb = other.attachedRigidbody;
        if (rb != null)
        {
            float objectY = other.transform.position.y;

            if (other.gameObject.layer == LayerMask.NameToLayer("CollapsedPlank"))
            {
                rb.drag = 7f; 
                rb.angularDrag = 8f;
            }

            if (objectY < waterSurfaceY)
            {
                float depthFactor = Mathf.Clamp01((waterSurfaceY - objectY) / submersionDepth);
                float adjustedBuoyancy = buoyancyForce * depthFactor;
                rb.AddForce(Vector2.up * adjustedBuoyancy * 0.3f, ForceMode2D.Force);
            }

            // To make sure it doesn't sink too far
            if (objectY <= waterBottomY)
            {
                rb.velocity = Vector2.zero;
                rb.gravityScale = 0f;
                rb.angularVelocity = 0f;
            }
        }
    }
}

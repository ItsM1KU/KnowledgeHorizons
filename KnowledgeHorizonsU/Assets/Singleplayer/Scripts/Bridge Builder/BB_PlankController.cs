using UnityEngine;

public class BB_PlankController : MonoBehaviour
{
    [Header("Plank Settings")]
    [Tooltip("Maximum weight this plank can support (Green:30, Yellow:20, Red:10).")]
    public float maxSupportedWeight = 10f;

    [Tooltip("Multiplier for the collapse force (for dramatic effect).")]
    public float collapseForceMultiplier = 1f;

    [Tooltip("Maximum extra force applied to a collapsing plank, to avoid excessive movement with heavy vehicles.")]
    public float maxCollapseForce = 20f;

    private bool hasCollapsed = false;
    private bool isSupported = false;

    private Rigidbody2D rb;
    private Collider2D col;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }

    public void SetSupported(bool supported)
    {
        isSupported = supported;
    }

    public void CheckVehicleWeight(float vehicleWeight)
    {
        if (!hasCollapsed && !isSupported && vehicleWeight >= maxSupportedWeight)
        {
            CollapsePlank(vehicleWeight);
        }
    }

    void CollapsePlank(float vehicleWeight)
    {
        hasCollapsed = true;
        Debug.Log($"{gameObject.name} is collapsing! Vehicle weight: {vehicleWeight}, Capacity: {maxSupportedWeight}");

        HingeJoint2D hinge = GetComponent<HingeJoint2D>();
        if (hinge != null)
        {
            Destroy(hinge);
        }

        gameObject.layer = LayerMask.NameToLayer("CollapsedPlank");

        if (rb != null)
        {
            rb.isKinematic = false;
            rb.gravityScale = 1.5f;
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;

            float extraForce = (vehicleWeight - maxSupportedWeight) * collapseForceMultiplier;

            extraForce = Mathf.Clamp(extraForce, 0, maxCollapseForce);

            // Apply the downward force.
            rb.AddForce(Vector2.down * extraForce, ForceMode2D.Impulse);

            rb.drag = 4f;
            rb.angularDrag = 10f;

            IgnoreAdjacentPlanks(true);

            if (col != null)
            {
                Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("CollapsedPlank"),
                                               LayerMask.NameToLayer("BuoyancyZone"), true);
            }
            Invoke(nameof(EnableBuoyancyCollision), 0.2f);
        }
    }

    private void IgnoreAdjacentPlanks(bool ignore)
    {
        Collider2D[] adjacentPlanks = Physics2D.OverlapBoxAll(transform.position, new Vector2(2f, 2f), 0f);
        foreach (var adjacent in adjacentPlanks)
        {
            if (adjacent != col && adjacent.CompareTag("Plank"))
            {
                Physics2D.IgnoreCollision(col, adjacent, ignore);
            }
        }
    }

    private void EnableBuoyancyCollision()
    {
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("CollapsedPlank"),
                                       LayerMask.NameToLayer("BuoyancyZone"), false);
    }
}

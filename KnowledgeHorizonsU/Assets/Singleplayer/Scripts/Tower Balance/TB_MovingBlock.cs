using UnityEngine;

public class TB_MovingBlock : MonoBehaviour
{
    public float swingSpeed = 3f;
    public float maxAngle = 30f;

    // Add public property for placement status
    public bool IsPlaced { get; private set; }

    private Transform pivot;
    private float radius;
    private float angle;
    private Rigidbody2D rb;

    public void Initialize(Transform swingPivot, float swingRadius)
    {
        pivot = swingPivot;
        radius = swingRadius;
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;
        IsPlaced = false; // Initialize placement status
    }

    void Update()
    {
        if (!IsPlaced)
        {
            angle = maxAngle * Mathf.Sin(Time.time * swingSpeed);
            Vector2 offset = new Vector2(
                Mathf.Sin(angle * Mathf.Deg2Rad) * radius,
                -Mathf.Cos(angle * Mathf.Deg2Rad) * radius
            );
            transform.position = (Vector2)pivot.position + offset;
        }
    }

    public void Place()
    {
        if (IsPlaced) return;

        IsPlaced = true;
        rb.isKinematic = false;
        rb.velocity = Vector2.zero;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
}
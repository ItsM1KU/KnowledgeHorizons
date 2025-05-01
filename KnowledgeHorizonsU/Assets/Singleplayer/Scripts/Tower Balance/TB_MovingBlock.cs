using UnityEngine;

public class TB_MovingBlock : MonoBehaviour
{
    public float swingSpeed = 3f;
    public float maxAngle = 30f;

    private Transform pivot;
    private float radius;
    private bool isPlaced;
    private float angle;
    private Rigidbody2D rb;

    public void Initialize(Transform swingPivot, float swingRadius)
    {
        pivot = swingPivot;
        radius = swingRadius;
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;
    }

    void Update()
    {
        if (!isPlaced)
        {
            // Smooth pendulum motion
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
        if (isPlaced) return;

        isPlaced = true;
        rb.isKinematic = false;
        rb.velocity = Vector2.zero;

        // Freeze rotation for stable stacking
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
}
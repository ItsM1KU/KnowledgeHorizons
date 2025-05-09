using UnityEngine;

public class TB_MovingBlock : MonoBehaviour
{
    public float swingSpeed = 3f;
    public float maxAngle = 30f;
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
        IsPlaced = false;
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Only trigger game over if:
        // 1. This block was placed (not swinging)
        // 2. It hits the ground
        // 3. It's not the first block (handled in GameManager)
        if (IsPlaced && collision.gameObject.CompareTag("Ground"))
        {
            TB_GameManager.Instance.HandleFallingBlock(transform.position);
        }
    }
}
using UnityEngine;

public class TB_MovingBlock : MonoBehaviour
{
    public float swingAngle = 30f;
    public float swingSpeed = 1f;
    private float currentAngle = 0f;
    private bool isActive = true;
    private Transform pivotPoint;
    private float ropeLength;
    private Rigidbody2D rb;
    private bool isInitialized = false;

    public void Initialize(float length)
    {
        ropeLength = length;
        pivotPoint = GameObject.Find("SwingPivot").transform;
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;
        isInitialized = true;
    }

    void Update()
    {
        if (!isActive || !isInitialized) return;

        // Pendulum movement
        currentAngle = swingAngle * Mathf.Sin(Time.time * swingSpeed);

        // Calculate position based on pivot point
        float xOffset = Mathf.Sin(currentAngle * Mathf.Deg2Rad) * ropeLength;
        float yOffset = -Mathf.Cos(currentAngle * Mathf.Deg2Rad) * ropeLength;

        transform.position = pivotPoint.position + new Vector3(xOffset, yOffset, 0);
        transform.rotation = Quaternion.Euler(0, 0, currentAngle * 0.3f);
    }

    public void PlaceBlock()
    {
        if (!isInitialized) return;

        isActive = false;
        rb.isKinematic = false;
        transform.rotation = Quaternion.identity;

        // Apply slight downward force to ensure it falls
        rb.velocity = new Vector2(
            Mathf.Sin(currentAngle * Mathf.Deg2Rad) * swingSpeed * 2f, // Small horizontal momentum
            -2f // Downward force
        );
    }
}
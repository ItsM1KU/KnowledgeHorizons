using UnityEngine;

public class TB_MovingBlock : MonoBehaviour
{
    public float swingAngle = 30f; // Maximum angle in degrees
    public float swingSpeed = 1f;
    private float currentAngle = 0f;
    private bool isActive = true;
    private Transform pivotPoint; // Reference to the top pivot
    private float ropeLength; // Distance from pivot to block

    void Start()
    {
        // Find the pivot point (we'll create this in GameManager)
        pivotPoint = GameObject.Find("SwingPivot").transform;

        // Calculate initial rope length based on spawn position
        ropeLength = Vector3.Distance(pivotPoint.position, transform.position);
    }

    void Update()
    {
        if (!isActive) return;

        // Pendulum swing movement using sine wave
        currentAngle = swingAngle * Mathf.Sin(Time.time * swingSpeed);

        // Calculate new position based on pivot point
        float xOffset = Mathf.Sin(currentAngle * Mathf.Deg2Rad) * ropeLength;
        float yOffset = -Mathf.Cos(currentAngle * Mathf.Deg2Rad) * ropeLength;

        Vector3 newPosition = pivotPoint.position + new Vector3(xOffset, yOffset, 0);
        transform.position = newPosition;

        // Optional: Tilt the block slightly based on swing angle
        transform.rotation = Quaternion.Euler(0, 0, currentAngle * 0.3f);
    }

    public void PlaceBlock()
    {
        isActive = false;
        // Snap to flat rotation when placed
        transform.rotation = Quaternion.identity;
    }
}
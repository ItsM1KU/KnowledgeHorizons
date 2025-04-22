using UnityEngine;

public class TB_CameraFollow : MonoBehaviour
{
    public Transform target;             // Crate to follow
    public float yOffset = 2f;           // Vertical buffer
    public float smoothSpeed = 2f;

    private float fixedX;
    private float fixedZ;
    private float cameraHeightWorld;
    private float initialY;

    private bool shouldFollow = false;   // When to start moving up

    void Start()
    {
        fixedX = transform.position.x;
        fixedZ = transform.position.z;
        initialY = transform.position.y;

        // Calculate half the camera height in world units
        Camera cam = Camera.main;
        cameraHeightWorld = cam.orthographicSize;
    }

    void LateUpdate()
    {
        if (target == null) return;

        // Get the top visible Y position of the camera
        float visibleTopY = transform.position.y + cameraHeightWorld;

        // If the crate goes above the visible top, begin following
        if (!shouldFollow && target.position.y > visibleTopY - yOffset)
        {
            shouldFollow = true;
        }

        if (shouldFollow)
        {
            float targetY = target.position.y + yOffset;

            // Only move upward
            if (targetY > transform.position.y)
            {
                Vector3 desiredPosition = new Vector3(fixedX, targetY, fixedZ);
                transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
            }
        }
    }
}


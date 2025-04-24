using UnityEngine;

public class TB_CameraFollow : MonoBehaviour
{
    public Transform target;  // The block that the camera will follow (topmost block)
    public float smoothSpeed = 0.125f;  // How smooth the camera movement is
    public Vector3 offset;  // The offset between the camera and the target (block)

    void LateUpdate()
    {
        // Only move the camera if there is a valid target
        if (target != null)
        {
            // Calculate the desired position of the camera
            Vector3 desiredPosition = target.position + offset;

            // Smoothly interpolate to the desired position
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

            // Update the camera's position
            transform.position = smoothedPosition;
        }
    }
}

using UnityEngine;

public class TB_SmoothCameraFollow : MonoBehaviour
{
    private float targetY;
    private bool canFollow = false;
    public float smoothSpeed = 2f;
    public float cameraStepY = 3f; // how much the camera moves up each drop
    private float initialY;

    void Start()
    {
        initialY = transform.position.y;
        targetY = initialY;
    }

    public void MoveCameraUp()
    {
        canFollow = true;
        targetY += cameraStepY;
    }

    void LateUpdate()
    {
        if (canFollow)
        {
            Vector3 targetPosition = new Vector3(transform.position.x, targetY, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
        }
    }
}

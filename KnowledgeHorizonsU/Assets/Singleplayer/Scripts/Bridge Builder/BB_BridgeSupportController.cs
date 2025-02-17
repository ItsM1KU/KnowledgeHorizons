using UnityEngine;

public class BB_BridgeSupportController : MonoBehaviour
{
    public float moveSpeed = 5f; // Adjust as needed

    void Update()
    {
        float moveDirection = 0f;

        // Check for left/right input using A and D keys.
        if (Input.GetKey(KeyCode.A))
        {
            moveDirection = -1f;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            moveDirection = 1f;
        }

        // Move the support horizontally.
        transform.Translate(Vector3.right * moveDirection * moveSpeed * Time.deltaTime);
    }
}

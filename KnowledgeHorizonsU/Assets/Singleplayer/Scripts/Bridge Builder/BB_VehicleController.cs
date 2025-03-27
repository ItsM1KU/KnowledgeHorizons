using UnityEngine;

public class BB_VehicleController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 5f;

    [Header("Vehicle Properties")]
    public float weight = 10f;

    public bool IsGrounded { get; private set; }

    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    // This method is called when the object is no longer visible by any camera.
    void OnBecameInvisible()
    {
        // Optional: Check if the vehicle is offscreen to the right (assuming x > some value)
        if (transform.position.x > 25f)
        {
            // Call the pass counter to increment the count.
            if (BB_VehiclePassCounter.Instance != null)
            {
                BB_VehiclePassCounter.Instance.IncrementCount();
            }
        }

        Debug.Log(gameObject.name + " is now offscreen and will be destroyed.");
        Destroy(gameObject);
    }
}

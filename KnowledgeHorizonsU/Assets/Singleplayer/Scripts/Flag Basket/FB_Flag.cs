using UnityEngine;

public class FB_Flag : MonoBehaviour
{
    [Header("Falling Settings")]
    public float fallSpeed = 3f;

    public string FlagCountry { get; set; }

    void Update()
    {
        // Move the flag downward over time
        transform.Translate(Vector2.down * fallSpeed * Time.deltaTime);

        if (transform.position.y < -6f)
            Destroy(gameObject);
    }
}

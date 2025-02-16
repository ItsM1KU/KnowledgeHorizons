using UnityEngine;

public class BB_VehicleController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 5f;  

    [Header("Vehicle Properties")]
    public float weight = 10f;  

    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);

        if (transform.position.x > 20f)  
        {
            Destroy(gameObject);
        }
    }
}

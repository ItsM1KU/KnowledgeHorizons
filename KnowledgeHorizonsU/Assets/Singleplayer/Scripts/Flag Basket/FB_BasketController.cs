using UnityEngine;

public class FB_BasketController : MonoBehaviour
{
    public float moveSpeed = 10f;
    private float screenWidth;

    void Start()
    {
        screenWidth = Camera.main.orthographicSize * Camera.main.aspect;
    }

    void Update()
    {
        float moveInput = Input.GetAxis("Horizontal");
        Vector3 newPos = transform.position + Vector3.right * moveInput * moveSpeed * Time.deltaTime;
        newPos.x = Mathf.Clamp(newPos.x, -screenWidth, screenWidth);
        transform.position = newPos;
    }
}
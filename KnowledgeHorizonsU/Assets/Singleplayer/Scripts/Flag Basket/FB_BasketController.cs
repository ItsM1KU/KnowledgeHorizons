using UnityEngine;
using static Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics;

public class FB_BasketController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float xBoundary = 8f; 

    void Update()
    {
        float moveInput = Input.GetAxis("Horizontal");
        transform.Translate(Vector2.right * moveInput * moveSpeed * Time.deltaTime);

        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, -xBoundary, xBoundary);
        transform.position = pos;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Flag"))
        {
            FB_Flag flag = collision.gameObject.GetComponent<FB_Flag>();
            if (flag != null)
            {
                // To access the GameManager (ensure only one exists in the scene)
                FB_GameManager gm = FindObjectOfType<FB_GameManager>();
                if (gm != null)
                    gm.RegisterFlagCaught(flag.flagCountry);

                Destroy(collision.gameObject);
            }
        }
    }
}

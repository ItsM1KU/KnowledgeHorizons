using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ERT_FallingPlatform : MonoBehaviour
{

    [SerializeField] float timeBeforeFall;
    private Rigidbody2D rb;
    private bool isfalling = false;
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        originalPosition = transform.position;
        originalRotation = transform.rotation;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player" && !isfalling)
        {
            StartCoroutine(FallingBlock());
        }
    }

    public IEnumerator FallingBlock()
    {
        isfalling = true;
        yield return new WaitForSeconds(timeBeforeFall);
        rb.bodyType = RigidbodyType2D.Dynamic;
        yield return new WaitForSeconds(2f);
        rb.bodyType = RigidbodyType2D.Static;
        transform.position = originalPosition;
        transform.rotation = originalRotation;
        isfalling = false;
    }
}

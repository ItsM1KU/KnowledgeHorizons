using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TV_PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private Vector2 movement;

    [SerializeField] private float moveSpeed = 5f;

    private const string Horizontal = "Horizontal";
    private const string Vertical = "Vertical";
    private const string LastHorizontal = "LastHorizontal";
    private const string LastVertical = "LastVertical";

    [SerializeField] private LayerMask ArtifactLayer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        movement = TV_InputManager.Movement;
        

        rb.velocity = movement * moveSpeed;

        anim.SetFloat(Horizontal, movement.x);
        anim.SetFloat(Vertical, movement.y);

        if(movement != Vector2.zero)
        {
            anim.SetFloat(LastHorizontal, movement.x);
            anim.SetFloat(LastVertical, movement.y);
        }


        if(TV_InputManager.Instance.isInteracting)
        {
            InteractArtifact();
        }

        if (TV_InputManager.Instance.isClosing)
        {
            CloseArtifactUI();
        }

    }

    private void InteractArtifact()
    {
        var collider = Physics2D.OverlapCircle(transform.position, 0.5f, ArtifactLayer);
        if (collider != null) 
        {
            TV_GameManager.Instance.SetupArtifactUI(collider);
        }
    }

    private void CloseArtifactUI()
    {
        TV_GameManager.Instance.CloseArtifactUI();
    }
}

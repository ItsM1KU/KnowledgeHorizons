using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TV_InputManager : MonoBehaviour
{
    public static Vector2 Movement;

    private PlayerInput playerInput;
    private InputAction moveAction;

    public static TV_InputManager Instance;

    public bool isInteracting;
    public bool isClosing;

    private void Awake()
    {
        if (Instance == null) 
        {
            Instance = this;
        }

        playerInput = GetComponent<PlayerInput>();

        moveAction = playerInput.actions["Move"];
    }

    private void Update()
    {
        Movement = moveAction.ReadValue<Vector2>();


        isInteracting = playerInput.actions["Interact"].IsPressed();
        isClosing = playerInput.actions["Close"].IsPressed();
    }
}

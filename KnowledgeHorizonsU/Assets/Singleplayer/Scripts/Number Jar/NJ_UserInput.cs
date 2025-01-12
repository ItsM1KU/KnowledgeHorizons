using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NJ_UserInput : MonoBehaviour
{
    public static PlayerInput playerInput;

    public static Vector2 MoveInput { get; set; }

    public static bool isThrowPressed { get; set; }

    private InputAction moveAction;
    private InputAction throwAction;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        moveAction = playerInput.actions["Move"];
        throwAction = playerInput.actions["Throw"];
    }

    private void Update()
    {
        MoveInput = moveAction.ReadValue<Vector2>();

        isThrowPressed = throwAction.WasPressedThisFrame();
    }
}

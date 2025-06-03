using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{ public static  GameInput Instance { get; private set; }
    private PlayerInputAction playerInputAction;
    public event EventHandler OnPlayerAttack;
    private void Awake()
    {
        Instance = this;
        playerInputAction = new PlayerInputAction();
        playerInputAction.Enable();
        playerInputAction.Combat.Attack.started += PlayerAttack_started;
    }
    private void PlayerAttack_started(InputAction.CallbackContext obj)
    {
          OnPlayerAttack?.Invoke(this, EventArgs.Empty); 
       
    }
    public Vector2 GetMovementVector()
    {
        Vector2 inputVector = playerInputAction.Player.Move.ReadValue<Vector2>();
        return inputVector;
    }
    public Vector3 GetMousePosition()
    {
        Vector3 mousePos = Mouse.current.position.ReadValue();
        return mousePos;
    }
}


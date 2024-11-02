using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Controls.Controls;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "New Input Reader", menuName = "Input/Input Reader")]
public class InputReader : ScriptableObject, IPlayerActions
{
    public event Action<Vector2> MoveEvent;
    public event Action<bool> PrimaryFireEvent;

    public Vector2 AimPosition { get; private set; } 
    
    private Controls.Controls controls;

    private void OnEnable()
    {
        if (controls == null)
        {
            controls = new Controls.Controls();
            controls.Player.SetCallbacks(this);
        }

        controls.Player.Enable();
    }
    
    public void OnMove(InputAction.CallbackContext context)
    {
       MoveEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            PrimaryFireEvent?.Invoke(true);    
        }else if (context.canceled)
        {
            PrimaryFireEvent?.Invoke(false);
        }

        
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        AimPosition = context.ReadValue<Vector2>();
    }
}

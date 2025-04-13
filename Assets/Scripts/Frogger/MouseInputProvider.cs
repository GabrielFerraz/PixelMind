using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System;

public class MouseInputProvider : MonoBehaviour
{

    public event Action Clicked;
    public Vector2 worldPosition {get; private set;}

    private void OnLook(InputValue value) {
        // Convert screen position to world position    
        if (value.Get<Vector2>() != Vector2.zero) {
            worldPosition = Camera.main.ScreenToWorldPoint(value.Get<Vector2>());
        }
    }

    private void OnAction(InputValue value) {
        Clicked?.Invoke();
    }

    private void OnTest(InputValue value) {
        Debug.Log("Test input: " + Camera.main.ScreenToWorldPoint(value.Get<Vector2>()));
    }
}

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class ClickHandler : MonoBehaviour
{
    [SerializeField]
    private UnityEvent _clicked;
    private BoxCollider2D _collider;
    private int position;
    private MouseInputProvider _mouse;

    private void Awake()
    {
        _mouse = FindFirstObjectByType<MouseInputProvider>();
        _mouse.Clicked += MouseOnClicked;
        _collider = GetComponent<BoxCollider2D>();
        
    }

    private void MouseOnClicked() {
        Debug.Log("Mouse Position: " + _mouse.worldPosition);
        Debug.Log("Collider Position: " + _collider.bounds.center);
        if(_collider.OverlapPoint(_mouse.worldPosition)) {
            Debug.Log("Mouse clicked on: " + gameObject.name);
            _clicked?.Invoke();
        }
    }
}

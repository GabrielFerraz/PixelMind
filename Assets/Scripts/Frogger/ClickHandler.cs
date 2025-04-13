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
        Debug.Log("World Position" + _mouse.worldPosition);
        Debug.Log("Mouse clicked on: " + _collider.bounds);
        if(_collider.bounds.Contains(_mouse.worldPosition)) {
            _clicked?.Invoke();
        }
    }
}

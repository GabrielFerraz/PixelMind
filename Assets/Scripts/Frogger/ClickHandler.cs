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
        Debug.Log("coll" + _collider.bounds.Contains(_mouse.worldPosition));
            
        Debug.Log("World Position" + _mouse.worldPosition);
        Debug.Log("Mouse clicked on: " + _collider.bounds);
        var pos =  new Vector3(_mouse.worldPosition.x, _mouse.worldPosition.y, 0.03f);
        if(_collider.bounds.Contains(pos)) {
            _clicked?.Invoke();
        }
    }
}

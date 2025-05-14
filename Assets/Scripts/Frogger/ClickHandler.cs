using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class ClickHandler : MonoBehaviour
{
    [SerializeField]
    private UnityEvent _clicked;
    private BoxCollider2D _collider;
    public int position;
    private MouseInputProvider _mouse;
    public InputAction tapAction;

    private void Awake()
    {
        _mouse = FindFirstObjectByType<MouseInputProvider>();
        _mouse.Clicked += MouseOnClicked;
        _collider = GetComponent<BoxCollider2D>();
        tapAction.Enable();
        tapAction.performed += OnTapPerformed;
    }

    private void MouseOnClicked() {
        if(_collider.OverlapPoint(_mouse.worldPosition)) {
            _clicked?.Invoke();
        }
    }

    public void OnTapPerformed(InputAction.CallbackContext context)
    {
        // Obtém a posição do toque na tela
        Vector2 screenPosition = Pointer.current.position.ReadValue();

        // Converte para posição no mundo
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, Camera.main.nearClipPlane));

        if(_collider.OverlapPoint(worldPosition)) {
            _clicked?.Invoke();
        }
    }
}

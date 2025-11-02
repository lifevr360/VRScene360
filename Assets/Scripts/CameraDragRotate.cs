using UnityEngine;
using UnityEngine.InputSystem;

public class CameraDragRotate : MonoBehaviour
{
    [Header("Rotation Settings")]
    public float rotationSpeed = 0.2f;  // Adjust for sensitivity

    private Vector2 _lastPointerPosition;
    private bool _isDragging;

    void Update()
    {
        HandleMouseInput();
        HandleTouchInput();
    }

    void HandleMouseInput()
    {
        if (Mouse.current == null)
            return;

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            _isDragging = true;
            _lastPointerPosition = Mouse.current.position.ReadValue();
        }
        else if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            _isDragging = false;
        }

        if (_isDragging)
        {
            Vector2 currentPosition = Mouse.current.position.ReadValue();
            Vector2 delta = currentPosition - _lastPointerPosition;
            _lastPointerPosition = currentPosition;

            RotateCamera(delta);
        }
    }

    void HandleTouchInput()
    {
        if (Touchscreen.current == null)
            return;

        var touch = Touchscreen.current.primaryTouch;

        if (touch.press.wasPressedThisFrame)
        {
            _isDragging = true;
            _lastPointerPosition = touch.position.ReadValue();
        }
        else if (touch.press.wasReleasedThisFrame)
        {
            _isDragging = false;
        }

        if (_isDragging && touch.press.isPressed)
        {
            Vector2 currentPosition = touch.position.ReadValue();
            Vector2 delta = currentPosition - _lastPointerPosition;
            _lastPointerPosition = currentPosition;

            RotateCamera(delta);
        }
    }

    void RotateCamera(Vector2 delta)
    {
        float rotationX = delta.y * rotationSpeed;
        float rotationY = -delta.x * rotationSpeed;

        transform.eulerAngles += new Vector3(rotationX, rotationY, 0);
    }
}

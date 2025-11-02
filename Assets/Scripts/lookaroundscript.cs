using UnityEngine;
using UnityEngine.InputSystem;

public class CameraLookController : MonoBehaviour
{
    [Header("Settings")]
    public float lookSpeed = 2.0f;
    private Vector2 rotation = Vector2.zero;
    private bool isDragging = false;
    private Vector2 lastPointerPosition;

    void Update()
    {
        HandlePointerInput();
    }

    void HandlePointerInput()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        // --- Mouse input ---
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            isDragging = true;
            lastPointerPosition = Mouse.current.position.ReadValue();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        if (Mouse.current.leftButton.isPressed && isDragging)
        {
            Vector2 currentPosition = Mouse.current.position.ReadValue();
            Vector2 delta = currentPosition - lastPointerPosition;
            lastPointerPosition = currentPosition;

            ApplyRotation(delta.x, delta.y);
        }

        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            isDragging = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

#elif UNITY_ANDROID || UNITY_IOS
        // --- Touch input ---
        if (Touchscreen.current != null && Touchscreen.current.touches.Count > 0)
        {
            var touch = Touchscreen.current.touches[0];

            if (touch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Began)
            {
                isDragging = true;
                lastPointerPosition = touch.position.ReadValue();
            }

            if (isDragging && touch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Moved)
            {
                Vector2 currentPosition = touch.position.ReadValue();
                Vector2 delta = currentPosition - lastPointerPosition;
                lastPointerPosition = currentPosition;

                ApplyRotation(delta.x * 0.1f, delta.y * 0.1f);
            }

            if (touch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Ended ||
                touch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Canceled)
            {
                isDragging = false;
            }
        }
#endif
    }

    void ApplyRotation(float deltaX, float deltaY)
    {
        rotation.x += deltaX * lookSpeed * Time.deltaTime;
        rotation.y -= deltaY * lookSpeed * Time.deltaTime;
        rotation.y = Mathf.Clamp(rotation.y, -90f, 90f);

        transform.rotation = Quaternion.Euler(rotation.y, rotation.x, 0f);
    }
}

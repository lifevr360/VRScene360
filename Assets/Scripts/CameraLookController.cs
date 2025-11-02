using UnityEngine;

public class CameraLookController : MonoBehaviour
{
    public float lookSpeed = 2.0f;
    private Vector2 rotation = Vector2.zero;
    private Vector2 touchOrigin;

    void Update()
    {
        HandleMouseInput();
        HandleTouchInput();
    }

    void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        if (Input.GetMouseButton(0))
        {
            rotation.x += Input.GetAxis("Mouse X") * lookSpeed;
            rotation.y -= Input.GetAxis("Mouse Y") * lookSpeed;
            rotation.y = Mathf.Clamp(rotation.y, -90, 90);

            transform.eulerAngles = new Vector2(rotation.y, rotation.x);
        }

        if (Input.GetMouseButtonUp(0))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    void HandleTouchInput()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                touchOrigin = touch.position;
                return;
            }

            if (touch.phase == TouchPhase.Moved)
            {
                Vector2 touchDelta = touch.deltaPosition;

                rotation.x += touchDelta.x * lookSpeed * 0.1f;
                rotation.y -= touchDelta.y * lookSpeed * 0.1f;
                rotation.y = Mathf.Clamp(rotation.y, -90, 90);

                transform.eulerAngles = new Vector2(rotation.y, rotation.x);
            }
        }
    }
}

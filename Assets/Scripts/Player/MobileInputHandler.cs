using UnityEngine;

public class MobileInputHandler : MonoBehaviour
{
    [Header("Mobile Input Settings")]
    [SerializeField] private float joystickDeadzone = 0.1f;
    [SerializeField] private float lookSensitivity = 1f;

    private Vector2 movementInput = Vector2.zero;
    private Vector2 lookInput = Vector2.zero;
    private bool isSprinting = false;

    private Touch[] touches;
    private Vector2 joystickCenter;
    private Vector2 lookTouchStart;

    private void Update()
    {
        #if UNITY_IOS || UNITY_ANDROID
        HandleTouchInput();
        #else
        HandleMouseInput();
        #endif
    }

    private void HandleTouchInput()
    {
        touches = Input.touches;
        movementInput = Vector2.zero;
        lookInput = Vector2.zero;

        foreach (Touch touch in touches)
        {
            float screenMidpoint = Screen.width / 2f;

            // Left side: Movement joystick
            if (touch.position.x < screenMidpoint)
            {
                HandleMovementJoystick(touch);
            }
            // Right side: Look/Camera
            else
            {
                HandleLookInput(touch);
            }
        }
    }

    private void HandleMovementJoystick(Touch touch)
    {
        if (touch.phase == TouchPhase.Began)
        {
            joystickCenter = touch.position;
        }
        else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
        {
            Vector2 offset = touch.position - joystickCenter;
            float maxDistance = 80f; // Joystick radius
            offset = Vector2.ClampMagnitude(offset, maxDistance);

            movementInput = offset / maxDistance;

            // Apply deadzone
            if (movementInput.magnitude < joystickDeadzone)
            {
                movementInput = Vector2.zero;
            }
        }
        else if (touch.phase == TouchPhase.Ended)
        {
            movementInput = Vector2.zero;
        }
    }

    private void HandleLookInput(Touch touch)
    {
        if (touch.phase == TouchPhase.Began)
        {
            lookTouchStart = touch.position;
        }
        else if (touch.phase == TouchPhase.Moved)
        {
            Vector2 delta = touch.position - lookTouchStart;
            lookInput = delta * lookSensitivity * 0.01f;
            lookTouchStart = touch.position;
        }
        else if (touch.phase == TouchPhase.Ended)
        {
            lookInput = Vector2.zero;
        }
    }

    private void HandleMouseInput()
    {
        // Desktop controls for testing
        movementInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        lookInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        isSprinting = Input.GetKey(KeyCode.LeftShift);
    }

    public Vector2 GetMovementInput() => movementInput;
    public Vector2 GetLookInput() => lookInput;
    public bool IsSprinting() => isSprinting;
}

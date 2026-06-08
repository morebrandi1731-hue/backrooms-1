using UnityEngine;

public class MobileInputHandler : MonoBehaviour
{
    [Header("Movement Input")]
    [SerializeField] private RectTransform leftJoystickArea;
    [SerializeField] private RectTransform leftJoystickHandle;
    [SerializeField] private float joystickDeadzone = 0.1f;

    [Header("Look Input")]
    [SerializeField] private RectTransform rightTouchArea;
    [SerializeField] private float lookSensitivity = 1f;

    [Header("Action Buttons")]
    [SerializeField] private UnityEngine.UI.Button pickupButton;
    [SerializeField] private UnityEngine.UI.Button consumeButton;
    [SerializeField] private UnityEngine.UI.Button toggleFlashlightButton;
    [SerializeField] private UnityEngine.UI.Button sprintButton;

    [Header("Desktop Settings")]
    [SerializeField] private float desktopMouseSensitivity = 2f;

    private Vector2 movementInput = Vector2.zero;
    private Vector2 lookInput = Vector2.zero;
    private Vector2 leftJoystickStartPos;
    private Vector2 rightTouchStartPos;
    private bool isSprinting = false;
    private bool isPickupPressed = false;
    private bool isConsumePressed = false;
    private bool isToggleFlashlightPressed = false;

    private int leftJoystickFingerId = -1;
    private int rightTouchFingerId = -1;
    private bool isDesktop = false;

    private void Start()
    {
        #if UNITY_IOS || UNITY_ANDROID
        isDesktop = false;
        #else
        isDesktop = true;
        #endif

        if (!isDesktop)
        {
            // Setup button listeners for mobile
            if (pickupButton != null)
            {
                pickupButton.onClick.AddListener(() => isPickupPressed = true);
            }
            if (consumeButton != null)
            {
                consumeButton.onClick.AddListener(() => isConsumePressed = true);
            }
            if (toggleFlashlightButton != null)
            {
                toggleFlashlightButton.onClick.AddListener(() => isToggleFlashlightPressed = true);
            }
            if (sprintButton != null)
            {
                sprintButton.onPointerDown.AddListener(x => isSprinting = true);
                sprintButton.onPointerUp.AddListener(x => isSprinting = false);
            }
        }
    }

    private void Update()
    {
        if (isDesktop)
        {
            HandleDesktopInput();
        }
        else
        {
            HandleTouchInput();
        }
        ResetFrameInputs();
    }

    private void HandleDesktopInput()
    {
        // WASD movement
        movementInput = new Vector2(
            Input.GetAxis("Horizontal"),
            Input.GetAxis("Vertical")
        );

        // Mouse look
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            lookInput = new Vector2(
                Input.GetAxis("Mouse X"),
                Input.GetAxis("Mouse Y")
            ) * desktopMouseSensitivity;
        }

        // Sprint with Left Shift
        isSprinting = Input.GetKey(KeyCode.LeftShift);

        // Pickup with E
        if (Input.GetKeyDown(KeyCode.E))
        {
            isPickupPressed = true;
        }

        // Consume with F
        if (Input.GetKeyDown(KeyCode.F))
        {
            isConsumePressed = true;
        }

        // Toggle flashlight with Spacebar
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isToggleFlashlightPressed = true;
        }

        // Toggle cursor lock with Escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = Cursor.lockState == CursorLockMode.Locked ? CursorLockMode.None : CursorLockMode.Locked;
        }
    }

    private void HandleTouchInput()
    {
        // Handle multiple touches
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch touch = Input.GetTouch(i);

            if (touch.phase == TouchPhase.Began)
            {
                HandleTouchDown(touch);
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                HandleTouchMove(touch);
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                HandleTouchUp(touch);
            }
        }
    }

    private void HandleTouchDown(Touch touch)
    {
        // Check if touch is on left joystick area
        if (leftJoystickArea != null && RectTransformUtility.RectTransformPointIsValid(leftJoystickArea, touch.position))
        {
            leftJoystickFingerId = touch.fingerId;
            leftJoystickStartPos = touch.position;
        }
        // Check if touch is on right side for camera look
        else if (rightTouchArea != null && RectTransformUtility.RectTransformPointIsValid(rightTouchArea, touch.position))
        {
            rightTouchFingerId = touch.fingerId;
            rightTouchStartPos = touch.position;
        }
    }

    private void HandleTouchMove(Touch touch)
    {
        // Update left joystick
        if (touch.fingerId == leftJoystickFingerId)
        {
            Vector2 direction = (touch.position - leftJoystickStartPos).normalized;
            float magnitude = Mathf.Min(Vector2.Distance(touch.position, leftJoystickStartPos) / 75f, 1f);
            
            if (magnitude < joystickDeadzone)
                magnitude = 0;

            movementInput = direction * magnitude;

            // Update joystick handle position
            if (leftJoystickHandle != null)
            {
                leftJoystickHandle.anchoredPosition = direction * magnitude * 75f;
            }
        }

        // Update camera look
        if (touch.fingerId == rightTouchFingerId)
        {
            Vector2 delta = touch.position - rightTouchStartPos;
            lookInput = delta * lookSensitivity * 0.01f;
            rightTouchStartPos = touch.position;
        }
    }

    private void HandleTouchUp(Touch touch)
    {
        if (touch.fingerId == leftJoystickFingerId)
        {
            leftJoystickFingerId = -1;
            movementInput = Vector2.zero;
            if (leftJoystickHandle != null)
                leftJoystickHandle.anchoredPosition = Vector2.zero;
        }

        if (touch.fingerId == rightTouchFingerId)
        {
            rightTouchFingerId = -1;
            lookInput = Vector2.zero;
        }
    }

    private void ResetFrameInputs()
    {
        isPickupPressed = false;
        isConsumePressed = false;
        isToggleFlashlightPressed = false;
    }

    // Getters
    public Vector2 GetMovementInput() => movementInput;
    public Vector2 GetLookInput() => lookInput;
    public bool IsSprinting() => isSprinting;
    public bool IsPickupPressed() => isPickupPressed;
    public bool IsConsumePressed() => isConsumePressed;
    public bool IsToggleFlashlightPressed() => isToggleFlashlightPressed;
    public bool IsDesktop() => isDesktop;
}

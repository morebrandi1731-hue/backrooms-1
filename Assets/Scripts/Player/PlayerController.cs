using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float sprintSpeed = 8f;
    [SerializeField] private Rigidbody rb;

    [Header("Camera")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float lookSensitivity = 2f;
    [SerializeField] private float maxLookAngle = 90f;

    [Header("Mobile Input")]
    [SerializeField] private MobileInputHandler inputHandler;

    [Header("Equipment")]
    [SerializeField] private HazmatModel hazmatModel;
    [SerializeField] private ItemManager itemManager;

    private Vector3 moveDirection = Vector3.zero;
    private float currentSpeed;
    private float cameraYaw = 0f;
    private float cameraPitch = 0f;

    private void Start()
    {
        if (rb == null) rb = GetComponent<Rigidbody>();
        if (mainCamera == null) mainCamera = Camera.main;
        if (inputHandler == null) inputHandler = GetComponent<MobileInputHandler>();
        if (hazmatModel == null) hazmatModel = GetComponent<HazmatModel>();
        if (itemManager == null) itemManager = GetComponent<ItemManager>();

        // Lock cursor on desktop
        #if !UNITY_IOS && !UNITY_ANDROID
        Cursor.lockState = CursorLockMode.Locked;
        #endif
    }

    private void Update()
    {
        HandleInput();
        HandleCameraRotation();
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleInput()
    {
        // Get input from mobile input handler
        Vector2 moveInput = inputHandler.GetMovementInput();
        bool isSprinting = inputHandler.IsSprinting();

        currentSpeed = isSprinting ? sprintSpeed : moveSpeed;

        // Calculate movement direction relative to camera facing
        Vector3 forward = mainCamera.transform.forward;
        Vector3 right = mainCamera.transform.right;

        // Flatten forward direction (no vertical movement from forward input)
        forward.y = 0f;
        forward.Normalize();

        moveDirection = (forward * moveInput.y + right * moveInput.x).normalized * currentSpeed;
    }

    private void HandleMovement()
    {
        // Apply horizontal movement
        Vector3 velocity = rb.velocity;
        velocity.x = moveDirection.x;
        velocity.z = moveDirection.z;
        rb.velocity = velocity;
    }

    private void HandleCameraRotation()
    {
        Vector2 lookInput = inputHandler.GetLookInput();

        cameraYaw += lookInput.x * lookSensitivity;
        cameraPitch -= lookInput.y * lookSensitivity;
        cameraPitch = Mathf.Clamp(cameraPitch, -maxLookAngle, maxLookAngle);

        // Apply rotation to camera
        mainCamera.transform.localEulerAngles = new Vector3(cameraPitch, 0f, 0f);
        transform.eulerAngles = new Vector3(0f, cameraYaw, 0f);
    }
}

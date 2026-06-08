using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TouchCanvas : MonoBehaviour
{
    [SerializeField] private MobileInputHandler inputHandler;
    [SerializeField] private Image leftJoystickBackground;
    [SerializeField] private Image leftJoystickHandle;
    [SerializeField] private TextMeshProUGUI debugText;

    private Vector2 joystickCenter;

    private void Update()
    {
        #if UNITY_IOS || UNITY_ANDROID
        UpdateJoystickVisuals();
        UpdateDebugText();
        #endif
    }

    private void UpdateJoystickVisuals()
    {
        if (leftJoystickHandle == null) return;

        Vector2 movementInput = inputHandler.GetMovementInput();
        Vector2 handlePosition = joystickCenter + (movementInput * 50f);
        leftJoystickHandle.rectTransform.anchoredPosition = handlePosition;
    }

    private void UpdateDebugText()
    {
        if (debugText == null) return;

        Vector2 movement = inputHandler.GetMovementInput();
        Vector2 look = inputHandler.GetLookInput();
        debugText.text = $"Movement: ({movement.x:F2}, {movement.y:F2})\nLook: ({look.x:F2}, {look.y:F2})";
    }
}

using UnityEngine;
using UnityEngine.UI;

public class MobileUIManager : MonoBehaviour
{
    [Header("Joystick Setup")]
    [SerializeField] private RectTransform leftJoystickBase;
    [SerializeField] private RectTransform leftJoystickHandle;
    [SerializeField] private RectTransform rightTouchArea;

    [Header("Buttons")]
    [SerializeField] private Button pickupButton;
    [SerializeField] private Button consumeButton;
    [SerializeField] private Button toggleFlashlightButton;
    [SerializeField] private Button sprintButton;

    [Header("Status Displays")]
    [SerializeField] private Text sanityText;
    [SerializeField] private Image sanityBar;
    [SerializeField] private Text itemDisplayText;

    [SerializeField] private SanitySystem sanitySystem;
    [SerializeField] private ItemManager itemManager;

    private void Start()
    {
        if (sanitySystem == null)
            sanitySystem = FindObjectOfType<SanitySystem>();
        if (itemManager == null)
            itemManager = FindObjectOfType<ItemManager>();

        SetupButtons();
    }

    private void Update()
    {
        UpdateStatusDisplays();
    }

    private void SetupButtons()
    {
        if (pickupButton != null)
            pickupButton.onClick.AddListener(OnPickupPressed);
        if (consumeButton != null)
            consumeButton.onClick.AddListener(OnConsumePressed);
        if (toggleFlashlightButton != null)
            toggleFlashlightButton.onClick.AddListener(OnToggleFlashlightPressed);
    }

    private void UpdateStatusDisplays()
    {
        if (sanitySystem != null)
        {
            float sanityPercent = sanitySystem.GetSanityPercent();
            
            if (sanityText != null)
                sanityText.text = $"Sanity: {sanityPercent:F0}%";

            if (sanityBar != null)
                sanityBar.fillAmount = sanityPercent / 100f;
        }

        if (itemManager != null)
        {
            string itemText = "No Item";
            if (itemManager.GetEquippedFlashlight() != null)
                itemText = "Holding: Flashlight";
            else if (itemManager.GetEquippedAlmondWater() != null)
                itemText = "Holding: Almond Water";

            if (itemDisplayText != null)
                itemDisplayText.text = itemText;
        }
    }

    private void OnPickupPressed() { }
    private void OnConsumePressed() { }
    private void OnToggleFlashlightPressed() { }
}

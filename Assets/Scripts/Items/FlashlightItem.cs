using UnityEngine;

public class FlashlightItem : MonoBehaviour
{
    [SerializeField] private Light flashlightLight;
    [SerializeField] private bool isEquipped = false;
    [SerializeField] private bool isActive = true;

    private Collider itemCollider;
    private MeshRenderer[] meshRenderers;
    private Transform originalParent;
    private Vector3 originalPosition;
    private Quaternion originalRotation;

    private void Start()
    {
        itemCollider = GetComponent<Collider>();
        meshRenderers = GetComponentsInChildren<MeshRenderer>();
        
        // Create light source
        GameObject lightGO = new GameObject("Light");
        lightGO.transform.SetParent(transform);
        lightGO.transform.localPosition = new Vector3(0, 0, 0.65f);
        
        flashlightLight = lightGO.AddComponent<Light>();
        flashlightLight.type = LightType.Spot;
        flashlightLight.intensity = 2f;
        flashlightLight.range = 50f;
        flashlightLight.spotAngle = 40f;
        flashlightLight.color = new Color(1f, 1f, 0.9f);
    }

    private void Update()
    {
        if (isEquipped && Input.GetKeyDown(KeyCode.Space))
        {
            ToggleFlashlight();
        }
    }

    public void Equip(Transform hand)
    {
        isEquipped = true;
        originalParent = transform.parent;
        originalPosition = transform.position;
        originalRotation = transform.rotation;

        transform.SetParent(hand);
        transform.localPosition = new Vector3(0, -0.1f, 0.15f);
        transform.localRotation = Quaternion.Euler(-15f, 0, 0);

        // Disable physics when equipped
        if (itemCollider != null)
            itemCollider.enabled = false;

        flashlightLight.enabled = isActive;
    }

    public void Unequip(Vector3 dropPosition)
    {
        isEquipped = false;
        transform.SetParent(originalParent);
        transform.position = dropPosition;
        transform.rotation = Quaternion.identity;

        // Enable physics when dropped
        if (itemCollider != null)
            itemCollider.enabled = true;

        flashlightLight.enabled = false;
    }

    public void ToggleFlashlight()
    {
        isActive = !isActive;
        if (flashlightLight != null)
            flashlightLight.enabled = isActive;
    }

    public Light GetLight() => flashlightLight;
    public bool IsEquipped() => isEquipped;
    public bool IsActive() => isActive;
}

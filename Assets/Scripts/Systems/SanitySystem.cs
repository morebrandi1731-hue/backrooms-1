using UnityEngine;

public class SanitySystem : MonoBehaviour
{
    [Header("Sanity Settings")]
    [SerializeField] private float maxSanity = 100f;
    [SerializeField] private float currentSanity = 100f;
    [SerializeField] private float sanityDrainRate = 5f; // Per second in darkness
    [SerializeField] private float lightRestoreRate = 2f; // Per second near flashlight
    [SerializeField] private float hallucinationThreshold = 30f; // Below this, hallucinations start

    [Header("Darkness Detection")]
    [SerializeField] private float darkThreshold = 0.3f; // Light intensity below this
    [SerializeField] private Light detectedLight;

    [Header("Effects")]
    [SerializeField] private HallucinationController hallucinationController;
    [SerializeField] private AudioSource audioSource;

    private bool isInDarkness = true;
    private bool isHallucinating = false;

    private void Start()
    {
        if (hallucinationController == null)
            hallucinationController = GetComponent<HallucinationController>();
    }

    private void Update()
    {
        DetectLighting();
        UpdateSanity();
        UpdateHallucinations();
    }

    private void DetectLighting()
    {
        // Find nearby lights
        Light[] lights = FindObjectsOfType<Light>();
        float maxIntensity = 0f;

        foreach (Light light in lights)
        {
            float distance = Vector3.Distance(transform.position, light.transform.position);
            if (distance < light.range)
            {
                float intensity = light.intensity / (1 + distance * 0.1f);
                maxIntensity = Mathf.Max(maxIntensity, intensity);
            }
        }

        isInDarkness = maxIntensity < darkThreshold;
    }

    private void UpdateSanity()
    {
        if (isInDarkness)
        {
            currentSanity -= sanityDrainRate * Time.deltaTime;
        }
        else
        {
            currentSanity += lightRestoreRate * Time.deltaTime;
        }

        currentSanity = Mathf.Clamp(currentSanity, 0, maxSanity);
    }

    private void UpdateHallucinations()
    {
        bool shouldHallucinate = currentSanity < hallucinationThreshold;

        if (shouldHallucinate && !isHallucinating)
        {
            isHallucinating = true;
            if (hallucinationController != null)
                hallucinationController.StartHallucinations(currentSanity / hallucinationThreshold);
        }
        else if (!shouldHallucinate && isHallucinating)
        {
            isHallucinating = false;
            if (hallucinationController != null)
                hallucinationController.StopHallucinations();
        }
        else if (isHallucinating && hallucinationController != null)
        {
            hallucinationController.UpdateIntensity(currentSanity / hallucinationThreshold);
        }
    }

    public void RestoreSanity(float amount)
    {
        currentSanity = Mathf.Min(currentSanity + amount, maxSanity);
    }

    public float GetSanity() => currentSanity;
    public float GetSanityPercent() => (currentSanity / maxSanity) * 100f;
    public bool IsHallucinating() => isHallucinating;
    public bool IsInDarkness() => isInDarkness;
}

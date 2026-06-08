using UnityEngine;

public class HallucinationController : MonoBehaviour
{
    [Header("Visual Effects")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float maxDistortion = 0.05f;
    [SerializeField] private float maxColorShift = 0.3f;
    [SerializeField] private float maxScreenShake = 0.15f;

    [Header("Audio Effects")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip hallucinationSound;
    [SerializeField] private float maxAudioPitch = 1.5f;

    private bool isActive = false;
    private float hallucinationIntensity = 0f;
    private float screenShakeAmount = 0f;
    private Material screenEffectMaterial;
    private Vector3 originalCameraPos;

    private void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        originalCameraPos = mainCamera.transform.localPosition;
        screenEffectMaterial = new Material(Shader.Find("Standard"));
    }

    private void Update()
    {
        if (isActive)
        {
            UpdateVisualEffects();
            UpdateAudioEffects();
        }
        else
        {
            // Reset camera position
            if (mainCamera != null)
                mainCamera.transform.localPosition = originalCameraPos;
        }
    }

    public void StartHallucinations(float intensity)
    {
        isActive = true;
        hallucinationIntensity = Mathf.Clamp01(intensity);
    }

    public void UpdateIntensity(float intensity)
    {
        hallucinationIntensity = Mathf.Clamp01(intensity);
    }

    public void StopHallucinations()
    {
        isActive = false;
        hallucinationIntensity = 0f;
        screenShakeAmount = 0f;
    }

    private void UpdateVisualEffects()
    {
        if (mainCamera == null) return;

        // Screen shake - scales with sanity loss
        screenShakeAmount = Mathf.Sin(Time.time * 8f) * hallucinationIntensity * maxScreenShake;
        Vector3 shakeOffset = new Vector3(
            Random.Range(-screenShakeAmount, screenShakeAmount),
            Random.Range(-screenShakeAmount, screenShakeAmount),
            0
        );
        mainCamera.transform.localPosition = originalCameraPos + shakeOffset;

        // Color distortion
        mainCamera.backgroundColor = Color.Lerp(
            new Color(0.3f, 0.3f, 0.25f),
            new Color(0.15f, 0.1f, 0.05f),
            hallucinationIntensity
        );
    }

    private void UpdateAudioEffects()
    {
        if (audioSource == null) return;

        // Pitch shift for audio
        audioSource.pitch = Mathf.Lerp(1f, maxAudioPitch, hallucinationIntensity * 0.5f);
    }
}

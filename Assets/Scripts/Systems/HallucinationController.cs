using UnityEngine;

public class HallucinationController : MonoBehaviour
{
    [Header("Visual Effects")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float maxDistortion = 0.05f;
    [SerializeField] private float maxColorShift = 0.3f;

    [Header("Audio Effects")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip hallucinationSound;
    [SerializeField] private float maxAudioPitch = 1.5f;

    private bool isActive = false;
    private float hallucinationIntensity = 0f;
    private float screenShakeAmount = 0f;
    private Material screenEffectMaterial;

    private void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        // Create screen effect material
        screenEffectMaterial = new Material(Shader.Find("Standard"));
    }

    private void Update()
    {
        if (isActive)
        {
            UpdateVisualEffects();
            UpdateAudioEffects();
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

        // Screen shake
        screenShakeAmount = Mathf.Sin(Time.time * 5f) * hallucinationIntensity * 0.1f;
        mainCamera.transform.localPosition += new Vector3(
            Random.Range(-screenShakeAmount, screenShakeAmount),
            Random.Range(-screenShakeAmount, screenShakeAmount),
            0
        );

        // Color distortion (subtle)
        mainCamera.backgroundColor = Color.Lerp(
            new Color(0.3f, 0.3f, 0.25f),
            new Color(0.2f, 0.2f, 0.15f),
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

using UnityEngine;
using System.Collections.Generic;

public class HallwayGenerator : MonoBehaviour
{
    [Header("Hallway Dimensions")]
    [SerializeField] private float hallwayWidth = 8f;
    [SerializeField] private float hallwayHeight = 3.5f;
    [SerializeField] private float segmentLength = 10f;

    [Header("Generation Settings")]
    [SerializeField] private int segmentsToGenerate = 5;
    [SerializeField] private int segmentsToKeep = 8;
    [SerializeField] private float generationDistance = 50f;

    [Header("Visual Variation")]
    [SerializeField] private bool varyWallColors = true;
    [SerializeField] private float colorVariation = 0.1f;
    [SerializeField] private bool varyLighting = true;
    [SerializeField] private bool addWallImperfections = true;

    [Header("Layout Rules")]
    [SerializeField] private bool allowTurns = false;
    [SerializeField] private float turnChance = 0.05f;
    [SerializeField] private float maxTurnAngle = 15f;
    [SerializeField] private bool addDoors = false;
    [SerializeField] private float doorChance = 0.15f;

    [Header("Materials")]
    [SerializeField] private Material yellowWallMaterial;
    [SerializeField] private Material floorMaterial;
    [SerializeField] private Material ceilingMaterial;

    private Queue<HallwaySegment> activeSegments = new Queue<HallwaySegment>();
    private float currentZPosition = 0f;
    private float currentRotation = 0f;
    private int segmentIndex = 0;
    private Transform playerTransform;

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
        
        // Generate initial segments
        for (int i = 0; i < segmentsToGenerate; i++)
        {
            GenerateSegment();
        }
    }

    private void Update()
    {
        if (playerTransform == null) return;

        // Generate new segments when player approaches
        float playerZ = playerTransform.position.z;
        if (playerZ > currentZPosition - generationDistance)
        {
            GenerateSegment();
        }

        // Remove old segments
        while (activeSegments.Count > segmentsToKeep)
        {
            HallwaySegment oldSegment = activeSegments.Dequeue();
            Destroy(oldSegment.gameObject);
        }
    }

    private void GenerateSegment()
    {
        GameObject segmentGO = new GameObject($"HallwaySegment_{segmentIndex++}");
        segmentGO.transform.position = new Vector3(0, 0, currentZPosition);
        segmentGO.transform.eulerAngles = new Vector3(0, currentRotation, 0);

        HallwaySegment segment = segmentGO.AddComponent<HallwaySegment>();
        segment.Initialize(
            hallwayWidth,
            hallwayHeight,
            segmentLength,
            yellowWallMaterial,
            floorMaterial,
            ceilingMaterial,
            varyWallColors,
            colorVariation,
            varyLighting,
            addWallImperfections,
            addDoors,
            doorChance
        );

        activeSegments.Enqueue(segment);
        currentZPosition += segmentLength;

        // Handle turns
        if (allowTurns && Random.value < turnChance)
        {
            float turnAmount = Random.Range(-maxTurnAngle, maxTurnAngle);
            currentRotation += turnAmount;
        }
    }

    public float GetHallwayWidth() => hallwayWidth;
    public float GetHallwayHeight() => hallwayHeight;
}

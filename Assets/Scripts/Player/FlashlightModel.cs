using UnityEngine;

public class FlashlightModel : MonoBehaviour
{
    [SerializeField] private Material metalMaterial;
    [SerializeField] private Material glassMaterial;
    [SerializeField] private Light flashlightLight;
    [SerializeField] private bool isActive = true;

    private void Start()
    {
        BuildFlashlight();
    }

    private void Update()
    {
        // Toggle flashlight with spacebar (for testing)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ToggleFlashlight();
        }
    }

    private void BuildFlashlight()
    {
        // Flashlight barrel (cylinder)
        GameObject barrel = CreateCylinder("Barrel", new Vector3(0, 0, 0.3f), new Vector3(0.08f, 0.08f, 0.3f));
        barrel.transform.SetParent(transform);
        if (metalMaterial != null)
            barrel.GetComponent<MeshRenderer>().material = metalMaterial;

        // Flashlight head (wider front)
        GameObject head = CreateCylinder("Head", new Vector3(0, 0, 0.55f), new Vector3(0.12f, 0.12f, 0.08f));
        head.transform.SetParent(transform);
        if (metalMaterial != null)
            head.GetComponent<MeshRenderer>().material = metalMaterial;

        // Flashlight lens (glass sphere)
        GameObject lens = CreateSphere("Lens", new Vector3(0, 0, 0.62f), 0.065f);
        lens.transform.SetParent(transform);
        if (glassMaterial != null)
        {
            Material lensMat = new Material(glassMaterial);
            lensMat.color = new Color(1f, 1f, 0.8f, 0.7f); // Yellowish transparent
            lens.GetComponent<MeshRenderer>().material = lensMat;
        }

        // Handle (grip)
        GameObject handle = CreateCube("Handle", new Vector3(-0.08f, -0.06f, 0.15f), new Vector3(0.06f, 0.12f, 0.15f));
        handle.transform.SetParent(transform);
        if (metalMaterial != null)
            handle.GetComponent<MeshRenderer>().material = metalMaterial;

        // Battery compartment (darker section)
        GameObject battery = CreateCylinder("Battery", new Vector3(0, 0, -0.1f), new Vector3(0.075f, 0.075f, 0.15f));
        battery.transform.SetParent(transform);
        Material batteryMat = new Material(metalMaterial != null ? metalMaterial : new Material(Shader.Find("Standard")));
        batteryMat.color = new Color(0.3f, 0.3f, 0.3f); // Dark gray
        battery.GetComponent<MeshRenderer>().material = batteryMat;

        // Create light source
        GameObject lightGO = new GameObject("Light");
        lightGO.transform.SetParent(transform);
        lightGO.transform.localPosition = new Vector3(0, 0, 0.65f);
        
        flashlightLight = lightGO.AddComponent<Light>();
        flashlightLight.type = LightType.Spot;
        flashlightLight.intensity = 2f;
        flashlightLight.range = 50f;
        flashlightLight.spotAngle = 40f;
        flashlightLight.color = new Color(1f, 1f, 0.9f); // Warm white light
    }

    private GameObject CreateCylinder(string name, Vector3 position, Vector3 scale)
    {
        GameObject obj = new GameObject(name);
        obj.transform.localPosition = position;
        obj.transform.localScale = scale;

        MeshRenderer renderer = obj.AddComponent<MeshRenderer>();
        MeshFilter filter = obj.AddComponent<MeshFilter>();
        CapsuleCollider collider = obj.AddComponent<CapsuleCollider>();

        Mesh mesh = CreateCylinderMesh();
        filter.mesh = mesh;
        renderer.material = new Material(Shader.Find("Standard"));

        return obj;
    }

    private GameObject CreateSphere(string name, Vector3 position, float radius)
    {
        GameObject obj = new GameObject(name);
        obj.transform.localPosition = position;
        obj.transform.localScale = Vector3.one * radius * 2;

        MeshRenderer renderer = obj.AddComponent<MeshRenderer>();
        MeshFilter filter = obj.AddComponent<MeshFilter>();
        SphereCollider collider = obj.AddComponent<SphereCollider>();

        Mesh mesh = CreateSphereMesh();
        filter.mesh = mesh;
        renderer.material = new Material(Shader.Find("Standard"));

        return obj;
    }

    private GameObject CreateCube(string name, Vector3 position, Vector3 scale)
    {
        GameObject obj = new GameObject(name);
        obj.transform.localPosition = position;
        obj.transform.localScale = scale;

        MeshRenderer renderer = obj.AddComponent<MeshRenderer>();
        MeshFilter filter = obj.AddComponent<MeshFilter>();
        BoxCollider collider = obj.AddComponent<BoxCollider>();

        Mesh mesh = CreateCubeMesh();
        filter.mesh = mesh;
        renderer.material = new Material(Shader.Find("Standard"));

        return obj;
    }

    private Mesh CreateCylinderMesh()
    {
        Mesh mesh = new Mesh();
        int segments = 16;
        Vector3[] vertices = new Vector3[segments * 2 + 2];
        
        // Create cylinder vertices
        for (int i = 0; i < segments; i++)
        {
            float angle = (i / (float)segments) * Mathf.PI * 2;
            float x = Mathf.Cos(angle);
            float y = Mathf.Sin(angle);
            
            vertices[i] = new Vector3(x, y, -0.5f);
            vertices[i + segments] = new Vector3(x, y, 0.5f);
        }
        
        vertices[segments * 2] = new Vector3(0, 0, -0.5f); // Bottom center
        vertices[segments * 2 + 1] = new Vector3(0, 0, 0.5f); // Top center
        
        mesh.vertices = vertices;
        
        // Create triangles
        int[] triangles = new int[segments * 12];
        int triIndex = 0;
        
        for (int i = 0; i < segments; i++)
        {
            int next = (i + 1) % segments;
            
            // Side faces
            triangles[triIndex++] = i;
            triangles[triIndex++] = next;
            triangles[triIndex++] = i + segments;
            
            triangles[triIndex++] = next;
            triangles[triIndex++] = next + segments;
            triangles[triIndex++] = i + segments;
            
            // Bottom cap
            triangles[triIndex++] = segments * 2;
            triangles[triIndex++] = i;
            triangles[triIndex++] = next;
            
            // Top cap
            triangles[triIndex++] = segments * 2 + 1;
            triangles[triIndex++] = next + segments;
            triangles[triIndex++] = i + segments;
        }
        
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        return mesh;
    }

    private Mesh CreateSphereMesh()
    {
        Mesh mesh = new Mesh();
        float t = (1f + Mathf.Sqrt(5f)) / 2f;
        
        Vector3[] vertices = new Vector3[12]
        {
            new Vector3(-1, t, -1).normalized,
            new Vector3(1, t, -1).normalized,
            new Vector3(-1, t, 1).normalized,
            new Vector3(1, t, 1).normalized,
            new Vector3(-1, -t, -1).normalized,
            new Vector3(1, -t, -1).normalized,
            new Vector3(-1, -t, 1).normalized,
            new Vector3(1, -t, 1).normalized,
            new Vector3(-t, -1, 1).normalized,
            new Vector3(t, -1, 1).normalized,
            new Vector3(-t, 1, 1).normalized,
            new Vector3(t, 1, 1).normalized
        };

        int[] triangles = new int[60]
        {
            0,11,5, 0,5,1, 0,1,7, 0,7,10,
            0,10,11, 1,5,9, 5,11,4, 11,10,2,
            10,7,6, 7,1,8, 3,9,4, 3,4,2,
            3,2,6, 3,6,8, 3,8,9, 4,9,5,
            2,4,11, 6,2,10, 8,6,7, 9,8,1
        };

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        return mesh;
    }

    private Mesh CreateCubeMesh()
    {
        Mesh mesh = new Mesh();
        Vector3[] vertices = new Vector3[8]
        {
            new Vector3(-0.5f, -0.5f, -0.5f),
            new Vector3(0.5f, -0.5f, -0.5f),
            new Vector3(-0.5f, 0.5f, -0.5f),
            new Vector3(0.5f, 0.5f, -0.5f),
            new Vector3(-0.5f, -0.5f, 0.5f),
            new Vector3(0.5f, -0.5f, 0.5f),
            new Vector3(-0.5f, 0.5f, 0.5f),
            new Vector3(0.5f, 0.5f, 0.5f)
        };

        int[] triangles = new int[36]
        {
            0,2,1, 1,2,3, 4,5,6, 5,7,6,
            0,4,2, 2,4,6, 1,3,5, 3,7,5,
            0,1,4, 1,5,4, 2,6,3, 3,6,7
        };

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        return mesh;
    }

    public void ToggleFlashlight()
    {
        isActive = !isActive;
        if (flashlightLight != null)
            flashlightLight.enabled = isActive;
    }

    public Light GetFlashlightLight() => flashlightLight;
}

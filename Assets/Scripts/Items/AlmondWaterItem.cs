using UnityEngine;

public class AlmondWaterItem : MonoBehaviour
{
    [SerializeField] private float sanityRestore = 40f;
    [SerializeField] private Material glassBottleMaterial;
    [SerializeField] private Material liquidMaterial;

    private Collider itemCollider;
    private bool isPickedUp = false;

    private void Start()
    {
        BuildAlmondWater();
        itemCollider = GetComponent<Collider>();
    }

    private void BuildAlmondWater()
    {
        // Bottle body (cylinder)
        GameObject bottleBody = CreateCylinder("BottleBody", new Vector3(0, 0, 0), new Vector3(0.08f, 0.08f, 0.15f));
        bottleBody.transform.SetParent(transform);
        Material bottleMat = new Material(glassBottleMaterial != null ? glassBottleMaterial : Shader.Find("Standard"));
        bottleMat.color = new Color(0.7f, 0.9f, 0.8f); // Pale blue-green
        bottleBody.GetComponent<MeshRenderer>().material = bottleMat;

        // Liquid inside
        GameObject liquid = CreateCylinder("Liquid", new Vector3(0, 0, -0.02f), new Vector3(0.07f, 0.07f, 0.12f));
        liquid.transform.SetParent(transform);
        Material liquidMat = new Material(liquidMaterial != null ? liquidMaterial : Shader.Find("Standard"));
        liquidMat.color = new Color(0.8f, 1f, 0.6f); // Light almond/yellowish liquid
        liquid.GetComponent<MeshRenderer>().material = liquidMat;

        // Bottle cap
        GameObject cap = CreateCube("Cap", new Vector3(0, 0.08f, 0), new Vector3(0.1f, 0.03f, 0.1f));
        cap.transform.SetParent(transform);
        Material capMat = new Material(Shader.Find("Standard"));
        capMat.color = new Color(0.2f, 0.2f, 0.2f); // Dark gray cap
        cap.GetComponent<MeshRenderer>().material = capMat;

        // Label
        GameObject label = CreateCube("Label", new Vector3(0, 0.02f, 0.041f), new Vector3(0.06f, 0.08f, 0.001f));
        label.transform.SetParent(transform);
        Material labelMat = new Material(Shader.Find("Standard"));
        labelMat.color = new Color(1f, 1f, 1f); // White label
        label.GetComponent<MeshRenderer>().material = labelMat;
    }

    private GameObject CreateCylinder(string name, Vector3 position, Vector3 scale)
    {
        GameObject obj = new GameObject(name);
        obj.transform.localPosition = position;
        obj.transform.localScale = scale;

        MeshRenderer renderer = obj.AddComponent<MeshRenderer>();
        MeshFilter filter = obj.AddComponent<MeshFilter>();
        CapsuleCollider collider = obj.AddComponent<CapsuleCollider>();
        collider.enabled = false; // Disable individual colliders

        Mesh mesh = CreateCylinderMesh();
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
        collider.enabled = false; // Disable individual colliders

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
        
        for (int i = 0; i < segments; i++)
        {
            float angle = (i / (float)segments) * Mathf.PI * 2;
            float x = Mathf.Cos(angle);
            float y = Mathf.Sin(angle);
            
            vertices[i] = new Vector3(x, y, -0.5f);
            vertices[i + segments] = new Vector3(x, y, 0.5f);
        }
        
        vertices[segments * 2] = new Vector3(0, 0, -0.5f);
        vertices[segments * 2 + 1] = new Vector3(0, 0, 0.5f);
        
        mesh.vertices = vertices;
        
        int[] triangles = new int[segments * 12];
        int triIndex = 0;
        
        for (int i = 0; i < segments; i++)
        {
            int next = (i + 1) % segments;
            triangles[triIndex++] = i;
            triangles[triIndex++] = next;
            triangles[triIndex++] = i + segments;
            triangles[triIndex++] = next;
            triangles[triIndex++] = next + segments;
            triangles[triIndex++] = i + segments;
            triangles[triIndex++] = segments * 2;
            triangles[triIndex++] = i;
            triangles[triIndex++] = next;
            triangles[triIndex++] = segments * 2 + 1;
            triangles[triIndex++] = next + segments;
            triangles[triIndex++] = i + segments;
        }
        
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

    public void Consume(SanitySystem sanitySystem)
    {
        if (sanitySystem != null)
        {
            sanitySystem.RestoreSanity(sanityRestore);
        }
        Destroy(gameObject);
    }

    public float GetSanityRestore() => sanityRestore;
}

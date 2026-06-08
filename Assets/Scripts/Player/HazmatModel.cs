using UnityEngine;

public class HazmatModel : MonoBehaviour
{
    [SerializeField] private Transform bodyTransform;
    [SerializeField] private Material hazmatMaterial;

    private void Start()
    {
        BuildHazmatSuit();
    }

    private void BuildHazmatSuit()
    {
        // Create body (main torso)
        GameObject body = CreateCapsule("Body", new Vector3(0, 0.5f, 0), new Vector3(0.35f, 0.6f, 0.35f));
        body.transform.SetParent(transform);

        // Create head
        GameObject head = CreateSphere("Head", new Vector3(0, 1.3f, 0), 0.25f);
        head.transform.SetParent(transform);

        // Create helmet visor (yellow tint glass)
        GameObject helmet = CreateSphere("Helmet", new Vector3(0, 1.3f, 0.15f), 0.15f);
        Material helmetMat = new Material(hazmatMaterial);
        helmetMat.color = new Color(1f, 1f, 0.7f, 0.6f); // Yellowish transparent
        helmet.GetComponent<MeshRenderer>().material = helmetMat;
        helmet.transform.SetParent(transform);

        // Left arm
        GameObject leftArm = CreateCapsule("LeftArm", new Vector3(-0.35f, 0.6f, 0), new Vector3(0.12f, 0.5f, 0.12f));
        leftArm.transform.SetParent(transform);

        // Right arm
        GameObject rightArm = CreateCapsule("RightArm", new Vector3(0.35f, 0.6f, 0), new Vector3(0.12f, 0.5f, 0.12f));
        rightArm.transform.SetParent(transform);

        // Left leg
        GameObject leftLeg = CreateCapsule("LeftLeg", new Vector3(-0.15f, 0.15f, 0), new Vector3(0.12f, 0.4f, 0.12f));
        leftLeg.transform.SetParent(transform);

        // Right leg
        GameObject rightLeg = CreateCapsule("RightLeg", new Vector3(0.15f, 0.15f, 0), new Vector3(0.12f, 0.4f, 0.12f));
        rightLeg.transform.SetParent(transform);

        // Yellow hazmat suit details (stripes)
        GameObject stripe1 = CreateCube("Stripe1", new Vector3(0, 0.7f, 0.2f), new Vector3(0.4f, 0.1f, 0.05f));
        Material stripeMat = new Material(hazmatMaterial);
        stripeMat.color = new Color(1f, 0.9f, 0f); // Bright yellow
        stripe1.GetComponent<MeshRenderer>().material = stripeMat;
        stripe1.transform.SetParent(transform);
    }

    private GameObject CreateCapsule(string name, Vector3 position, Vector3 scale)
    {
        GameObject obj = new GameObject(name);
        obj.transform.localPosition = position;
        obj.transform.localScale = scale;

        MeshRenderer renderer = obj.AddComponent<MeshRenderer>();
        MeshFilter filter = obj.AddComponent<MeshFilter>();
        BoxCollider collider = obj.AddComponent<BoxCollider>();

        // Create simple capsule mesh (approximated as elongated cube)
        Mesh mesh = CreateCapsuleMesh(1, 1);
        filter.mesh = mesh;
        renderer.material = hazmatMaterial != null ? hazmatMaterial : new Material(Shader.Find("Standard"));

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
        renderer.material = hazmatMaterial != null ? hazmatMaterial : new Material(Shader.Find("Standard"));

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
        renderer.material = hazmatMaterial != null ? hazmatMaterial : new Material(Shader.Find("Standard"));

        return obj;
    }

    private Mesh CreateCapsuleMesh(float height, float radius)
    {
        Mesh mesh = new Mesh();
        // Simplified capsule - elongated box
        Vector3[] vertices = new Vector3[8]
        {
            new Vector3(-radius, -height/2, -radius),
            new Vector3(radius, -height/2, -radius),
            new Vector3(-radius, height/2, -radius),
            new Vector3(radius, height/2, -radius),
            new Vector3(-radius, -height/2, radius),
            new Vector3(radius, -height/2, radius),
            new Vector3(-radius, height/2, radius),
            new Vector3(radius, height/2, radius)
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

    private Mesh CreateSphereMesh()
    {
        Mesh mesh = new Mesh();
        // Simplified sphere using icosahedron approximation
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
}
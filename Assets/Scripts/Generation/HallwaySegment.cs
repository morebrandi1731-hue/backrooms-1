using UnityEngine;

public class HallwaySegment : MonoBehaviour
{
    private float width;
    private float height;
    private float length;
    private Material wallMaterial;
    private Material floorMaterial;
    private Material ceilingMaterial;

    public void Initialize(
        float w, float h, float l,
        Material wall, Material floor, Material ceiling,
        bool varyColors, float colorVar,
        bool varyLighting, bool imperfections,
        bool addDoors, float doorChance)
    {
        width = w;
        height = h;
        length = l;
        wallMaterial = wall;
        floorMaterial = floor;
        ceilingMaterial = ceiling;

        BuildSegment(varyColors, colorVar, varyLighting, imperfections, addDoors, doorChance);
    }

    private void BuildSegment(bool varyColors, float colorVar, bool varyLighting, bool imperfections, bool addDoors, float doorChance)
    {
        // Floor
        CreatePlane("Floor", new Vector3(0, 0, 0), new Vector3(width, 1, length), floorMaterial);

        // Ceiling
        CreatePlane("Ceiling", new Vector3(0, height, 0), new Vector3(width, 1, length), ceilingMaterial);

        // Left Wall
        CreateWall("LeftWall", new Vector3(-width / 2, height / 2, 0), new Vector3(1, height, length), wallMaterial, varyColors, colorVar);

        // Right Wall
        CreateWall("RightWall", new Vector3(width / 2, height / 2, 0), new Vector3(1, height, length), wallMaterial, varyColors, colorVar);

        // Front Wall (at end of segment)
        CreateWall("FrontWall", new Vector3(0, height / 2, length / 2), new Vector3(width, height, 1), wallMaterial, varyColors, colorVar);

        // Add fluorescent lights
        if (varyLighting)
        {
            AddLighting();
        }

        // Add doors
        if (addDoors && Random.value < doorChance)
        {
            AddDoor();
        }
    }

    private void CreatePlane(string name, Vector3 position, Vector3 scale, Material material)
    {
        GameObject plane = new GameObject(name);
        plane.transform.SetParent(transform);
        plane.transform.localPosition = position;

        MeshRenderer renderer = plane.AddComponent<MeshRenderer>();
        MeshFilter filter = plane.AddComponent<MeshFilter>();
        BoxCollider collider = plane.AddComponent<BoxCollider>();

        // Create mesh
        Mesh mesh = new Mesh();
        Vector3[] vertices = new Vector3[4]
        {
            new Vector3(-scale.x / 2, 0, -scale.z / 2),
            new Vector3(scale.x / 2, 0, -scale.z / 2),
            new Vector3(-scale.x / 2, 0, scale.z / 2),
            new Vector3(scale.x / 2, 0, scale.z / 2)
        };

        int[] triangles = new int[6] { 0, 2, 1, 1, 2, 3 };
        Vector2[] uvs = new Vector2[4]
        {
            Vector2.zero,
            Vector2.right,
            Vector2.up,
            Vector2.one
        };

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals();

        filter.mesh = mesh;
        renderer.material = material;
        collider.size = scale;
    }

    private void CreateWall(string name, Vector3 position, Vector3 scale, Material material, bool varyColors, float colorVar)
    {
        GameObject wall = new GameObject(name);
        wall.transform.SetParent(transform);
        wall.transform.localPosition = position;

        MeshRenderer renderer = wall.AddComponent<MeshRenderer>();
        MeshFilter filter = wall.AddComponent<MeshFilter>();
        BoxCollider collider = wall.AddComponent<BoxCollider>();

        // Create simple quad mesh
        Mesh mesh = new Mesh();
        Vector3[] vertices = new Vector3[4]
        {
            new Vector3(-scale.x / 2, -scale.y / 2, -scale.z / 2),
            new Vector3(scale.x / 2, -scale.y / 2, -scale.z / 2),
            new Vector3(-scale.x / 2, scale.y / 2, -scale.z / 2),
            new Vector3(scale.x / 2, scale.y / 2, -scale.z / 2)
        };

        int[] triangles = new int[6] { 0, 2, 1, 1, 2, 3 };
        Vector2[] uvs = new Vector2[4]
        {
            Vector2.zero,
            Vector2.right,
            Vector2.up,
            Vector2.one
        };

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals();

        filter.mesh = mesh;

        // Apply material with slight color variation
        if (varyColors)
        {
            Material variedMat = new Material(material);
            Color baseColor = variedMat.color;
            float variation = Random.Range(-colorVar, colorVar);
            variedMat.color = baseColor * (1 + variation);
            renderer.material = variedMat;
        }
        else
        {
            renderer.material = material;
        }

        collider.size = scale;
    }

    private void AddLighting()
    {
        // Create fluorescent light panels
        for (int i = 0; i < 2; i++)
        {
            float xPos = (i == 0) ? -width / 3 : width / 3;
            
            GameObject lightGO = new GameObject($"Light_{i}");
            lightGO.transform.SetParent(transform);
            lightGO.transform.localPosition = new Vector3(xPos, height - 0.2f, length / 2);

            Light light = lightGO.AddComponent<Light>();
            light.type = LightType.Point;
            light.intensity = Random.Range(0.8f, 1.2f);
            light.range = 15f;
            light.color = new Color(1f, 0.95f, 0.85f); // Warm yellow-white
        }
    }

    private void AddDoor()
    {
        // Add a door frame on the left or right wall
        bool leftSide = Random.value > 0.5f;
        float xPos = leftSide ? -width / 2 : width / 2;

        GameObject doorFrame = new GameObject("DoorFrame");
        doorFrame.transform.SetParent(transform);
        doorFrame.transform.localPosition = new Vector3(xPos, height * 0.6f, Random.Range(0, length));

        // Simple door opening (just a visual marker)
        MeshRenderer renderer = doorFrame.AddComponent<MeshRenderer>();
        MeshFilter filter = doorFrame.AddComponent<MeshFilter>();n        Mesh mesh = CreateQuadMesh(2f, 2.2f);
        filter.mesh = mesh;
        renderer.material = wallMaterial;
    }

    private Mesh CreateQuadMesh(float w, float h)
    {
        Mesh mesh = new Mesh();
        Vector3[] vertices = new Vector3[4]
        {
            new Vector3(-w / 2, -h / 2, 0),
            new Vector3(w / 2, -h / 2, 0),
            new Vector3(-w / 2, h / 2, 0),
            new Vector3(w / 2, h / 2, 0)
        };
        int[] triangles = new int[6] { 0, 2, 1, 1, 2, 3 };
        Vector2[] uvs = new Vector2[4] { Vector2.zero, Vector2.right, Vector2.up, Vector2.one };
        
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals();
        return mesh;
    }
}

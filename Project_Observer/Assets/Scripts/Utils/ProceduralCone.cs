using Unity.Properties;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public class ProceduralCone : MonoBehaviour
{
    #region Variables

    public float height = 5f; // Height of the cone
    public float radius = 2f; // Radius of the base of the cone
    public int segments = 20; // Number of segments around the base

    private MeshFilter meshFilter;
    private MeshCollider meshCollider;

    #endregion

    #region Start Functions

    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshCollider = GetComponent<MeshCollider>();
    }

    void Start()
    {
        Mesh coneMesh = CreateConeMesh();

        meshFilter.mesh = coneMesh;
        meshCollider.sharedMesh = coneMesh; // Use the same mesh for the collider
    }

    #endregion

    #region Update Functions

    // private void Update()
    // {
    //     transform.position = transform.parent.position; // Position the cone at the player's position
    //     transform.rotation = transform.parent.rotation; // Align the cone with the player's rotation (look direction)
    // }

    #endregion

    #region Base Functions

    Mesh CreateConeMesh()
    {
        Mesh mesh = new Mesh();

        // Vertices
        Vector3[] vertices = new Vector3[segments + 2]; // +2: one for the tip, one for the center of the base
        vertices[0] = transform.localPosition; // Tip of the cone at the player's position (origin)

        // Calculate base position (in player's forward direction)
        Vector3 baseCenter = transform.localPosition + Vector3.forward * height;

        // Create a right vector that is perpendicular to the forward vector
        Vector3 right = Vector3.Cross(Vector3.forward, Vector3.up).normalized;

        // If the forward direction is roughly aligned with the up direction, adjust the right vector
        if (Vector3.Dot(Vector3.forward, Vector3.up) > 0.9f) // Adjust based on a threshold
        {
            right = Vector3.Cross(Vector3.forward, Vector3.right).normalized;
        }

        // Create the base vertices in a circular arrangement around the base center
        float angleStep = 360f / segments;

        for (int i = 0; i < segments; i++)
        {
            float angle = Mathf.Deg2Rad * i * angleStep;
            float x = Mathf.Cos(angle) * radius; // X coordinate
            float z = Mathf.Sin(angle) * radius; // Z coordinate

            // Create base vertex using the right vector to maintain the circular base
            Vector3 baseVertex = baseCenter + (right * x) + (transform.up * z);
            vertices[i + 1] = baseVertex; // Assign the vertex in world space
        }

        vertices[segments + 1] = baseCenter; // Center of the base

        // Triangles
        int[] triangles = new int[segments * 6]; // Each segment has two triangles (3 vertices per triangle)

        // Side triangles (tip to base)
        for (int i = 0; i < segments; i++)
        {
            triangles[i * 3] = 0; // Tip of the cone
            triangles[i * 3 + 1] = i + 1; // Current vertex
            triangles[i * 3 + 2] = (i + 1) % segments + 1; // Next vertex
        }

        // Base triangles (base of the cone)
        int baseIndex = segments * 3;
        for (int i = 0; i < segments; i++)
        {
            triangles[baseIndex + i * 3] = segments + 1; // Center of the base
            triangles[baseIndex + i * 3 + 1] = (i + 1) % segments + 1; // Next vertex
            triangles[baseIndex + i * 3 + 2] = i + 1; // Current vertex
        }

        // Normals
        Vector3[] normals = new Vector3[vertices.Length];
        normals[0] = -Vector3.forward; // Normal for the tip of the cone
        for (int i = 0; i < segments; i++)
        {
            normals[i + 1] = (vertices[i + 1] - vertices[0]).normalized; // Normals for the side vertices
        }
        normals[segments + 1] = Vector3.forward; // Normal for the base center

        // UVs (optional, for texturing)
        Vector2[] uvs = new Vector2[vertices.Length];
        uvs[0] = new Vector2(0.5f, 1f); // Tip
        for (int i = 0; i < segments; i++)
        {
            float u = (float)i / segments;
            uvs[i + 1] = new Vector2(u, 0);
        }
        uvs[segments + 1] = new Vector2(0.5f, 0); // Base center

        // Assign data to mesh
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.normals = normals;
        mesh.uv = uvs;

        mesh.RecalculateBounds();

        return mesh;
    }

    #endregion
}

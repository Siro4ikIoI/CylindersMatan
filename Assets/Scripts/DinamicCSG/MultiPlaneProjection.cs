using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class MultiPlaneProjection : MonoBehaviour
{
    public float projectionOffset = 1.5f; // Расстояние от оригинала
    public Material projectionMaterial;   // Материал для проекций

    private GameObject projectionXY, projectionXZ, projectionZY;
    private MeshFilter originalMeshFilter;
    private Mesh originalMesh;

    void Start()
    {
        projectionXY = CreateProjection(ProjectionPlane.XY);
        projectionXZ = CreateProjection(ProjectionPlane.XZ);
        projectionZY = CreateProjection(ProjectionPlane.ZY);
    }

    void Update()
    {
        originalMeshFilter = GetComponent<MeshFilter>();
        originalMesh = originalMeshFilter.mesh;

        projectionXY.transform.position = transform.position + Vector3.back * projectionOffset;
        projectionXZ.transform.position = transform.position + Vector3.down * projectionOffset;
        projectionZY.transform.position = transform.position + Vector3.left * projectionOffset;

        UpdateProjectionMesh(projectionXY, ProjectionPlane.XY);
        UpdateProjectionMesh(projectionXZ, ProjectionPlane.XZ);
        UpdateProjectionMesh(projectionZY, ProjectionPlane.ZY);
    }

    GameObject CreateProjection(ProjectionPlane plane)
    {
        GameObject proj = new GameObject($"Projection_{plane}");
        proj.transform.SetParent(transform);
        proj.transform.localRotation = Quaternion.identity;

        MeshFilter mf = proj.AddComponent<MeshFilter>();
        MeshRenderer mr = proj.AddComponent<MeshRenderer>();

        mf.mesh = new Mesh();
        mr.material = projectionMaterial ? projectionMaterial : GetComponent<MeshRenderer>().sharedMaterial;

        return proj;
    }

    void UpdateProjectionMesh(GameObject proj, ProjectionPlane plane)
    {
        MeshFilter mf = proj.GetComponent<MeshFilter>();
        if (!mf) return;

        Vector3[] vertices = originalMesh.vertices;
        int[] triangles = originalMesh.triangles;
        Vector2[] uvs = originalMesh.uv;
        Vector3[] newVertices = new Vector3[vertices.Length];

        for (int i = 0; i < vertices.Length; i++)
        {
            newVertices[i] = ProjectToPlane(vertices[i], plane);
        }

        Mesh newMesh = new Mesh
        {
            vertices = newVertices,
            triangles = triangles,
            uv = uvs
        };

        newMesh.RecalculateNormals();
        newMesh.RecalculateBounds();
        mf.mesh = newMesh;
    }

    Vector3 ProjectToPlane(Vector3 vertex, ProjectionPlane plane)
    {
        switch (plane)
        {
            case ProjectionPlane.XY: return new Vector3(vertex.x, vertex.y, 0);
            case ProjectionPlane.XZ: return new Vector3(vertex.x, 0, vertex.z);
            case ProjectionPlane.ZY: return new Vector3(0, vertex.y, vertex.z);
            default: return vertex;
        }
    }

    enum ProjectionPlane { XY, XZ, ZY }
}

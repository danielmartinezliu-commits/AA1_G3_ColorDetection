using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class NavMeshVisualizer : MonoBehaviour
{
    private Mesh mesh;

    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        var mat = new Material(Shader.Find("Standard"));
        mat.color = new Color(0, 1, 0, 0.3f);
        mat.SetFloat("_Mode", 3);
        GetComponent<MeshRenderer>().material = mat;

        InvokeRepeating(nameof(UpdateMesh), 1.5f, 2f);
    }

    void UpdateMesh()
    {
        NavMeshTriangulation tri = NavMesh.CalculateTriangulation();

        mesh.Clear();
        mesh.vertices = tri.vertices;
        mesh.triangles = tri.indices;
        mesh.RecalculateNormals();
    }
}
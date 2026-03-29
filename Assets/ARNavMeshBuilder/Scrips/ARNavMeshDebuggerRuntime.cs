using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class ARNavMeshDebuggerRuntime : MonoBehaviour
{
    [Header("NavMesh Colors")]
    public Color fillColor   = new Color(0.1f, 0.5f, 1f, 0.35f);
    public Color borderColor = new Color(0.1f, 0.7f, 1f, 0.9f);

    private Material _mat;
    private Camera   _cam;

    void Awake()
    {
        _cam = GetComponent<Camera>();

        _mat = new Material(Shader.Find("Hidden/Internal-Colored"));
        _mat.hideFlags = HideFlags.HideAndDontSave;
        _mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        _mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        _mat.SetInt("_Cull",     (int)UnityEngine.Rendering.CullMode.Off);
        _mat.SetInt("_ZWrite", 0);
    }

    void OnEnable()
    {
        RenderPipelineManager.endCameraRendering += OnEndCameraRendering;
    }

    void OnDisable()
    {
        RenderPipelineManager.endCameraRendering -= OnEndCameraRendering;
    }

    void OnDestroy()
    {
        if (_mat != null) Destroy(_mat);
    }

    void OnEndCameraRendering(ScriptableRenderContext context, Camera cam)
    {
        if (cam != _cam) return;

        _mat.SetPass(0);
        GL.PushMatrix();
        DrawNavMeshTriangulation();
        GL.PopMatrix();
    }

    void DrawNavMeshTriangulation()
    {
        NavMeshTriangulation tri = NavMesh.CalculateTriangulation();
        if (tri.vertices.Length == 0) return;

        float offset = 0.02f;

        GL.Begin(GL.TRIANGLES);
        GL.Color(fillColor);
        for (int i = 0; i < tri.indices.Length; i += 3)
        {
            GL.Vertex(tri.vertices[tri.indices[i]]     + Vector3.up * offset);
            GL.Vertex(tri.vertices[tri.indices[i + 1]] + Vector3.up * offset);
            GL.Vertex(tri.vertices[tri.indices[i + 2]] + Vector3.up * offset);
        }
        GL.End();

        GL.Begin(GL.LINES);
        GL.Color(borderColor);
        for (int i = 0; i < tri.indices.Length; i += 3)
        {
            Vector3 v0 = tri.vertices[tri.indices[i]]     + Vector3.up * (offset + 0.002f);
            Vector3 v1 = tri.vertices[tri.indices[i + 1]] + Vector3.up * (offset + 0.002f);
            Vector3 v2 = tri.vertices[tri.indices[i + 2]] + Vector3.up * (offset + 0.002f);
            GL.Vertex(v0); GL.Vertex(v1);
            GL.Vertex(v1); GL.Vertex(v2);
            GL.Vertex(v2); GL.Vertex(v0);
        }
        GL.End();
    }
}
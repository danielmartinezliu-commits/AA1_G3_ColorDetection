using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(Camera))]
public class ARNavMeshDebuggerRuntime : MonoBehaviour
{
    [Header("Colores")]
    public Color colorOK   = new Color(0f, 1f, 0.3f, 0.4f);
    public Color colorFail = new Color(1f, 0.2f, 0f,  0.4f);
    public Color colorBorde = new Color(0f, 1f, 0.3f, 1f);

    [Header("Referencias")]
    public ARPlaneManager planeManager;

    private Material _mat;

    void Awake()
    {
        // Material unlit para GL
        _mat = new Material(Shader.Find("Hidden/Internal-Colored"));
        _mat.hideFlags = HideFlags.HideAndDontSave;
        _mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        _mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        _mat.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
        _mat.SetInt("_ZWrite", 0);

        if (planeManager == null)
            planeManager = FindObjectOfType<ARPlaneManager>();
    }

    void OnDestroy()
    {
        if (_mat != null) Destroy(_mat);
    }

    void OnPostRender()
    {
        if (planeManager == null) return;

        _mat.SetPass(0);
        GL.PushMatrix();

        foreach (var plane in planeManager.trackables)
        {
            var surface   = plane.GetComponent<NavMeshSurface>();
            var meshFilter = plane.GetComponent<MeshFilter>();
            if (meshFilter == null || meshFilter.sharedMesh == null) continue;

            bool ok = surface != null && surface.navMeshData != null;
            Mesh mesh = meshFilter.sharedMesh;
            Transform t = plane.transform;

            // Relleno (triangulos)
            GL.Begin(GL.TRIANGLES);
            GL.Color(ok ? colorOK : colorFail);
            foreach (int idx in mesh.triangles)
            {
                Vector3 worldPos = t.TransformPoint(mesh.vertices[idx]);
                worldPos += t.up * 0.01f; // offset para evitar z-fighting con el plano AR
                GL.Vertex(worldPos);
            }
            GL.End();

            // Borde (wireframe por líneas)
            GL.Begin(GL.LINES);
            GL.Color(ok ? colorBorde : new Color(1f, 0.2f, 0f, 1f));
            int[] tris = mesh.triangles;
            for (int i = 0; i < tris.Length; i += 3)
            {
                Vector3 v0 = t.TransformPoint(mesh.vertices[tris[i]])     + t.up * 0.012f;
                Vector3 v1 = t.TransformPoint(mesh.vertices[tris[i + 1]]) + t.up * 0.012f;
                Vector3 v2 = t.TransformPoint(mesh.vertices[tris[i + 2]]) + t.up * 0.012f;
                GL.Vertex(v0); GL.Vertex(v1);
                GL.Vertex(v1); GL.Vertex(v2);
                GL.Vertex(v2); GL.Vertex(v0);
            }
            GL.End();
        }

        GL.PopMatrix();
    }
}
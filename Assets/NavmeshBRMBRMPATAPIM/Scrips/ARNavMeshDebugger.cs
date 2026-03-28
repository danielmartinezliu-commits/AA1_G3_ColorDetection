using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.XR.ARFoundation;

public class ARNavMeshDebugger : MonoBehaviour
{
    [Header("Colores")]
    public Color colorSuperficieOK   = new Color(0f, 1f, 0.3f, 0.35f);
    public Color colorSuperficieFail = new Color(1f, 0.2f, 0f,  0.35f);
    public Color colorBorde          = new Color(0f, 1f, 0.3f, 0.9f);
    public Color colorLabel          = Color.white;

    [Header("Opciones")]
    public bool mostrarTriangulos = false;
    public bool mostrarLabels     = true;

    private ARPlaneManager _planeManager;

    void Awake() => _planeManager = GetComponentInParent<ARPlaneManager>();

    void OnDrawGizmos()
    {
        if (_planeManager == null) return;

        foreach (var plane in _planeManager.trackables)
        {
            var surface = plane.GetComponent<NavMeshSurface>();
            bool tieneNavMesh = surface != null && surface.navMeshData != null;

            var meshFilter = plane.GetComponent<MeshFilter>();
            if (meshFilter == null || meshFilter.sharedMesh == null) continue;

            Gizmos.matrix = plane.transform.localToWorldMatrix;

            // Relleno del plano
            Gizmos.color = tieneNavMesh ? colorSuperficieOK : colorSuperficieFail;
            Gizmos.DrawMesh(meshFilter.sharedMesh);

            // Borde (wireframe)
            Gizmos.color = tieneNavMesh ? colorBorde : colorSuperficieFail;
            Gizmos.DrawWireMesh(meshFilter.sharedMesh);

            // Triangulos internos (opcional)
            if (mostrarTriangulos)
            {
                Gizmos.color = new Color(colorBorde.r, colorBorde.g, colorBorde.b, 0.2f);
                var verts = meshFilter.sharedMesh.vertices;
                var tris  = meshFilter.sharedMesh.triangles;
                for (int i = 0; i < tris.Length; i += 3)
                {
                    Gizmos.DrawLine(verts[tris[i]],   verts[tris[i+1]]);
                    Gizmos.DrawLine(verts[tris[i+1]], verts[tris[i+2]]);
                    Gizmos.DrawLine(verts[tris[i+2]], verts[tris[i]]);
                }
            }

            // Label en el centro
            if (mostrarLabels)
            {
                Gizmos.matrix = Matrix4x4.identity;
                Vector3 centro = plane.transform.position + Vector3.up * 0.05f;
                string texto = tieneNavMesh
                    ? $"NavMesh OK\n{plane.size.x:F1}x{plane.size.y:F1}m"
                    : "Sin NavMesh";

#if UNITY_EDITOR
                UnityEditor.Handles.color = colorLabel;
                UnityEditor.Handles.Label(centro, texto);
#endif
            }
        }
    }
}
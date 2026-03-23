using UnityEngine;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARPlane))]
public class NavMeshVisualizer : MonoBehaviour
{
    private ARPlane plane;
    private MeshRenderer meshRenderer;

    void Awake()
    {
        plane = GetComponent<ARPlane>();

        meshRenderer = GetComponent<MeshRenderer>();

        var mat = new Material(Shader.Find("Standard"));
        mat.color = new Color(0, 1, 0, 0.3f);
        meshRenderer.material = mat;
    }
}
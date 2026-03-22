using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARPlane))]
public class ARPlaneNavMesh : MonoBehaviour
{
    private NavMeshSurface surface;

    void Awake()
    {
        surface = gameObject.AddComponent<NavMeshSurface>();
        surface.collectObjects = CollectObjects.Children;
        surface.useGeometry = NavMeshCollectGeometry.RenderMeshes;
        surface.layerMask = LayerMask.GetMask("ARPlane");
    }

    void Start()
    {
        InvokeRepeating(nameof(BuildNavMesh), 1f, 2f);
    }

    public void BuildNavMesh()
    {
        surface.BuildNavMesh();
    }
}
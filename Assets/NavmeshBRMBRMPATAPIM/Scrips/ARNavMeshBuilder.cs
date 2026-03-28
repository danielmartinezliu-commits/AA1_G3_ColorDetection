using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARPlaneManager))]
public class ARNavMeshBuilder : MonoBehaviour
{
    [Header("NavMesh Settings")]
    [SerializeField] private NavMeshSurface navMeshSurfacePrefab;
    [SerializeField] private int agentTypeID = 1;

    private ARPlaneManager _planeManager;
    private readonly Dictionary<TrackableId, NavMeshSurface> _surfaces = new();

    void Awake()
    {
        _planeManager = GetComponent<ARPlaneManager>();
    }

    void OnEnable()
    {
        _planeManager.planesChanged += OnPlanesChanged;
    }

    void OnDisable()
    {
        _planeManager.planesChanged -= OnPlanesChanged;
    }

    private void OnPlanesChanged(ARPlanesChangedEventArgs args)
    {
        Debug.Log("Adding NavMesh");
        // Planos nuevos → añadir NavMeshSurface y buildear
        foreach (var plane in args.added)
            AddNavMeshSurface(plane);

        // Planos actualizados → rebuild
        foreach (var plane in args.updated)
            RebuildSurface(plane);

        // Planos eliminados → limpiar
        foreach (var plane in args.removed)
            RemoveSurface(plane);
    }

    private void AddNavMeshSurface(ARPlane plane)
    {
        var surface = plane.gameObject.AddComponent<NavMeshSurface>();
        surface.agentTypeID = NavMesh.GetSettingsByIndex(agentTypeID).agentTypeID;
        
        surface.collectObjects = CollectObjects.Children;
        surface.useGeometry = NavMeshCollectGeometry.PhysicsColliders;
        
        surface.BuildNavMesh();

        _surfaces[plane.trackableId] = surface;
    }

    private void RebuildSurface(ARPlane plane)
    {
        if (_surfaces.TryGetValue(plane.trackableId, out var surface))
            surface.BuildNavMesh();
    }

    private void RemoveSurface(ARPlane plane)
    {
        if (_surfaces.TryGetValue(plane.trackableId, out var surface))
        {
            surface.RemoveData();
            Destroy(surface);
            _surfaces.Remove(plane.trackableId);
        }
    }
}
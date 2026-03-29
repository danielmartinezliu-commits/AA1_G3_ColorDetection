using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARPlaneManager))]
public class ARNavMeshBuilder : MonoBehaviour
{
    // ─────────────────────────────────────────────
    // PLANE DETECTION
    // ─────────────────────────────────────────────
    [Header("Plane Detection")]
    [Tooltip("Detect and show horizontal planes (floor, table, etc.)")]
    public bool detectHorizontal = true;

    [Tooltip("Detect and show vertical planes (walls)")]
    public bool detectVertical = false;

    // ─────────────────────────────────────────────
    // NAVMESH BUILD
    // ─────────────────────────────────────────────
    [Header("NavMesh Build")]
    [Tooltip("Build NavMesh on horizontal planes")]
    public bool navMeshOnHorizontal = true;

    [Tooltip("Build NavMesh on vertical planes (walls)")]
    public bool navMeshOnVertical = false;

    [Space(4)]
    [Tooltip("Agent Type index from Window → AI → Navigation → Agents")]
    public int agentTypeIndex = 1;

    [Tooltip("Geometry source used to build the NavMesh")]
    public NavMeshCollectGeometry useGeometry = NavMeshCollectGeometry.PhysicsColliders;

    [Tooltip("Tile size. Larger values cover small planes better")]
    public int tileSize = 256;

    [Tooltip("Voxel resolution. Smaller = more detail, higher cost")]
    [Range(0.01f, 0.5f)]
    public float voxelSize = 0.05f;

    [Tooltip("Minimum area for a triangle to be included in the NavMesh")]
    [Range(0f, 1f)]
    public float minRegionArea = 0f;

    // ─────────────────────────────────────────────
    // PRIVATE
    // ─────────────────────────────────────────────
    private ARPlaneManager _planeManager;
    private readonly Dictionary<TrackableId, NavMeshSurface> _surfaces = new();

    // ─────────────────────────────────────────────
    // INIT
    // ─────────────────────────────────────────────
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

    // ─────────────────────────────────────────────
    // PLANE VISIBILITY
    // ─────────────────────────────────────────────
    void Update()
    {
        foreach (var plane in _planeManager.trackables)
        {
            bool isVertical   = plane.alignment == PlaneAlignment.Vertical;
            bool isHorizontal = plane.alignment == PlaneAlignment.HorizontalUp
                                || plane.alignment == PlaneAlignment.HorizontalDown;

            bool visible = (isHorizontal && detectHorizontal) ||
                           (isVertical   && detectVertical);

            // Solo oculta el renderer, no el GameObject
            var renderer = plane.GetComponent<MeshRenderer>();
            if (renderer != null)
                renderer.enabled = visible;

            var lineRenderer = plane.GetComponent<LineRenderer>();
            if (lineRenderer != null)
                lineRenderer.enabled = visible;
        }
    }

    // ─────────────────────────────────────────────
    // PLANES CHANGED
    // ─────────────────────────────────────────────
    private void OnPlanesChanged(ARPlanesChangedEventArgs args)
    {
        foreach (var plane in args.added)   AddNavMeshSurface(plane);
        foreach (var plane in args.updated) RebuildSurface(plane);
        foreach (var plane in args.removed) RemoveSurface(plane);
    }

    // ─────────────────────────────────────────────
    // NAVMESH LOGIC
    // ─────────────────────────────────────────────
    private bool ShouldBuild(ARPlane plane)
    {
        bool isVertical   = plane.alignment == PlaneAlignment.Vertical;
        bool isHorizontal = plane.alignment == PlaneAlignment.HorizontalUp
                         || plane.alignment == PlaneAlignment.HorizontalDown;

        if (isHorizontal && !navMeshOnHorizontal) return false;
        if (isVertical   && !navMeshOnVertical)   return false;

        return true;
    }

    private void AddNavMeshSurface(ARPlane plane)
    {
        if (!ShouldBuild(plane)) return;

        var surface = plane.gameObject.AddComponent<NavMeshSurface>();
        ConfigureSurface(surface);
        surface.BuildNavMesh();
        _surfaces[plane.trackableId] = surface;
    }

    private void RebuildSurface(ARPlane plane)
    {
        if (_surfaces.TryGetValue(plane.trackableId, out var surface))
        {
            if (!ShouldBuild(plane))
            {
                RemoveSurface(plane);
                return;
            }
            ConfigureSurface(surface);
            surface.BuildNavMesh();
            return;
        }

        AddNavMeshSurface(plane);
    }

    private void RemoveSurface(ARPlane plane)
    {
        if (!_surfaces.TryGetValue(plane.trackableId, out var surface)) return;
        surface.RemoveData();
        Destroy(surface);
        _surfaces.Remove(plane.trackableId);
    }

    private void ConfigureSurface(NavMeshSurface surface)
    {
        surface.agentTypeID       = NavMesh.GetSettingsByIndex(agentTypeIndex).agentTypeID;
        surface.collectObjects    = CollectObjects.Children;
        surface.useGeometry       = useGeometry;
        surface.minRegionArea     = minRegionArea;
        surface.overrideTileSize  = true;
        surface.tileSize          = tileSize;
        surface.overrideVoxelSize = true;
        surface.voxelSize         = voxelSize;
        surface.buildHeightMesh   = false;
    }

    // ─────────────────────────────────────────────
    // PUBLIC API
    // ─────────────────────────────────────────────

    /// <summary>Rebuilds all existing NavMesh surfaces.</summary>
    public void RebuildAll()
    {
        foreach (var surface in _surfaces.Values)
            if (surface != null) surface.BuildNavMesh();
    }

    /// <summary>Removes all NavMesh surfaces and clears tracking data.</summary>
    public void ClearAll()
    {
        foreach (var kvp in _surfaces)
        {
            if (kvp.Value == null) continue;
            kvp.Value.RemoveData();
            Destroy(kvp.Value);
        }
        _surfaces.Clear();
    }

    /// <summary>Returns the NavMeshSurface for a given plane, or null.</summary>
    public NavMeshSurface GetSurface(ARPlane plane)
    {
        _surfaces.TryGetValue(plane.trackableId, out var surface);
        return surface;
    }
}
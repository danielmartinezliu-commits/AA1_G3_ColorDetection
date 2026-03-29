using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ARNavMeshBuilderConfig : MonoBehaviour
{
    // ─────────────────────────────────────────────
    // REFERENCES
    // ─────────────────────────────────────────────
    [Header("References")]
    [Tooltip("Script that handles AR plane detection and NavMesh building")]
    public ARNavMeshBuilder navMeshBuilder;
    [Tooltip("Script that renders the NavMesh debug overlay in Game View")]
    public ARNavMeshDebuggerRuntime runtimeDebugger;
    [Tooltip("AR Plane Manager used to toggle plane detection")]
    public ARPlaneManager planeManager;
    [Tooltip("Agent Drawer used to test navmesh")]
    public NavMeshAgentDrawer agentDrawer;

    // ─────────────────────────────────────────────
    // PLANE DETECTION
    // ─────────────────────────────────────────────
    [Header("Plane Detection")]
    [Tooltip("Enable or disable AR plane detection entirely")]
    public bool detectPlanes = true;

    [Tooltip("Detect horizontal planes (floor, table, etc.)")]
    public bool detectHorizontal = true;

    [Tooltip("Detect vertical planes (walls)")]
    public bool detectVertical = false;

    // ─────────────────────────────────────────────
    // NAVMESH BUILD
    // ─────────────────────────────────────────────
    [Header("NavMesh — Build")]
    [Tooltip("Enable or disable NavMesh building entirely")]
    public bool buildNavMesh = true;

    [Tooltip("Build NavMesh on horizontal planes")]
    public bool navMeshOnHorizontal = true;

    [Tooltip("Build NavMesh on vertical planes (walls)")]
    public bool navMeshOnVertical = false;
    // ─────────────────────────────────────────────
    // AGENT DRAWER
    // ─────────────────────────────────────────────
    [Header("Agent Drawer")]
    [Tooltip("Enable or disable the NavMesh agent drawer")]
    public bool agentDrawerEnabled = true;
    
    // ─────────────────────────────────────────────
    // DEBUG
    // ─────────────────────────────────────────────
    [Header("Debug")]
    [Tooltip("Show NavMesh overlay in Game View (runtime)")]
    public bool showRuntimeDebug = true;


    // ─────────────────────────────────────────────
    // PROPERTIES — apply changes when toggled
    // ─────────────────────────────────────────────
    public bool DetectPlanes
    {
        get => detectPlanes;
        set
        {
            detectPlanes = value;
            if (planeManager != null)
                planeManager.enabled = value;
        }
    }

    public bool DetectHorizontal
    {
        get => detectHorizontal;
        set
        {
            detectHorizontal = value;
            if (navMeshBuilder != null)
                navMeshBuilder.detectHorizontal = value;
        }
    }

    public bool DetectVertical
    {
        get => detectVertical;
        set
        {
            detectVertical = value;
            if (navMeshBuilder != null)
                navMeshBuilder.detectVertical = value;
        }
    }

    public bool BuildNavMesh
    {
        get => buildNavMesh;
        set
        {
            buildNavMesh = value;
            if (navMeshBuilder != null)
                navMeshBuilder.enabled = value;
        }
    }

    public bool NavMeshOnHorizontal
    {
        get => navMeshOnHorizontal;
        set
        {
            navMeshOnHorizontal = value;
            if (navMeshBuilder != null)
                navMeshBuilder.navMeshOnHorizontal = value;
        }
    }

    public bool NavMeshOnVertical
    {
        get => navMeshOnVertical;
        set
        {
            navMeshOnVertical = value;
            if (navMeshBuilder != null)
                navMeshBuilder.navMeshOnVertical = value;
        }
    }
    public bool AgentDrawerEnabled
    {
        get => agentDrawerEnabled;
        set
        {
            agentDrawerEnabled = value;

            if (agentDrawer == null) return;

            agentDrawer.enabled = value;

            if (!value && agentDrawer.targetObject != null)
                agentDrawer.targetObject.SetActive(false);
        }
    }
    
    public bool ShowRuntimeDebug
    {
        get => showRuntimeDebug;
        set
        {
            showRuntimeDebug = value;
            if (runtimeDebugger != null)
                runtimeDebugger.enabled = value;
        }
    }

    // ─────────────────────────────────────────────
    // INIT — push Inspector values to child scripts
    // ─────────────────────────────────────────────
    void Awake()
    {
        // Auto-find references if not assigned in Inspector
        if (navMeshBuilder  == null) navMeshBuilder  = GetComponentInChildren<ARNavMeshBuilder>();
        if (runtimeDebugger == null) runtimeDebugger = GetComponentInChildren<ARNavMeshDebuggerRuntime>();
        if (planeManager    == null) planeManager    = GetComponentInChildren<ARPlaneManager>();
        if (agentDrawer     == null) agentDrawer     = GetComponentInChildren<NavMeshAgentDrawer>();

        ApplyAll();
    }

    public void ApplyAll()
    {
        DetectPlanes       = detectPlanes;
        DetectHorizontal   = detectHorizontal;
        DetectVertical     = detectVertical;
        BuildNavMesh       = buildNavMesh;
        NavMeshOnHorizontal = navMeshOnHorizontal;
        NavMeshOnVertical  = navMeshOnVertical;
        AgentDrawerEnabled  = agentDrawerEnabled;
        ShowRuntimeDebug   = showRuntimeDebug;
    }
    
    void OnValidate() => ApplyAll();
}

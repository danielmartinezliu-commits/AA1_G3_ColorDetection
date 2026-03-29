using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ARNavMeshRuntimeUI : MonoBehaviour
{
    [Header("Config Reference")]
    public ARNavMeshBuilderConfig config;

    [Header("Plane Detection")]
    public Toggle toggleDetectPlanes;
    public Toggle toggleDetectHorizontal;
    public Toggle toggleDetectVertical;

    [Header("NavMesh Build")]
    public Toggle toggleBuildNavMesh;
    public Toggle toggleNavMeshHorizontal;
    public Toggle toggleNavMeshVertical;

    [Header("Agent Drawer")]
    public Toggle toggleAgentDrawer;

    [Header("Debug")]
    public Toggle toggleRuntimeDebug;

    [Header("Panel")]
    public GameObject panel;
    public Button buttonTogglePanel;

    void Start()
    {
        if (config == null)
            config = FindObjectOfType<ARNavMeshBuilderConfig>();

        // Sync UI state from config
        SyncToggles();

        // Listeners
        toggleDetectPlanes     .onValueChanged.AddListener(OnDetectPlanes);
        toggleDetectHorizontal .onValueChanged.AddListener(OnDetectHorizontal);
        toggleDetectVertical   .onValueChanged.AddListener(OnDetectVertical);
        toggleBuildNavMesh     .onValueChanged.AddListener(OnBuildNavMesh);
        toggleNavMeshHorizontal.onValueChanged.AddListener(OnNavMeshHorizontal);
        toggleNavMeshVertical  .onValueChanged.AddListener(OnNavMeshVertical);
        toggleAgentDrawer      .onValueChanged.AddListener(OnAgentDrawer);
        toggleRuntimeDebug     .onValueChanged.AddListener(OnRuntimeDebug);

        buttonTogglePanel.onClick.AddListener(TogglePanel);

        // Start with panel hidden
        panel.SetActive(false);
    }

    // ─────────────────────────────────────────────
    // SYNC
    // ─────────────────────────────────────────────

    void SyncToggles()
    {
        SetToggleWithoutNotify(toggleDetectPlanes,      config.detectPlanes);
        SetToggleWithoutNotify(toggleDetectHorizontal,  config.detectHorizontal);
        SetToggleWithoutNotify(toggleDetectVertical,    config.detectVertical);
        SetToggleWithoutNotify(toggleBuildNavMesh,      config.buildNavMesh);
        SetToggleWithoutNotify(toggleNavMeshHorizontal, config.navMeshOnHorizontal);
        SetToggleWithoutNotify(toggleNavMeshVertical,   config.navMeshOnVertical);
        SetToggleWithoutNotify(toggleAgentDrawer,       config.agentDrawerEnabled);
        SetToggleWithoutNotify(toggleRuntimeDebug,      config.showRuntimeDebug);

        RefreshInteractable();
    }

    // Keeps UI in sync without triggering listeners
    void SetToggleWithoutNotify(Toggle toggle, bool value)
    {
        if (toggle != null) toggle.SetIsOnWithoutNotify(value);
    }

    // ─────────────────────────────────────────────
    // INTERACTABLE — mirrors editor dependency rules
    // ─────────────────────────────────────────────

    void RefreshInteractable()
    {
        bool planesOn   = config.detectPlanes;
        bool buildOn    = planesOn && config.buildNavMesh;
        bool horizOn    = planesOn && config.detectHorizontal;
        bool vertOn     = planesOn && config.detectVertical;

        SetInteractable(toggleDetectHorizontal,  planesOn);
        SetInteractable(toggleDetectVertical,    planesOn);
        SetInteractable(toggleBuildNavMesh,      planesOn);
        SetInteractable(toggleNavMeshHorizontal, buildOn && horizOn);
        SetInteractable(toggleNavMeshVertical,   buildOn && vertOn);
        SetInteractable(toggleAgentDrawer,       buildOn);
        SetInteractable(toggleRuntimeDebug,      buildOn);
    }

    void SetInteractable(Toggle toggle, bool interactable)
    {
        if (toggle == null) return;
        toggle.interactable = interactable;

        // Dim label when not interactable
        var label = toggle.GetComponentInChildren<TextMeshProUGUI>();
        if (label != null)
            label.color = interactable
                ? Color.white
                : new Color(1f, 1f, 1f, 0.35f);
    }

    // ─────────────────────────────────────────────
    // LISTENERS
    // ─────────────────────────────────────────────

    void OnDetectPlanes(bool value)
    {
        config.detectPlanes = value;
        config.ApplyAll();
        RefreshInteractable();
    }

    void OnDetectHorizontal(bool value)
    {
        config.detectHorizontal = value;
        config.ApplyAll();
        RefreshInteractable();
    }

    void OnDetectVertical(bool value)
    {
        config.detectVertical = value;
        config.ApplyAll();
        RefreshInteractable();
    }

    void OnBuildNavMesh(bool value)
    {
        config.buildNavMesh = value;
        config.ApplyAll();
        RefreshInteractable();
    }

    void OnNavMeshHorizontal(bool value)
    {
        config.navMeshOnHorizontal = value;
        config.ApplyAll();
    }

    void OnNavMeshVertical(bool value)
    {
        config.navMeshOnVertical = value;
        config.ApplyAll();
    }

    void OnAgentDrawer(bool value)
    {
        config.agentDrawerEnabled = value;
        config.ApplyAll();
    }

    void OnRuntimeDebug(bool value)
    {
        config.showRuntimeDebug = value;
        config.ApplyAll();
    }

    // ─────────────────────────────────────────────
    // PANEL
    // ─────────────────────────────────────────────

    void TogglePanel()
    {
        panel.SetActive(!panel.activeSelf);
        if (panel.activeSelf) SyncToggles();
    }
}
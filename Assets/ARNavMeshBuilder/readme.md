# AR NavMesh Builder

A Unity package that dynamically builds a NavMesh on AR-detected planes using AR Foundation and AI Navigation. Includes ready-to-use prefabs and a demo scene.

---

## Requirements

| Dependency | Version |
|---|---|
| Unity | 2022.3 LTS or higher |
| AR Foundation | 5.0 or higher |
| AI Navigation | 1.1 or higher |
| XR Plugin Management | 4.0 or higher |
| ARCore XR Plugin (Android) | 5.0 or higher |
| ARKit XR Plugin (iOS) | 5.0 or higher |
| TextMeshPro | 3.0 or higher |

---

## Installation

1. Open `Window → Package Manager` and install all dependencies listed above from the **Unity Registry**
2. Import the `ARNavMeshBuilder` package into your project
3. Open the included demo scene at `ARNavMeshBuilder/Scene/NavigationAR`

---

## Quick Start

Drag the **XROriginARNavMesh** prefab into your scene. That's it — it includes all scripts pre-configured and ready to use.

If you prefer to set up manually, add the **NavMeshGenerator** prefab as a child of your existing `XR Origin`.

---

## Scripts Overview

| Script | Description |
|---|---|
| `ARNavMeshBuilder` | Listens to `ARPlaneManager` and builds a `NavMeshSurface` per detected plane |
| `ARNavMeshBuilderConfig` | Central config panel — controls all scripts from one Inspector |
| `ARNavMeshDebuggerRuntime` | Renders the NavMesh as a GL overlay in Game View (URP) — place on Main Camera |
| `ARNavMeshRuntimeUI` | In-game UI panel to toggle NavMesh options at runtime |
| `NavMeshAgentDrawer` | Moves a target object to the nearest valid NavMesh position under the camera |

---

## Build Configuration

### Android

When running on an Android device, the app will request camera permission on launch. Make sure to **grant camera access** when prompted — without it AR tracking will not work.

### iOS

Go to `Edit → Project Settings → Player → iOS → Other Settings` and fill in **Camera Usage Description**:
```
This app uses the camera for Augmented Reality.
```

---

## Important

If you want the NavMesh to build closer to the edges of detected planes, go to
`Window → AI → Navigation → Agents` and create a new Agent with the following values:

| Parameter | Recommended value |
|---|---|
| Radius | 0.1 |
| Height | 0.1 |
| Max Slope | 90 |
| Step Height | 0.1 |

Then open the **XROriginARNavMesh** prefab, select the `XR Origin (Mobile AR)` GameObject,
and in the `ARNavMeshBuilder` component set **Agent Type Index** to match the index of
the new agent you just created (e.g. if it is the second agent in the list, set it to `1`).

---

## Troubleshooting

**Planes detected slowly on device**
AR Foundation needs plane GameObjects to remain active to keep tracking. The package handles visibility via `MeshRenderer.enabled` instead of `SetActive(false)`.

**Nothing visible in Game View**
`ARNavMeshDebuggerRuntime` requires URP. If using Built-in RP, replace `RenderPipelineManager.endCameraRendering` with `OnPostRender`.

**App crashes on iOS**
`NSCameraUsageDescription` is missing — see the iOS build section above.
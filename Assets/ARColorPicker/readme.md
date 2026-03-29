# AR Color Picker

A Unity package that detects real-world colors using AR Foundation and generates dynamic color palettes. Includes ready-to-use prefabs and a demo scene.

---

## Requirements

| Dependency | Version |
|---|---|
| Unity | 2022.3 LTS or higher |
| AR Foundation | 5.0 or higher |
| XR Plugin Management | 4.0 or higher |
| ARCore XR Plugin (Android) | 5.0 or higher |
| ARKit XR Plugin (iOS) | 5.0 or higher |
| TextMeshPro | 3.0 or higher |

---

## Installation

1. Open `Window → Package Manager` and install all dependencies listed above from the **Unity Registry**
2. Import the `AR Color Picker` package into your project
3. Open the included demo scene at `Scenes/ColorDetector`

---

## Quick Start

Drag the **ColorManager** prefab into your scene. That's it — it includes all scripts pre-configured and ready to use.

If you prefer to set up manually:

- Add the **ColorManager** prefab
- Ensure it has a child GameObject called `ColorDetector`
- Assign an `ARCameraManager` to the `cameraManager` field inside the `ColorDetector` component

---

## Scripts Overview

| Script | Description |
|---|---|
| `ColorManager` | Main controller — handles color capture and palette generation |
| `ColorDetector` | Captures the center pixel color from the AR camera feed |
| `ColorPalette` | Struct that stores palette data (selected, variations, primary, contrast) |
| `IColorPicker` | Interface for custom palette generation |
| `ConfiguredColorPicker` | Default HSV-based palette generator |

---

## Custom Color Picker

The color generation system is fully customizable.

To create your own palette logic, implement the `IColorPicker` interface:

```csharp
public interface IColorPicker 
{
    ColorPalette GetColorPalette(Color selectedColor);
}
```

Then:

1. Create a new script implementing `IColorPicker`
2. Attach it to the object assigned as `colorPickerObject` in `ColorManager`

This allows you to define your own:
- Palette generation rules
- Color relationships
- Artistic or functional color systems

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

The `ColorManager` requires a valid implementation of `IColorPicker`.

If no component implementing this interface is found in the assigned `colorPickerObject`, the system will log an error and disable itself.

---

## Troubleshooting

**No color detected**
Ensure `ARCameraManager` is correctly assigned and AR session is running.

**UI not updating**
Check all references inside `ColorManager` (images, prefabs, spawn positions).

**App shows static or incorrect color**
Make sure the camera feed is active and permissions are granted.


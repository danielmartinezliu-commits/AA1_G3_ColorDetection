using UnityEngine;

public class ConfiguredColorPicker : MonoBehaviour, IColorPicker
{
    public ColorPalette GetColorPalette(Color _selectedColor)
    {
        ColorPalette palette = new ColorPalette();


        return palette;

    }
}

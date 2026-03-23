using UnityEngine;

public class ConfiguredColorPicker : MonoBehaviour, IColorPicker
{
    public ColorPalette GetColorPalette(Color _selectedColor)
    {
        Color.RGBToHSV(_selectedColor, out float h, out float s, out float v);


        Color[] palette = new Color[5];

        for (int i = 0; i < 5; i++)
        {
            float newHue = (h + (i + 1) * 0.05f) % 1f; // pequeños offsets
            float newSat = Mathf.Clamp01(s * (0.8f + i * 0.05f));
            float newVal = Mathf.Clamp01(v * (0.9f + i * 0.02f));

            palette[i] = Color.HSVToRGB(newHue, newSat, newVal);
        }

        Color primaryColor = Color.HSVToRGB(
            h,
            Mathf.Clamp01(s * 0.5f),
            Mathf.Clamp01(v * 1.1f)
        );

        float contrastHue = (h + 0.5f) % 1f;
        Color contrastColor = Color.HSVToRGB(contrastHue, s, v);

        return new ColorPalette(
            _selectedColor,
            palette,
            primaryColor,
            contrastColor
        );
    }
}

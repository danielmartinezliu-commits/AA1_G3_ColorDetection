using UnityEngine;

public struct ColorPalette
{
    public ColorPalette(Color _selectedColor, Color[] _selectedCPal, Color _primaryCToSelected, Color _contrastC)
    {
        selectedColor = _selectedColor;
        selectedColorPalette = _selectedCPal;
        primaryColorToSelected = _primaryCToSelected;
        contrastColor = _contrastC;
    }

    public Color GetSelectedColor() { return selectedColor; }
    public Color[] GetPaletteColors() { return selectedColorPalette; }
    public Color GetPrimaryColor() { return primaryColorToSelected; }
    public Color GetContrastColor() { return contrastColor; }

    private Color selectedColor;
    private Color[] selectedColorPalette;
    private Color primaryColorToSelected;
    private Color contrastColor;
}

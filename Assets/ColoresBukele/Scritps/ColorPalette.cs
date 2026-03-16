using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

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
    public Color[] GetPalette() {  return selectedColorPalette; }
    public Color GetPrimaryColor() { return primaryColorToSelected; }
    public Color GetContrastColor() { return contrastColor; }

    Color selectedColor;
    Color[] selectedColorPalette;
    Color primaryColorToSelected;
    Color contrastColor;
    
}

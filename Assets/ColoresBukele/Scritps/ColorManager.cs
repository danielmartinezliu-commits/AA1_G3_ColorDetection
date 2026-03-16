using UnityEngine;

public class ColorManager : MonoBehaviour
{
    public static ColorManager Instance { get; private set; }

    [SerializeField]
    private ColorDetector detector;
    [SerializeField]
    private GameObject colorPickerObject;
    private IColorPicker colorPicker;


    private void Awake()
    {
        if(Instance != null && Instance != this) 
            Destroy(Instance.gameObject);

        Instance = this;

    }

    private void Start()
    {
        if (colorPickerObject.TryGetComponent(out IColorPicker _colorPicker))
            colorPicker = _colorPicker;
        else
        {
            Debug.LogError("El objeto seleccionado no contiene ningun componente que implemente IColorPicker");
            Destroy(gameObject);
        }
    }

    public void CaptureCurrentColor()
    {
        detector.CaptureCurrentColor();
    }

    public ColorPalette GetColorPalette()
    {
        return colorPicker.GetColorPalette(detector.GetSelectedColor());
    }





}

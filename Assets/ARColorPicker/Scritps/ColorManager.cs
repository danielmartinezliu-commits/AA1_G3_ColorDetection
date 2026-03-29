using UnityEngine;
using UnityEngine.UI;

public class ColorManager : MonoBehaviour
{
    public static ColorManager Instance { get; private set; }

    [SerializeField]
    private ColorDetector detector;
    [SerializeField]
    private GameObject colorPickerObject;


    private IColorPicker colorPicker;

    private ColorPalette currentPalette;


    [Space, SerializeField]
    private Image selectedColorImage;
    [SerializeField]
    private Image closestPrimaryColorImage;
    [SerializeField]
    private Image contrastColorImage;

    [Space, SerializeField]
    private GameObject colorImagePrefab;
    [SerializeField]
    private Transform colorImagesSpawnPos;
    [SerializeField]
    private float colorImagesOffset;

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
        currentPalette = colorPicker.GetColorPalette(detector.GetSelectedColor());
        selectedColorImage.color = currentPalette.GetSelectedColor();
        closestPrimaryColorImage.color = currentPalette.GetPrimaryColor();
        contrastColorImage.color = currentPalette.GetContrastColor();

        Color[] paletteColors = currentPalette.GetPaletteColors();

        for (int i = 0; i < paletteColors.Length; i++)
        {
            GameObject paletteColorObj = Instantiate(colorImagePrefab, colorImagesSpawnPos);

            paletteColorObj.transform.position = colorImagesSpawnPos.position + new Vector3(0, -colorImagesOffset * i);
            paletteColorObj.GetComponent<Image>().color = paletteColors[i];
        }
    }

    public ColorPalette GetCurrentPalette() { return currentPalette; }





}

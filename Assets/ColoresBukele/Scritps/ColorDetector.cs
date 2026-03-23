using UnityEngine;
using UnityEngine.XR.ARFoundation;
using Unity.Collections;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;

public class ColorDetector : MonoBehaviour
{
    public ARCameraManager cameraManager;
    public Image previewColorImage;

    private Color detectedColor;
    void FixedUpdate()
    {
        CaptureColorUpdate();
    }

    private void CaptureColorUpdate()
    {
        if (cameraManager.TryAcquireLatestCpuImage(out XRCpuImage image))
        {
            var conversionParams = new XRCpuImage.ConversionParams
            {
                inputRect = new RectInt(0, 0, image.width, image.height),
                outputDimensions = new Vector2Int(image.width, image.height),
                outputFormat = TextureFormat.RGB24,
                transformation = XRCpuImage.Transformation.None
            };

            int size = image.GetConvertedDataSize(conversionParams);
            var buffer = new NativeArray<byte>(size, Allocator.Temp);

            image.Convert(conversionParams, buffer);
            image.Dispose();

            int centerX = image.width / 2;
            int centerY = image.height / 2;

            int index = (centerY * image.width + centerX) * 3;

            byte r = buffer[index];
            byte g = buffer[index + 1];
            byte b = buffer[index + 2];

            detectedColor = new Color32(r, g, b, 255);

            previewColorImage.color = detectedColor;

            Debug.Log("RGB: " + r + ", " + g + ", " + b);

            buffer.Dispose();
        }
    }

    public Color GetSelectedColor() { return detectedColor; }
}
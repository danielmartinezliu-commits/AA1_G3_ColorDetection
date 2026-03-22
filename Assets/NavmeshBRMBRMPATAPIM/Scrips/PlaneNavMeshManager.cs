using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class PlaneNavMeshManager : MonoBehaviour
{
    public ARPlaneManager planeManager;

    void OnEnable()
    {
        planeManager.planesChanged += OnPlanesChanged;
    }

    void OnDisable()
    {
        planeManager.planesChanged -= OnPlanesChanged;
    }

    void OnPlanesChanged(ARPlanesChangedEventArgs args)
    {
        foreach (var plane in args.added)
        {
            plane.gameObject.AddComponent<ARPlaneNavMesh>();

            plane.gameObject.AddComponent<NavMeshVisualizer>();
        }
    }
}
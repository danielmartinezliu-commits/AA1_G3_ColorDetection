using UnityEngine;
using UnityEngine.AI;

public class NavMeshDebugger : MonoBehaviour
{
    [Header("Referencia al objeto a mover/mostrar")]
    public GameObject targetObject;

    [Header("Cámara del dispositivo (AR Camera)")]
    public Transform cameraTransform;

    [Header("Distancia hacia delante si no hay impacto")]
    public float forwardOffset = 1.5f;

    [Header("Distancia máxima para buscar NavMesh")]
    public float maxSearchDistance = 2.0f;

    [Header("Capas para el Raycast")]
    public LayerMask raycastLayers = Physics.DefaultRaycastLayers;

    void Update()
    {
        if (targetObject == null || cameraTransform == null)
            return;

        Vector3 origin = cameraTransform.position;
        Vector3 direction = cameraTransform.forward;

        RaycastHit rayHit;
        Vector3 samplePoint;

        // 1. Raycast infinito hacia delante
        if (Physics.Raycast(origin, direction, out rayHit, Mathf.Infinity, raycastLayers))
        {
            // Si impacta → usamos el punto de colisión
            samplePoint = rayHit.point;
        }
        else
        {
            // Si no impacta → usamos un punto hacia delante
            Vector3 flatForward = Vector3.ProjectOnPlane(direction, Vector3.up).normalized;
            samplePoint = origin + flatForward * forwardOffset;
        }

        NavMeshHit navHit;

        // 2. Buscar punto más cercano en la NavMesh
        if (NavMesh.SamplePosition(samplePoint, out navHit, maxSearchDistance, NavMesh.AllAreas))
        {
            if (!targetObject.activeSelf)
                targetObject.SetActive(true);

            // 3. Posición
            targetObject.transform.position = navHit.position;

            // 4. Orientación → align con la normal de la NavMesh
            targetObject.transform.up = navHit.normal;
        }
        else
        {
            if (targetObject.activeSelf)
                targetObject.SetActive(false);
        }
    }
}

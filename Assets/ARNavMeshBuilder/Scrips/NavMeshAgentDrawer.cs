using UnityEngine;
using UnityEngine.AI;

public class NavMeshAgentDrawer : MonoBehaviour
{
    [Header("Target Object")]
    [Tooltip("Object to move and display on the NavMesh")]
    public GameObject targetObject;

    [Header("Camera")]
    [Tooltip("AR Camera transform used as raycast origin")]
    public Transform cameraTransform;

    [Header("Settings")]
    [Tooltip("Forward distance used when raycast hits nothing")]
    public float forwardOffset = 1.5f;

    [Tooltip("Max distance to search for a valid NavMesh position")]
    public float maxSearchDistance = 2.0f;

    [Tooltip("Layers considered by the raycast")]
    public LayerMask raycastLayers = Physics.DefaultRaycastLayers;

    void Update()
    {
        if (targetObject == null || cameraTransform == null) return;

        Vector3 origin    = cameraTransform.position;
        Vector3 direction = cameraTransform.forward;

        Vector3 samplePoint;

        if (Physics.Raycast(origin, direction, out RaycastHit rayHit, Mathf.Infinity, raycastLayers))
        {
            samplePoint = rayHit.point;
        }
        else
        {
            Vector3 flatForward = Vector3.ProjectOnPlane(direction, Vector3.up).normalized;
            samplePoint = origin + flatForward * forwardOffset;
        }

        if (NavMesh.SamplePosition(samplePoint, out NavMeshHit navHit, maxSearchDistance, NavMesh.AllAreas))
        {
            if (!targetObject.activeSelf)
                targetObject.SetActive(true);

            targetObject.transform.position = navHit.position;
            targetObject.transform.up       = navHit.normal;
        }
        else
        {
            if (targetObject.activeSelf)
                targetObject.SetActive(false);
        }
    }
}
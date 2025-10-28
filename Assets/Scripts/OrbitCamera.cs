using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Camera))]
public class OrbitCamera : MonoBehaviour
{
    [Header("Target")]
    public Transform target;
    [Tooltip("If the target has a LorenzAttractor, the camera will orbit its dynamic center.")]
    public bool useDynamicCenter = true;

    [Header("Rotation Settings")]
    public float rotationSpeed = 200f;        // Sensitivity of rotation
    public float rotationSmoothTime = 0.1f;   // Smoothing for rotation
    public float minYAngle = -20f;
    public float maxYAngle = 80f;

    [Header("Zoom Settings")]
    public float distance = 10f;
    public float minDistance = 1f;
    public float maxDistance = 50f;
    public float zoomSpeed = 10f;
    public float zoomSmoothTime = 0.1f;

    [Header("Center Smoothing")]
    public float centerSmoothTime = 0.1f;

    // Rotation
    private float yaw;
    private float pitch;
    private Vector2 currentRotationVelocity;
    private Vector2 targetRotationDelta;

    // Zoom
    private float targetDistance;
    private float currentDistanceVelocity;

    // Dynamic center
    private Vector3 currentCenter;
    private Vector3 centerVelocity;

    void Start()
    {
        targetDistance = distance;

        if (target != null)
            currentCenter = target.position;
    }

    void LateUpdate()
    {
        if (target == null) return;

        var mouse = Mouse.current;

        // --- ROTATION ---
        if (mouse != null && mouse.leftButton.isPressed)
        {
            Vector2 mouseDelta = mouse.delta.ReadValue();
            targetRotationDelta = new Vector2(-mouseDelta.y, mouseDelta.x) * rotationSpeed * Time.deltaTime;
        }
        else
        {
            targetRotationDelta = Vector2.zero;
        }

        yaw += Mathf.SmoothDamp(0, targetRotationDelta.y, ref currentRotationVelocity.y, rotationSmoothTime);
        pitch += Mathf.SmoothDamp(0, targetRotationDelta.x, ref currentRotationVelocity.x, rotationSmoothTime);
        pitch = Mathf.Clamp(pitch, minYAngle, maxYAngle);

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);

        // --- ZOOM ---
        if (mouse != null)
        {
            float scroll = mouse.scroll.ReadValue().y;
            targetDistance -= scroll * zoomSpeed * Time.deltaTime;
            targetDistance = Mathf.Clamp(targetDistance, minDistance, maxDistance);
        }

        distance = Mathf.SmoothDamp(distance, targetDistance, ref currentDistanceVelocity, zoomSmoothTime);

        // --- DYNAMIC CENTER ---
        Vector3 targetCenterPos = target.position;
        if (useDynamicCenter)
        {
            LorenzAttractor attractor = target.GetComponent<LorenzAttractor>();
            if (attractor != null)
                targetCenterPos = attractor.GetCenter();
        }

        currentCenter = Vector3.SmoothDamp(currentCenter, targetCenterPos, ref centerVelocity, centerSmoothTime);

        // --- APPLY POSITION & ROTATION ---
        transform.position = currentCenter - rotation * Vector3.forward * distance;
        transform.rotation = rotation;
    }
}

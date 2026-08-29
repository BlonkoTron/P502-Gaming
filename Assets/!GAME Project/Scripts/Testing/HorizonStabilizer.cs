using UnityEngine;

public class HorizonStabilizer : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("Smoothing speed. Higher is faster. Set to 0 for instant locking.")]
    public float smoothSpeed = 20f;

    [Tooltip("If true, the camera stays level. If false, the camera tilts with your hand but stays smooth.")]
    public bool lockHorizon = true;

    // We add this to remember our smoothed rotation independently of the parent's instant snapping
    private Quaternion _currentRotation;

    void Start()
    {
        // Initialize our memory to wherever we start
        _currentRotation = transform.rotation;
    }

    void LateUpdate()
    {
        Quaternion targetRotation;

        if (lockHorizon)
        {
            // 1. HORIZON LOCK ON: Calculate rotation to kill the "Roll"
            Vector3 targetForward = transform.parent.forward;

            // Prevent error if looking perfectly straight up or down
            if (targetForward != Vector3.zero && Mathf.Abs(Vector3.Dot(targetForward, Vector3.up)) < 0.99f)
            {
                targetRotation = Quaternion.LookRotation(targetForward, Vector3.up);
            }
            else
            {
                // Fallback for looking straight up/down
                targetRotation = transform.parent.rotation;
            }
        }
        else
        {
            // 2. HORIZON LOCK OFF: Just use the exact rotation of the physical object
            targetRotation = transform.parent.rotation;
        }

        // 3. Apply the rotation (The Stabilization)
        if (smoothSpeed > 0)
        {
            // Smooth from our INDEPENDENT memory, not the instantly-snapping transform.rotation
            _currentRotation = Quaternion.Slerp(_currentRotation, targetRotation, Time.deltaTime * smoothSpeed);

            // Override the hierarchy's instant rotation with our smoothed one
            transform.rotation = _currentRotation;
        }
        else
        {
            // Instant locking
            transform.rotation = targetRotation;

            // Keep our memory updated even when instantly locking
            _currentRotation = targetRotation;
        }
    }

    // --- Helper functions to call from XR buttons ---

    public void ToggleHorizonLock()
    {
        lockHorizon = !lockHorizon;
    }

    public void SetHorizonLock(bool state)
    {
        lockHorizon = state;
    }
}
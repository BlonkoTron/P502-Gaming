using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(HingeJoint))]
public class Hinge_trigger : MonoBehaviour
{
    [Header("References")]
    public HingeJoint hinge;

    [Header("Trigger Settings")]
    public float pullDistanceThreshold = 0.5f;

    public float resetDistance = 0.3f;

    public float triggerCooldown = 1.0f;

    [Header("Events")]
    public UnityEvent onPulled; // Assign actions

    private Vector3 localAnchor;
    private Vector3 localConnectedAnchor;

    private float smoothedDistance;
    private float lastTriggerTime;
    private bool isTriggered;

    void Awake()
    {
        if (!hinge)
            hinge = GetComponent<HingeJoint>();

        // Cache local anchors so we don’t keep accessing transforms each frame
        localAnchor = hinge.anchor;
        localConnectedAnchor = hinge.connectedAnchor;
    }

    void FixedUpdate()
    {
        if (!hinge || !hinge.connectedBody) return;

        // Calculate world-space positions once per physics frame
        Vector3 anchorWorld = hinge.transform.TransformPoint(localAnchor);
        Vector3 connectedAnchorWorld = hinge.connectedBody.transform.TransformPoint(localConnectedAnchor);

        float distance = Vector3.Distance(anchorWorld, connectedAnchorWorld);

        // Smooth for visual stability
        smoothedDistance = Mathf.Lerp(smoothedDistance, distance, Time.fixedDeltaTime * 10f);

        // --- Trigger Logic ---
        if (!isTriggered && smoothedDistance >= pullDistanceThreshold && Time.time - lastTriggerTime > triggerCooldown)
        {
            isTriggered = true;
            lastTriggerTime = Time.time;
            Debug.Log($"[HingeTrigger] Pulled: {smoothedDistance:F3} m");
            onPulled.Invoke();
        }

        // --- Reset Logic ---
        if (isTriggered && smoothedDistance < resetDistance)
        {
            isTriggered = false;
        }
    }

    void OnDrawGizmosSelected()
    {
        if (hinge == null || hinge.connectedBody == null) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(hinge.transform.TransformPoint(hinge.anchor),
                        hinge.connectedBody.transform.TransformPoint(hinge.connectedAnchor));
    }
}

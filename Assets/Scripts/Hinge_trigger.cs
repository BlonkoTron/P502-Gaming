using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(HingeJoint))]
public class Hinge_trigger : MonoBehaviour
{
    [Header("References")]
    public HingeJoint hinge;

    [Header("Angle Trigger")]
    public float triggerAngle = 10f;
    public float resetAngle = 5f;

    [Header("Pull Trigger")]
    public float pullDistanceThreshold = 0.1f;
    public float resetDistance = 0.05f;

    [Header("Cooldown")]
    public float triggerCooldown = 1f;

    [Header("Events")]
    public UnityEvent onPulled;

    private bool isTriggered;
    private float lastTriggerTime;
    private Vector3 localAnchor;
    private Vector3 localConnectedAnchor;
    private float smoothedDistance;

    void Awake()
    {
        if (!hinge) hinge = GetComponent<HingeJoint>();
        localAnchor = hinge.anchor;
        localConnectedAnchor = hinge.connectedAnchor;
    }

    void FixedUpdate()
    {
        if (!hinge || !hinge.connectedBody) return;

        // -------- Angle --------
        float angle = Mathf.Abs(hinge.angle);

        // -------- Pull Distance in world space --------
        Vector3 worldA = hinge.transform.TransformPoint(localAnchor);
        Vector3 worldB = hinge.connectedBody.transform.TransformPoint(localConnectedAnchor);

        float rawDistance = Vector3.Distance(worldA, worldB);
        smoothedDistance = Mathf.Lerp(smoothedDistance, rawDistance, 0.12f);

        // --- Can trigger?
        bool anglePulled = angle >= triggerAngle;
        bool distancePulled = smoothedDistance >= pullDistanceThreshold;

        if (!isTriggered && (anglePulled || distancePulled) && Time.time - lastTriggerTime > triggerCooldown)
        {
            isTriggered = true;
            lastTriggerTime = Time.time;
            Debug.Log($"[HingeTrigger] Pulled (Angle:{angle:F2} Dist:{smoothedDistance:F3})");
            onPulled.Invoke();
        }

        // --- Reset logic ---
        bool angleReset = angle <= resetAngle;
        bool distanceReset = smoothedDistance <= resetDistance;

        if (isTriggered && angleReset && distanceReset)
            isTriggered = false;
    }
}

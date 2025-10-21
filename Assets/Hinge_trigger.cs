using UnityEngine;

public class Hinge_trigger : MonoBehaviour
{
    [Header("References")]
    public HingeJoint hinge;

    [Header("Trigger Settings")]
    public float pullDistanceThreshold = 0.3f; // meters between anchor points
    public float resetDistance = 0.15f;        // how close it must return to reset
    public float triggerCooldown = 0.5f;       // seconds between triggers

    private bool hasTriggered = false;
    private float lastTriggerTime = 0f;
    private float smoothedDistance = 0f;
    public bool SpawnFood = false;

    public float distance;

    private bool cooldown = false;

    void Start()
    {
        if (hinge == null)
            hinge = GetComponent<HingeJoint>();
    }

    void Update()
    {
        if (hinge == null || hinge.connectedBody == null)
            return;

        // Calculate current world-space positions of both hinge ends
        Vector3 anchorWorld = hinge.transform.TransformPoint(hinge.anchor);
        Vector3 connectedAnchorWorld = hinge.connectedBody.transform.TransformPoint(hinge.connectedAnchor);

        distance = Vector3.Distance(anchorWorld, connectedAnchorWorld);

        // Smooth out jitter
        smoothedDistance = Mathf.Lerp(smoothedDistance, distance, Time.deltaTime * 10f);

        // Check for pull
        if (hasTriggered == false && smoothedDistance >= pullDistanceThreshold && Time.time - lastTriggerTime > triggerCooldown)
        {
            hasTriggered = true;
            lastTriggerTime = Time.time;
            OnPull();
        }

        // Reset when the chain relaxes
        if (hasTriggered && smoothedDistance < resetDistance)
        {
            hasTriggered = false;
            SpawnFood = false;
            cooldown = false;
        }
    }

    void OnPull()
    {
        if (cooldown == false)
        {
            Debug.Log("Chain stretched beyond threshold!");
            SpawnFood = true;
            cooldown = true;
        }
        
    }
}

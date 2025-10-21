using UnityEngine;

public class Hinge_trigger : MonoBehaviour
{
    [Header("References")]
    public HingeJoint hinge;

    [Header("Trigger Settings")]
    public float pullAngleThreshold = 40f;  // degrees from rest position
    public float resetAngle = 100f;          // how far back it must go to reset
    public float triggerCooldown = 5.0f;    // seconds between triggers

    private bool hasTriggered = false;
    private float lastTriggerTime = 0f;
    private float smoothedAngle = 0f;
    public bool SpawnFood = false;

    void Start()
    {
        if (hinge == null)
            hinge = GetComponent<HingeJoint>();
    }

    void Update()
    {
        // Smooth the hinge angle to avoid jitter-triggering
        smoothedAngle = Mathf.Lerp(smoothedAngle, Mathf.Abs(hinge.angle), Time.deltaTime * 10f);

        // Check if rope is being pulled beyond threshold
        if (!hasTriggered && smoothedAngle >= pullAngleThreshold && Time.time - lastTriggerTime > triggerCooldown)
        {
            hasTriggered = true;
            lastTriggerTime = Time.time;
            OnPull();
        }

        // Reset when rope returns up
        if (hasTriggered && smoothedAngle < resetAngle)
        {
            hasTriggered = false;
            SpawnFood = false;
        }
    }

    void OnPull()
    {
        Debug.Log("🔔 Rope pulled!");
        SpawnFood = true;
        // Example: spawn something
        // Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
    }
}

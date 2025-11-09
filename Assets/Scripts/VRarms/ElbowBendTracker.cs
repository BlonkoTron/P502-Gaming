using UnityEngine;

public class ElbowBendTracker : MonoBehaviour
{
    [Header("Arm Transforms")]
    public Transform shoulder;
    public Transform elbow;
    public Transform hand;

    [Header("Debug")]
    public float currentBendAngle;
    public float minAngle;  // most bent
    public float maxAngle;  // most extended
    public float rangeOfMotion; // maxAngle - minAngle

    void Update()
    {
        if (shoulder == null || elbow == null || hand == null) return;

        // --- Calculate vectors ---
        Vector3 upperArm = elbow.position - shoulder.position;
        Vector3 lowerArm = hand.position - elbow.position;

        // --- Calculate angle between them ---
        currentBendAngle = Vector3.Angle(upperArm, lowerArm);

        // --- Track min/max over time ---
        if (minAngle == 0f || currentBendAngle < minAngle)
            minAngle = currentBendAngle;

        if (currentBendAngle > maxAngle)
            maxAngle = currentBendAngle;

        rangeOfMotion = maxAngle - minAngle;
    }

    public void ResetTracking()
    {
        minAngle = 0f;
        maxAngle = 0f;
        rangeOfMotion = 0f;
    }
}

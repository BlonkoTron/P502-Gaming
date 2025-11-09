using UnityEngine;

public class ArmFollowXR : MonoBehaviour
{
    [Header("XR Hand Targets")]
    public Transform leftHandXR;
    public Transform rightHandXR;

    [Header("Animated Arm Targets (Red Squares)")]
    public Transform leftArmTarget;
    public Transform rightArmTarget;

    [Header("Animated Elbow Targets (Blue Squares)")]
    public Transform leftElbowTarget;
    public Transform rightElbowTarget;

    [Header("Shoulder References")]
    public Transform leftShoulder;
    public Transform rightShoulder;

    [Header("Follow Settings")]
    public float followSpeed = 10f;
    public Vector3 handPositionOffset;
    public Vector3 leftHandRotationOffset;
    public Vector3 rightHandRotationOffset;

    [Header("Elbow Settings")]
    [Range(0f, 1f)]
    public float elbowBendRatio = 0.45f;
    public float elbowOutwardOffset = 0.15f;
    public float elbowDownOffset = -0.05f;
    public Vector3 leftElbowRotationOffset;
    public Vector3 rightElbowRotationOffset;

    [Header("Mirroring Options")]
    public bool mirrorLeftHand = false;
    public bool mirrorRightHand = false;

    void Update()
    {
        // --- HANDS ---
        if (leftHandXR && leftArmTarget)
            FollowTarget(leftArmTarget, leftHandXR, leftHandRotationOffset, handPositionOffset, mirrorLeftHand);

        if (rightHandXR && rightArmTarget)
            FollowTarget(rightArmTarget, rightHandXR, rightHandRotationOffset, handPositionOffset, mirrorRightHand);

        // --- ELBOWS ---
        if (leftShoulder && leftArmTarget && leftElbowTarget)
            UpdateElbow(leftShoulder, leftArmTarget, leftElbowTarget, true, leftElbowRotationOffset);

        if (rightShoulder && rightArmTarget && rightElbowTarget)
            UpdateElbow(rightShoulder, rightArmTarget, rightElbowTarget, false, rightElbowRotationOffset);
    }

    void FollowTarget(Transform target, Transform xrSource, Vector3 rotationOffset, Vector3 positionOffset, bool mirror)
    {
        // --- Position follow ---
        target.position = Vector3.Lerp(
            target.position,
            xrSource.position + xrSource.TransformDirection(positionOffset),
            Time.deltaTime * followSpeed
        );

        // --- Base rotation (optionally mirrored) ---
        Quaternion baseRotation = xrSource.rotation;
        if (mirror)
            baseRotation *= Quaternion.Euler(0f, 180f, 0f);

        // --- Apply custom rotation offset ---
        Quaternion targetRot = baseRotation * Quaternion.Euler(rotationOffset);

        // --- Smooth rotation ---
        target.rotation = Quaternion.Slerp(
            target.rotation,
            targetRot,
            Time.deltaTime * followSpeed
        );
    }

    void UpdateElbow(Transform shoulder, Transform hand, Transform elbowTarget, bool isLeft, Vector3 rotationOffset)
    {
        Vector3 shoulderToHand = hand.position - shoulder.position;
        float armLength = shoulderToHand.magnitude;

        // Find a stable "side" direction based on player body orientation (Y = up)
        Vector3 worldUp = Vector3.up;
        Vector3 sideDir = Vector3.Cross(worldUp, shoulderToHand).normalized;

        // Flip for left arm
        if (isLeft)
            sideDir = -sideDir;

        // Calculate the elbow’s base midpoint
        Vector3 midPoint = shoulder.position + shoulderToHand * elbowBendRatio;

        // Offset outward and slightly downward
        Vector3 elbowPos = midPoint + sideDir * elbowOutwardOffset + worldUp * elbowDownOffset * armLength;

        // Smooth position
        elbowTarget.position = Vector3.Lerp(elbowTarget.position, elbowPos, Time.deltaTime * followSpeed);

        // Rotate elbow to face the hand
        Quaternion elbowRot = Quaternion.LookRotation(hand.position - elbowTarget.position, worldUp) * Quaternion.Euler(rotationOffset);
        elbowTarget.rotation = Quaternion.Slerp(elbowTarget.rotation, elbowRot, Time.deltaTime * followSpeed);
    }
}

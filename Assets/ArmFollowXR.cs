using UnityEngine;

public class ArmFollowXR : MonoBehaviour
{
    [Header("XR Hand Targets")]
    public Transform leftHandXR;
    public Transform rightHandXR;

    [Header("XR Elbow Targets (Optional)")]
    public Transform leftElbowXR;
    public Transform rightElbowXR;

    [Header("Animated Arm Targets (Red Squares)")]
    public Transform leftArmTarget;
    public Transform rightArmTarget;

    [Header("Animated Elbow Targets (Blue Squares)")]
    public Transform leftElbowTarget;
    public Transform rightElbowTarget;

    [Header("Follow Settings")]
    public float followSpeed = 10f;
    public Vector3 handPositionOffset;
    public Vector3 elbowPositionOffset;

    [Header("Rotation Offsets")]
    public Vector3 leftHandRotationOffset;
    public Vector3 rightHandRotationOffset;
    public Vector3 leftElbowRotationOffset;
    public Vector3 rightElbowRotationOffset;

    [Header("Mirroring Options")]
    public bool mirrorLeftHand = false;
    public bool mirrorRightHand = false;
    public bool mirrorLeftElbow = false;
    public bool mirrorRightElbow = false;

    void Update()
    {
        // --- HANDS ---
        if (leftHandXR && leftArmTarget)
            FollowTarget(leftArmTarget, leftHandXR, leftHandRotationOffset, handPositionOffset, mirrorLeftHand);

        if (rightHandXR && rightArmTarget)
            FollowTarget(rightArmTarget, rightHandXR, rightHandRotationOffset, handPositionOffset, mirrorRightHand);

        // --- ELBOWS ---
        if (leftElbowXR && leftElbowTarget)
            FollowTarget(leftElbowTarget, leftElbowXR, leftElbowRotationOffset, elbowPositionOffset, mirrorLeftElbow);

        if (rightElbowXR && rightElbowTarget)
            FollowTarget(rightElbowTarget, rightElbowXR, rightElbowRotationOffset, elbowPositionOffset, mirrorRightElbow);
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
        {
            // Mirror across local X axis (adjust if needed)
            baseRotation *= Quaternion.Euler(0f, 180f, 0f);
        }

        // --- Apply custom rotation offset ---
        Quaternion targetRot = baseRotation * Quaternion.Euler(rotationOffset);

        // --- Smooth rotation ---
        target.rotation = Quaternion.Slerp(
            target.rotation,
            targetRot,
            Time.deltaTime * followSpeed
        );
    }
}

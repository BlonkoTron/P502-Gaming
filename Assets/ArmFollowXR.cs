using UnityEngine;

public class ArmFollowXR : MonoBehaviour
{
    [Header("XR Hand Targets")]
    public Transform leftHandXR;
    public Transform rightHandXR;

    [Header("Animated Arm Targets (Red Squares)")]
    public Transform leftArmTarget;
    public Transform rightArmTarget;

    [Header("Follow Settings")]
    public float followSpeed = 10f;
    public Vector3 positionOffset;

    [Header("Rotation Offsets")]
    public Vector3 leftRotationOffset;
    public Vector3 rightRotationOffset;

    [Header("Mirroring Options")]
    public bool mirrorLeftHand = false;
    public bool mirrorRightHand = false;

    void Update()
    {
        if (leftHandXR && leftArmTarget)
        {
            FollowTarget(leftArmTarget, leftHandXR, leftRotationOffset, mirrorLeftHand);
        }

        if (rightHandXR && rightArmTarget)
        {
            FollowTarget(rightArmTarget, rightHandXR, rightRotationOffset, mirrorRightHand);
        }
    }

    void FollowTarget(Transform armTarget, Transform xrHand, Vector3 rotationOffset, bool mirror)
    {
        // Position follow
        armTarget.position = Vector3.Lerp(
            armTarget.position,
            xrHand.position + xrHand.TransformDirection(positionOffset),
            Time.deltaTime * followSpeed
        );

        // Base rotation (optionally mirrored)
        Quaternion baseRotation = xrHand.rotation;

        if (mirror)
        {
            // Mirror across local X axis (can change to suit your rig)
            baseRotation *= Quaternion.Euler(0f, 180f, 0f);
        }

        // Apply custom rotation offset
        Quaternion targetRot = baseRotation * Quaternion.Euler(rotationOffset);

        // Smooth rotation
        armTarget.rotation = Quaternion.Slerp(
            armTarget.rotation,
            targetRot,
            Time.deltaTime * followSpeed
        );
    }
}

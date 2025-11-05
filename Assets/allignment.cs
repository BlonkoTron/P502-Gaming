using UnityEngine;
using UnityEngine.Animations.Rigging;

public class VRArmIKController : MonoBehaviour
{
    [Header("IK Constraints")]
    public TwoBoneIKConstraint leftArmIK;
    public TwoBoneIKConstraint rightArmIK;

    [Header("XR Hand Targets")]
    public Transform leftHandTarget;
    public Transform rightHandTarget;

    [Header("Elbow (Hint) Targets")]
    public Transform leftElbowHint;
    public Transform rightElbowHint;

    [Header("Rotation Offsets")]
    public Vector3 leftHandRotationOffset = Vector3.zero;
    public Vector3 rightHandRotationOffset = Vector3.zero;

    [Header("Mirror Fix (180° flip)")]
    public bool mirrorLeftHand = false;
    public bool mirrorRightHand = false;

    [Header("Settings")]
    public bool autoEnableIK = true;

    [Header("Character Arm Joints")]
    public Transform leftShoulder;
    public Transform rightShoulder;
    public Transform leftElbow;
    public Transform rightElbow;
    public float armLengthMultiplier = 1.0f;

    [Header("Head Tracking")]
    public Transform vrCamera;       // Your XR Camera (headset)
    public Transform headBone;       // The avatar's head bone
    public Vector3 headOffset = Vector3.zero;
    public bool followHeadPosition = true;
    public bool followHeadRotation = true;

    [Header("Spine Tracking")]
    public Transform spineBottom;    // e.g. hips or pelvis
    public Transform spineTop;       // e.g. upper spine or chest bone
    public bool followSpine = true;
    public float spineFollowSpeed = 5f;

    private float leftArmLength;
    private float rightArmLength;

    void Start()
    {
        // Precalculate character arm lengths
        if (leftShoulder && leftElbow && leftArmIK)
        {
            leftArmLength = (leftElbow.position - leftShoulder.position).magnitude +
                            (leftArmIK.data.tip.position - leftElbow.position).magnitude;
        }
        if (rightShoulder && rightElbow && rightArmIK)
        {
            rightArmLength = (rightElbow.position - rightShoulder.position).magnitude +
                             (rightArmIK.data.tip.position - rightElbow.position).magnitude;
        }
    }

    void Update()
    {
        // Arm IK Updates
        UpdateArmIK(leftArmIK, leftHandTarget, leftElbowHint, leftShoulder, leftArmLength, leftHandRotationOffset, mirrorLeftHand);
        UpdateArmIK(rightArmIK, rightHandTarget, rightElbowHint, rightShoulder, rightArmLength, rightHandRotationOffset, mirrorRightHand);

        // Head & Spine Updates
        UpdateHeadTracking();
        UpdateSpineFollow();
    }

    void UpdateArmIK(TwoBoneIKConstraint ik, Transform handTarget, Transform elbowHint, Transform shoulder, float armLength, Vector3 rotationOffset, bool mirror)
    {
        if (ik == null || handTarget == null || shoulder == null) return;

        var data = ik.data;

        // Calculate target position clamped to arm length
        Vector3 shoulderToHand = handTarget.position - shoulder.position;
        float dist = shoulderToHand.magnitude;
        Vector3 clampedPos = shoulder.position + shoulderToHand.normalized * Mathf.Min(dist, armLength * armLengthMultiplier);

        // Set rotation
        Quaternion rot = handTarget.rotation * Quaternion.Euler(rotationOffset);
        if (mirror) rot *= Quaternion.Euler(0, 180f, 0);

        data.target.position = clampedPos;
        data.target.rotation = rot;

        // Elbow hint
        if (elbowHint != null)
            data.hint.position = elbowHint.position;

        if (autoEnableIK)
            ik.weight = 1f;
    }

    void UpdateHeadTracking()
    {
        if (vrCamera == null || headBone == null) return;

        if (followHeadPosition)
            headBone.position = vrCamera.position + headOffset;

        if (followHeadRotation)
            headBone.rotation = vrCamera.rotation;
    }

    void UpdateSpineFollow()
    {
        if (!followSpine || spineBottom == null || spineTop == null || headBone == null)
            return;

        // Get direction from bottom of spine to head
        Vector3 targetDirection = (headBone.position - spineBottom.position).normalized;

        // Smoothly rotate the spine top toward the head
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
        spineTop.rotation = Quaternion.Slerp(spineTop.rotation, targetRotation, Time.deltaTime * spineFollowSpeed);

        // Optionally move the spine top slightly toward the head for a natural bend
        spineTop.position = Vector3.Lerp(spineTop.position, headBone.position, Time.deltaTime * (spineFollowSpeed * 0.5f));
    }
}

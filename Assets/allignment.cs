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
    public Vector3 leftHandRotationOffset = new Vector3(0f, 0f, 0f);
    public Vector3 rightHandRotationOffset = new Vector3(0f, 0f, 0f);

    [Header("Mirror Fix (180° flip)")]
    public bool mirrorLeftHand = false;
    public bool mirrorRightHand = false;

    [Header("Settings")]
    public bool autoEnableIK = true;  // makes sure weight stays at 1

    void Update()
    {
        // LEFT ARM
        if (leftArmIK && leftHandTarget)
        {
            var data = leftArmIK.data;

            // Target position + rotation
            Quaternion rot = leftHandTarget.rotation * Quaternion.Euler(leftHandRotationOffset);
            if (mirrorLeftHand) rot *= Quaternion.Euler(0, 180f, 0);
            data.target.position = leftHandTarget.position;
            data.target.rotation = rot;

            // Hint (elbow)
            if (leftElbowHint != null)
                data.hint.position = leftElbowHint.position;

            // Enable IK weight if desired
            if (autoEnableIK)
                leftArmIK.weight = 1f;
        }

        // RIGHT ARM
        if (rightArmIK && rightHandTarget)
        {
            var data = rightArmIK.data;

            // Target position + rotation
            Quaternion rot = rightHandTarget.rotation * Quaternion.Euler(rightHandRotationOffset);
            if (mirrorRightHand) rot *= Quaternion.Euler(0, 180f, 0);
            data.target.position = rightHandTarget.position;
            data.target.rotation = rot;

            // Hint (elbow)
            if (rightElbowHint != null)
                data.hint.position = rightElbowHint.position;

            if (autoEnableIK)
                rightArmIK.weight = 1f;
        }
    }
}

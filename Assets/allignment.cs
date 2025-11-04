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

    [Header("Rotation Offsets")]
    public Vector3 leftHandRotationOffset = new Vector3(0f, 0f, 0f);
    public Vector3 rightHandRotationOffset = new Vector3(0f, 0f, 0f);

    [Header("Mirror Fix (180° flip)")]
    public bool mirrorLeftHand = false;
    public bool mirrorRightHand = false;

    void Update()
    {
        if (leftArmIK && leftHandTarget)
        {
            Quaternion rot = leftHandTarget.rotation * Quaternion.Euler(leftHandRotationOffset);
            if (mirrorLeftHand) rot *= Quaternion.Euler(0, 180f, 0); // flip across Y axis
            leftArmIK.data.target.position = leftHandTarget.position;
            leftArmIK.data.target.rotation = rot;
            leftArmIK.weight = 1f;
        }

        if (rightArmIK && rightHandTarget)
        {
            Quaternion rot = rightHandTarget.rotation * Quaternion.Euler(rightHandRotationOffset);
            if (mirrorRightHand) rot *= Quaternion.Euler(0, 180f, 0);
            rightArmIK.data.target.position = rightHandTarget.position;
            rightArmIK.data.target.rotation = rot;
            rightArmIK.weight = 1f;
        }
    }
}

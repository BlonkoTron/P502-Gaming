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

    void Update()
    {
        if (leftArmIK != null && leftHandTarget != null)
        {
            leftArmIK.data.target.position = leftHandTarget.position;
            leftArmIK.data.target.rotation = leftHandTarget.rotation;
            leftArmIK.weight = 1.0f; // ensure active
        }

        if (rightArmIK != null && rightHandTarget != null)
        {
            rightArmIK.data.target.position = rightHandTarget.position;
            rightArmIK.data.target.rotation = rightHandTarget.rotation;
            rightArmIK.weight = 1.0f;
        }
    }
}

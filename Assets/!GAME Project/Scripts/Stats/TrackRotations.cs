using UnityEngine;

public class TrackRotations : MonoBehaviour
{
    [SerializeField] private PlayerSetUp playerSetUp;
    [SerializeField] private PlayerStats playerStats;

    [SerializeField] private GameObject handJointLeft;
    [SerializeField] private GameObject handJointRight;

    private GameObject handJoint;

    private float angleDegrees;
    private bool isReset;

    private float rotationDown;
    private float rotationUp;


    private void Start()
    {
        if (playerSetUp.isRightArm)
        {
            handJoint = handJointRight;
        }
        else
        {
            handJoint = handJointLeft;
        }
        isReset = true;
        rotationDown = 190 - playerSetUp.rotationROMDown;
        rotationUp = 190 + playerSetUp.rotationROMUp;
    }

    private void FixedUpdate()
    {
        CalculateRotation();
        CheckRotation();
    }

    private void CalculateRotation()
    {
        angleDegrees = handJoint.transform.rotation.eulerAngles.z;
        //Debug.Log("Current Rotation Angle: " + angleDegrees);
    }

    private void CheckRotation()
    {
        if (angleDegrees <= rotationDown && isReset)
        {
            playerStats.nrOfRotationsDown += 1;
            Debug.Log("ROTATED DOWN! Total Rotations Down: " + playerStats.nrOfRotationsDown);
            isReset = false;
        }
        else if (angleDegrees >= rotationUp && isReset)
        {
            playerStats.nrOfRotationsUp += 1;
            Debug.Log("ROTATED UP! Total Rotations Up: " + playerStats.nrOfRotationsUp);
            isReset = false;
        }
        else if (angleDegrees > 180 && angleDegrees < 200)
        {
            isReset = true;
        }
    }
}

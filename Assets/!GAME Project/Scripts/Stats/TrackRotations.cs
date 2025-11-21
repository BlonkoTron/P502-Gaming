using UnityEngine;
using UnityEngine.SceneManagement;

public class TrackRotations : MonoBehaviour
{
    [SerializeField] private PlayerSetUp playerSetUp;
    [SerializeField] private PlayerStats playerStats;

    [SerializeField] private GameObject handJointLeft;
    [SerializeField] private GameObject handJointRight;

    private GameObject handJoint;

    public float angleDegrees;
    private bool isReset;

    private float rotationDown;
    private float rotationUp;

    private bool tracking;


    private void Start()
    {
        UnityEngine.SceneManagement.Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name == "MainGameScene")
        {
            tracking = false;
        }
        else
        {
            tracking = true;
        }

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
        if (!tracking) return;
        CheckRotation();
    }

    private void CalculateRotation()
    {
        angleDegrees = handJoint.transform.rotation.eulerAngles.z;
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

using UnityEngine;
using UnityEngine.SceneManagement;

public class TrackRotations : MonoBehaviour
{
    [SerializeField] private PlayerSetUp playerSetUp;
    [SerializeField] private PlayerStats playerStats;

    // Hand joint GameObjects for left and right arms
    [SerializeField] private GameObject handJointLeft;
    [SerializeField] private GameObject handJointRight;

    // Currently active hand joint (chosen based on arm side)
    private GameObject handJoint;

    // Current rotation angle of the hand joint in degrees
    public float angleDegrees;
    // Used to prevent counting the same rotation multiple times
    private bool isReset;

    // Rotation thresholds for detecting up and down movements
    private float rotationDown;
    private float rotationUp;

    // Determines whether rotation tracking is active
    private bool tracking;

    private void Start()
    {
        // Disable tracking in the Main Menu scene
        UnityEngine.SceneManagement.Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name == "MainMenu")
        {
            tracking = false;
        }
        else
        {
            tracking = true;
        }

        // Select the correct hand joint based on the chosen arm
        if (playerSetUp.isRightArm)
        {
            handJoint = handJointRight;
        }
        else
        {
            handJoint = handJointLeft;
        }

        // Calculate rotation limits based on player range of motion
        rotationDown = 190 - playerSetUp.rotationROMDown;
        rotationUp = 190 + playerSetUp.rotationROMUp;
    }

    // Updates arm settings when the player changes arm or ROM values
    public void UpdateArm()
    {
        // Update active hand joint
        if (playerSetUp.isRightArm)
        {
            handJoint = handJointRight;
        }
        else
        {
            handJoint = handJointLeft;
        }

        // Reset rotation tracking state
        isReset = true;

        // Update rotation limits from player setup
        rotationDown = playerSetUp.rotationROMDown;
        rotationUp = playerSetUp.rotationROMUp;
    }

    private void FixedUpdate()
    {
        // Calculate current rotation angle
        CalculateRotation();

        // Stop here if tracking is disabled
        if (!tracking)
        { return; }
        else
        {
            // Check if a rotation movement should be counted
            CheckRotation();
        }
    }

    // Reads the Z-axis rotation of the hand joint
    private void CalculateRotation()
    {
        angleDegrees = handJoint.transform.rotation.eulerAngles.z;
    }

    // Checks if the arm has rotated past defined thresholds
    private void CheckRotation()
    {
        // Detect downward rotation
        if (angleDegrees >= rotationDown && isReset)
        {
            playerStats.nrOfRotationsDown += 1;
            isReset = false;
        }
        // Detect upward rotation
        else if (angleDegrees <= rotationUp && isReset)
        {
            playerStats.nrOfRotationsUp += 1;
            isReset = false;
        }
        // Reset once the arm returns to a neutral zone
        else if (angleDegrees > 180 && angleDegrees < 200)
        {
            isReset = true;
        }
    }
}

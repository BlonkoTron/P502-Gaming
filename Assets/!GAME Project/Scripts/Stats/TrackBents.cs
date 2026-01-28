using UnityEngine.SceneManagement;
using UnityEngine;

public class TrackBents : MonoBehaviour
{
    [SerializeField] private PlayerSetUp playerSetUp;
    [SerializeField] private PlayerStats playerStats;

    // Joint references for the left arm
    [SerializeField] private GameObject shoulderJointLeft;
    [SerializeField] private GameObject elbowJointLeft;
    [SerializeField] private GameObject handJointLeft;

    // Joint references for the right arm
    [SerializeField] private GameObject shoulderJointRight;
    [SerializeField] private GameObject elbowJointRight;
    [SerializeField] private GameObject handJointRight;

    // Currently active joints (selected based on arm side)
    private GameObject shoulderJoint;
    private GameObject elbowJoint;
    private GameObject handJoint;

    // Distances between joints used for angle calculation
    // a = elbow - hand
    // b = shoulder - elbow
    // c = shoulder - hand
    private float aDistance;
    private float bDistance;
    private float cDistance;

    // Cosine result and final bend angle in degrees
    private double bent;
    public float angleDegrees;

    // Used to prevent counting the same bend multiple times
    private bool isReset;

    // Determines whether bend tracking is active
    private bool tracking;

    private void Start()
    {
        // Disable tracking in the Main Menu scene
        UnityEngine.SceneManagement.Scene currentScene = SceneManager.GetActiveScene();
        if(currentScene.name == "MainMenu")
        {
            tracking = false;
        }
        else
        {
            tracking = true;
        }

        // Select the correct joints based on the chosen arm
        if (playerSetUp.isRightArm != false)
        {
            shoulderJoint = shoulderJointRight;
            elbowJoint = elbowJointRight;
            handJoint = handJointRight;
        }
        else
        {
            shoulderJoint = shoulderJointLeft;
            elbowJoint = elbowJointLeft;
            handJoint = handJointLeft;
        }

        // Calculate fixed segment lengths (used in angle calculation)
        aDistance = Vector3.Distance(elbowJoint.transform.position, handJoint.transform.position);
        bDistance = Vector3.Distance(shoulderJoint.transform.position, elbowJoint.transform.position);

        // Allow first bend to be counted
        isReset = true;
    }

    // Updates joint references when the player switches arms
    public void UpdateArm()
    {
        // Update active joints based on arm selection
        if (playerSetUp.isRightArm != false)
        {
            shoulderJoint = shoulderJointRight;
            elbowJoint = elbowJointRight;
            handJoint = handJointRight;
        }
        else
        {
            shoulderJoint = shoulderJointLeft;
            elbowJoint = elbowJointLeft;
            handJoint = handJointLeft;
        }

        // Recalculate segment lengths
        aDistance = Vector3.Distance(elbowJoint.transform.position, handJoint.transform.position);
        bDistance = Vector3.Distance(shoulderJoint.transform.position, elbowJoint.transform.position);
    }

    private void FixedUpdate()
    {
        // Calculate the dynamic distance between shoulder and hand
        cDistance = Vector3.Distance(shoulderJoint.transform.position, handJoint.transform.position);

        // Calculate current elbow bend angle
        CalculateBent(cDistance, aDistance, bDistance);

        // Check whether a bend movement should be counted
        CheckBent();
    }

    // Calculates the elbow bend angle using the law of cosines
    public void CalculateBent(float c, float a, float b)
    {
        // cos(theta) = (a^2 + b^2 - c^2) / (2ab)
        bent = (Mathf.Pow(a,2)+Mathf.Pow(b,2)-Mathf.Pow(c,2))/(2.0*a*b);

        // Convert the angle from radians to degrees
        angleDegrees = Mathf.Acos((float)bent) * Mathf.Rad2Deg;
    }

    // Recalculates the bend angle using current distances
    public void UpdateAngleDegrees()
    {
        CalculateBent(cDistance, aDistance, bDistance);
    }

    // Checks whether the arm has bent in or out past ROM thresholds
    private void CheckBent()
    {
        if (tracking)
        {
            // Detect inward bend
            if (angleDegrees <= playerSetUp.bentROMIn && isReset)
            {
                playerStats.nrOfBentsIn += 1;
                isReset = false;
            }
            // Detect outward bend
            else if (angleDegrees >= playerSetUp.bentROMOut &&  isReset)
            {
                playerStats.nrOfBentsOut += 1;
                isReset = false;
            }
            // Reset once the arm returns to the neutral range
            else if (angleDegrees > playerSetUp.bentROMIn && angleDegrees < playerSetUp.bentROMOut)
            {
                isReset = true;
            }
        }
    }
}

using UnityEngine.SceneManagement;
using UnityEngine;

public class TrackBents : MonoBehaviour
{
    [SerializeField] private PlayerSetUp playerSetUp;
    [SerializeField] private PlayerStats playerStats;

    [SerializeField] private GameObject shoulderJointLeft;
    [SerializeField] private GameObject elbowJointLeft;
    [SerializeField] private GameObject handJointLeft;

    [SerializeField] private GameObject shoulderJointRight;
    [SerializeField] private GameObject elbowJointRight;
    [SerializeField] private GameObject handJointRight;

    private GameObject shoulderJoint;
    private GameObject elbowJoint;
    private GameObject handJoint;

    private float aDistance;
    private float bDistance;
    private float cDistance;

    private double bent;
    public float angleDegrees;
    private bool isReset;
    private bool tracking;

    private void Start()
    {
        UnityEngine.SceneManagement.Scene currentScene = SceneManager.GetActiveScene();
        if(currentScene.name == "MainMenu")
        {
            tracking = false;
        }
        else
        {
            tracking = true;
        }

        if (playerSetUp.isRightArm == false)
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

        aDistance = Vector3.Distance(elbowJoint.transform.position, handJoint.transform.position);
        bDistance = Vector3.Distance(shoulderJoint.transform.position, elbowJoint.transform.position);

        isReset = true;
    }


    private void FixedUpdate()
    {
        cDistance = Vector3.Distance(shoulderJoint.transform.position, handJoint.transform.position);
        CalculateBent(cDistance, aDistance, bDistance);
        //Debug.Log("Bent Angle: " + angleDegrees);
        CheckBent();
    }


    public void CalculateBent(float c, float a, float b)
    {
        bent = (Mathf.Pow(a,2)+Mathf.Pow(b,2)-Mathf.Pow(c,2))/(2.0*a*b);
        angleDegrees = Mathf.Acos((float)bent) * Mathf.Rad2Deg;
    }

    public void UpdateAngleDegrees()
    {
        CalculateBent(cDistance, aDistance, bDistance);
    }

    private void CheckBent()
    {
        if (tracking)
        {
            if (angleDegrees <= playerSetUp.bentROMIn && isReset)
            {
                playerStats.nrOfBentsIn += 1;
                //Debug.Log("BENT IN! Total Bents In: " + playerStats.nrOfBentsIn);
                isReset = false;
            }
            else if (angleDegrees >= playerSetUp.bentROMOut &&  isReset)
            {
                playerStats.nrOfBentsOut += 1;
                //Debug.Log("BENT OUT! Total Bents Out: " + playerStats.nrOfBentsOut);
                isReset = false;
            }
            else if (angleDegrees > playerSetUp.bentROMIn && angleDegrees < playerSetUp.bentROMOut)
            {
                isReset = true;
            }
        }
       
    }

}

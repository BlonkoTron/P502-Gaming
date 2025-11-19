using UnityEngine;

public class TrackBents : MonoBehaviour
{
    [SerializeField] private PlayerSetUp playerSetUp;

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

    private void Start()
    {
        if (playerSetUp.isRightArm)
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
    }


    private void Update()
    {
        cDistance = Vector3.Distance(shoulderJoint.transform.position, handJoint.transform.position);
        CalculateBent(cDistance, aDistance, bDistance);
    }



    private void CalculateBent(float c, float a, float b)
    {
        double cosC = (Mathf.Pow(a,2)+Mathf.Pow(b,2)-Mathf.Pow(c,2))/(2.0*a*b);
    }

}

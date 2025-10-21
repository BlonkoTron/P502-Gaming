using UnityEngine;

public class Hinge_trigger : MonoBehaviour
{
    public HingeJoint hinge;           // Assign in Inspector
    public float pullAngleThreshold = 40; // Angle to trigger (degrees)
    public bool hasRung = false;       // Prevent multiple triggers
    public float currentang;
    void Start()
    {
        if (hinge == null)
            hinge = GetComponent<HingeJoint>();
    }

    void Update()
    {
        currentang = Mathf.Abs(hinge.angle); // Absolute value in case of negatives

        if (!hasRung && currentang >= pullAngleThreshold)
        {
            hasRung = true;
            RingBell();
        }

        // Optional: reset trigger if rope goes back up
        if (hasRung && currentang < pullAngleThreshold - 10f)
        {
            hasRung = false;
        }
    }

    void RingBell()
    {
        Debug.Log("Rope pull");
    }
}

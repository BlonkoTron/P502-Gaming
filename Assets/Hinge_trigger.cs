using UnityEngine;

public class Hinge_trigger : MonoBehaviour
{
    public HingeJoint hinge;           // Assign in Inspector
    public float pullAngleThreshold = 40; // Angle to trigger (degrees)
    public bool hasRung = false;       // Prevent multiple triggers

    void Start()
    {
        if (hinge == null)
            hinge = GetComponent<HingeJoint>();
    }

    void Update()
    {
        float currentAngle = Mathf.Abs(hinge.angle); // Absolute value in case of negatives

        if (!hasRung && currentAngle >= pullAngleThreshold)
        {
            hasRung = true;
            RingBell();
        }

        // Optional: reset trigger if rope goes back up
        if (hasRung && currentAngle < pullAngleThreshold - 10f)
        {
            hasRung = false;
        }
    }

    void RingBell()
    {
        Debug.Log("🔔 Bell triggered!");
    }
}

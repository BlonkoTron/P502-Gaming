using UnityEngine;

public class Orbit : MonoBehaviour
{
    // The GameObject this object will orbit around. Set this in the Inspector.
    [Tooltip("The object to orbit around (e.g., a central planet).")]
    public Transform target;

    // The speed of the orbit, in degrees per second.
    [Tooltip("The speed of the orbit, in degrees per second.")]
    public float orbitSpeed = 50f;

    // The axis to orbit around (e.g., Vector3.up for spinning horizontally).
    [Tooltip("The axis to spin around relative to the target's position.")]
    public Vector3 orbitAxis = Vector3.up;

    // Update is called once per frame
    void Update()
    {
        // 1. Check if a target has been assigned.
        if (target != null)
        {
            // 2. Calculate the rotation angle for this frame.
            float angle = orbitSpeed * Time.deltaTime;

            // 3. Perform the orbit rotation.
            // RotateAround takes three arguments:
            //   - The point to rotate around (target.position).
            //   - The axis of rotation (orbitAxis).
            //   - The angle in degrees for this frame (angle).
            transform.RotateAround(target.position, orbitAxis, angle);
        }
    }
}
using UnityEngine;

public class PlanetSpin : MonoBehaviour
{
    // A public variable to control the spin speed from the Inspector.
    // The [Tooltip] provides helpful text when hovering over the variable in the Inspector.
    [Tooltip("The speed at which the object will spin, in degrees per second.")]
    public float spinSpeed = 10f;

    // Update is called once per frame
    void Update()
    {
        // Calculate the rotation amount for this frame.
        // Time.deltaTime ensures the rotation is frame-rate independent.
        float rotationAmount = spinSpeed * Time.deltaTime;

        // Apply the rotation to the GameObject.
        // We are rotating around the Z-axis here. You can change this to 
        // Vector3.up (Y-axis) or Vector3.right (X-axis) for different spin directions.
        transform.Rotate(Vector3.up, rotationAmount);
    }
}
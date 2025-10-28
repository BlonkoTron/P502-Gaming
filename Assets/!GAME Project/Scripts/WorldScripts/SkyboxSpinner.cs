using UnityEngine;

public class SkyboxSpinner : MonoBehaviour
{
    // Public variable to control the rotation speed from the Inspector.
    [Tooltip("The speed at which the skybox will spin, in degrees per second.")]
    public float spinSpeed = 1f;

    // The name of the property in the Skybox material that controls rotation.
    // Standard Unity Skybox shaders (like 6 Sided, Procedural, etc.) use this.
    private const string RotationProperty = "_Rotation";

    // Private variable to track the current rotation value.
    private float currentRotation = 0f;

    void Update()
    {
        // 1. Calculate the new rotation amount.
        // Multiply by Time.deltaTime for frame-rate independence.
        currentRotation += spinSpeed * Time.deltaTime;

        // Keep the rotation value within 0-360 degrees to prevent overflow
        // and large floating-point numbers, though not strictly required.
        currentRotation %= 360f;

        // 2. Apply the rotation to the active Skybox material.
        // RenderSettings.skybox is the material currently used by the scene's skybox.
        if (RenderSettings.skybox != null)
        {
            RenderSettings.skybox.SetFloat(RotationProperty, currentRotation);
        }
        else
        {
            // Optional: Log a warning if no skybox is assigned.
            Debug.LogWarning("No Skybox material is assigned in RenderSettings. To spin the skybox, ensure one is set.");
        }
    }
}
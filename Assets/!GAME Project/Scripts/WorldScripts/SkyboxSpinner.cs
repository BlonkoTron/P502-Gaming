using UnityEngine;

public class SkyboxSpinner : MonoBehaviour
{
    [Tooltip("The speed at which the skybox will spin, in degrees per second.")]
    public float spinSpeed = 1f;

    private const string RotationProperty = "_Rotation";
    private float currentRotation = 0f;
    private Material skyboxMaterial;

    // A private variable to store the original rotation when the game starts.
    private float initialRotation = 0f;

    void Start()
    {
        // Cache the active Skybox material from the RenderSettings.
        skyboxMaterial = RenderSettings.skybox;

        if (skyboxMaterial != null)
        {
            // Store the rotation value as it was when the game launched.
            initialRotation = skyboxMaterial.GetFloat(RotationProperty);
            currentRotation = initialRotation;
        }
        else
        {
            Debug.LogError("SkyboxSpinner is active but no Skybox material is assigned in RenderSettings.");
            enabled = false; // Disable the script if no skybox is found.
        }
    }

    void Update()
    {
        // Only spin if the material exists.
        if (skyboxMaterial != null)
        {
            // Calculate the new rotation amount and keep it within 0-360 degrees.
            currentRotation += spinSpeed * Time.deltaTime;
            currentRotation %= 360f;

            // Apply the rotation.
            skyboxMaterial.SetFloat(RotationProperty, currentRotation);
        }
    }

    // Called when the application quits or when you stop playing in the editor.
    private void OnApplicationQuit()
    {
        // Check if we found the material and if its current rotation is different
        // from the starting rotation (i.e., we actually spun it).
        if (skyboxMaterial != null)
        {
            // **Crucial Step:** Reset the rotation property to its initial value.
            skyboxMaterial.SetFloat(RotationProperty, initialRotation);

            // Optional: Re-save the settings if you want to be extra safe, though
            // setting the float value is usually enough for the editor to update.
#if UNITY_EDITOR
            // This is only needed if you want the *asset* to be permanently changed, 
            // but we usually just want the runtime setting reset.
            // UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(gameObject.scene);
#endif
        }
    }
}
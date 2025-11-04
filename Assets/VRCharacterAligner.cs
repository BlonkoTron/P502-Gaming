using UnityEngine;

public class VRCharacterAlignerAdvanced : MonoBehaviour
{
    [Header("XR References")]
    public Transform xrOrigin;      // XR Origin or Camera Rig
    public Transform xrHead;        // Main Camera (VR headset)

    [Header("Character References")]
    public Transform characterRoot; // Root of the character (hips or full body root)
    public Transform characterHead; // Head bone or head transform

    [Header("Offsets")]
    public Vector3 positionOffset;   // Manual fine-tune offset
    public Vector3 rotationOffset;   // Yaw rotation adjustment (in degrees)
    [Tooltip("Use this to adjust how high or low the model appears relative to the XR rig.")]
    public float heightOffset = 0f;  // Y offset in meters

    [Header("Scale Settings")]
    [Tooltip("Uniformly scales the character to better match player height.")]
    [Range(0.5f, 2f)]
    public float characterScale = 1.0f; // Global scale multiplier

    [Header("Alignment Options")]
    public bool alignOnStart = true;
    public bool alignRotationToXR = true;
    public bool keepFeetOnGround = true;

    void Start()
    {
        if (alignOnStart)
            AlignCharacterWithXR();
    }

    [ContextMenu("Align Character Now")]
    public void AlignCharacterWithXR()
    {
        if (!xrOrigin || !xrHead || !characterRoot || !characterHead)
        {
            Debug.LogWarning("❌ VRCharacterAlignerAdvanced: Missing references!");
            return;
        }

        // Step 1: Apply scale
        characterRoot.localScale = Vector3.one * characterScale;

        // Step 2: Move character root to XR origin position + offset
        characterRoot.position = xrOrigin.position + positionOffset;

        // Step 3: Rotate character to match XR orientation (only Y-axis)
        if (alignRotationToXR)
        {
            Vector3 xrForward = xrOrigin.forward;
            xrForward.y = 0;
            Quaternion flatRotation = Quaternion.LookRotation(xrForward);
            characterRoot.rotation = flatRotation * Quaternion.Euler(rotationOffset);
        }

        // Step 4: Align character head to XR headset
        Vector3 headOffset = xrHead.position - characterHead.position;
        characterRoot.position += headOffset;

        // Step 5: Apply manual height adjustment
        characterRoot.position += Vector3.up * heightOffset;

        // Step 6: Keep character feet on ground (optional)
        if (keepFeetOnGround)
        {
            characterRoot.position = new Vector3(
                characterRoot.position.x,
                xrOrigin.position.y,
                characterRoot.position.z
            );
        }

        Debug.Log($"✅ Character aligned and scaled. Scale: {characterScale}, Height offset: {heightOffset}");
    }
}

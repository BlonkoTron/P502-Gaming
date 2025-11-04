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
    public Vector3 positionOffset;  // Manual fine-tune offset for character placement
    public Vector3 rotationOffset;  // Yaw rotation adjustment (in degrees)

    [Header("Alignment Options")]
    public bool alignOnStart = true;
    public bool alignRotationToXR = true;

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

        // Step 1: Move character root to XR origin position
        characterRoot.position = xrOrigin.position + positionOffset;

        // Step 2: Rotate character to match XR orientation (only Y-axis)
        if (alignRotationToXR)
        {
            Vector3 xrForward = xrOrigin.forward;
            xrForward.y = 0;
            Quaternion flatRotation = Quaternion.LookRotation(xrForward);
            characterRoot.rotation = flatRotation * Quaternion.Euler(rotationOffset);
        }

        // Step 3: Align character head to XR headset
        Vector3 headOffset = xrHead.position - characterHead.position;
        characterRoot.position += headOffset;

        // Optional: Keep character on ground plane (don’t lift feet off floor)
        characterRoot.position = new Vector3(
            characterRoot.position.x,
            xrOrigin.position.y,
            characterRoot.position.z
        );

        Debug.Log("✅ Character aligned with XR rig successfully!");
    }
}

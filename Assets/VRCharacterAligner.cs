using UnityEngine;

public class VRCharacterAligner : MonoBehaviour
{
    [Header("References")]
    public Transform xrRig;          // XR Origin or Camera Rig root
    public Transform xrHead;         // The VR camera (head)
    public Transform characterRoot;  // The character model root (hips or main transform)

    [Header("Offsets")]
    public Vector3 positionOffset;   // Manual adjustment for fine-tuning
    public Vector3 rotationOffset;   // Manual rotation offset (in degrees)

    [Header("Auto Align")]
    public bool alignOnStart = true; // Automatically align when pressing Play

    void Start()
    {
        if (alignOnStart)
            AlignCharacterWithXR();
    }

    [ContextMenu("Align Character Now")]
    public void AlignCharacterWithXR()
    {
        if (xrRig == null || xrHead == null || characterRoot == null)
        {
            Debug.LogWarning("Missing references in VRCharacterAligner!");
            return;
        }

        // Calculate head-level position difference
        Vector3 headPosition = xrHead.position;
        Vector3 characterPosition = characterRoot.position;

        // Align character hips roughly to XR Rig position
        Vector3 offset = headPosition - characterPosition;
        offset.y = 0; // Keep character feet on ground level

        characterRoot.position += offset + positionOffset;

        // Apply rotation offset (useful if character faces wrong direction)
        characterRoot.rotation = Quaternion.Euler(rotationOffset) * Quaternion.Euler(0, xrRig.eulerAngles.y, 0);
    }
}

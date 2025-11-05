using UnityEngine;

public class XROriginAlignToCharacter : MonoBehaviour
{
    [Header("XR References")]
    public Transform xrOrigin;  // XR Rig Root
    public Transform xrHead;    // XR Camera (player head in XR rig)

    [Header("Character Reference")]
    public Transform characterHead; // Character’s head/neck bone

    [Header("Options")]
    public bool alignOnStart = true;
    public bool continuousAlignment = false; // keep aligning each frame

    private void Start()
    {
        if (alignOnStart)
            AlignOriginToCharacterHead();
    }

    private void LateUpdate()
    {
        // Continuously keep camera inside character’s head
        if (continuousAlignment)
            AlignOriginToCharacterHead();
    }

    [ContextMenu("Align Now")]
    public void AlignOriginToCharacterHead()
    {
        if (!xrOrigin || !xrHead || !characterHead)
        {
            Debug.LogWarning("⚠️ Missing reference in XROriginAlignToCharacter!");
            return;
        }

        // --- POSITION ALIGNMENT ---
        Vector3 headOffset = xrHead.position - xrOrigin.position;
        xrOrigin.position = characterHead.position - headOffset;

        // --- ROTATION ALIGNMENT ---
        // Rotate the XR Origin so that the XR head forward matches the character's head forward
        Quaternion headRotationOffset = Quaternion.Inverse(xrHead.rotation) * xrOrigin.rotation;
        xrOrigin.rotation = characterHead.rotation * headRotationOffset;
    }
}

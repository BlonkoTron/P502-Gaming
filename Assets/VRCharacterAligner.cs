using UnityEngine;

public class VRCharacterAligner : MonoBehaviour
{
    public Transform xrOrigin;   // XR Rig Root
    public Transform xrHead;     // XR Camera (player head)

    public Transform characterRoot; // Avatar root at hips or pelvis
    public Transform characterHead; // Avatar head/neck bone

    [Range(0.5f, 2.0f)]
    public float characterScale = 1.0f;

    public bool keepFeetOnGround = true;
    public bool followContinuously = false;

    private float headHeightOffset;

    private void Start()
    {
        if (!ValidRefs()) return;

        // Store the avatar’s original head height from root
        headHeightOffset = characterHead.position.y - characterRoot.position.y;

        Align();
    }

    private void Update()
    {
        if (followContinuously)
            Align();
    }

    void Align()
    {
        if (!ValidRefs()) return;

        // ✅ 1. Scale avatar around root BEFORE aligning
        characterRoot.localScale = Vector3.one * characterScale;

        // ✅ 2. Rotate avatar to match XR direction (Y only)
        Vector3 forward = xrHead.forward;
        forward.y = 0f;
        characterRoot.rotation = Quaternion.LookRotation(forward);

        // ✅ 3. Position avatar so character head = XR head
        Vector3 targetPosition = xrHead.position - (characterHead.position - characterRoot.position);
        characterRoot.position = targetPosition;

        // ✅ 4. Keep feet grounded (optional)
        if (keepFeetOnGround)
        {
            characterRoot.position = new Vector3(
                characterRoot.position.x,
                xrOrigin.position.y - headHeightOffset,
                characterRoot.position.z
            );
        }
    }

    bool ValidRefs()
    {
        if (!xrOrigin || !xrHead || !characterRoot || !characterHead)
        {
            Debug.LogWarning("⚠️ Missing reference in VRCharacterAligner!");
            return false;
        }
        return true;
    }
}

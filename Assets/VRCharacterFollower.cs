using UnityEngine;

public class VRCharacterFollower : MonoBehaviour
{
    [Header("References")]
    public Transform xrOrigin;       // Your XR Origin or Camera Rig
    public Transform characterRoot;  // Usually hips or the root of your model

    [Header("Offsets")]
    public Vector3 positionOffset;   // Fine-tune offset
    public Vector3 rotationOffset;   // Optional rotation tweak
    public bool followRotation = true;

    void LateUpdate()
    {
        if (!xrOrigin || !characterRoot) return;

        // Follow XR Origin position
        characterRoot.position = xrOrigin.position + positionOffset;

        // Optionally follow rotation
        if (followRotation)
        {
            Quaternion xrRotation = Quaternion.Euler(0, xrOrigin.eulerAngles.y, 0);
            characterRoot.rotation = xrRotation * Quaternion.Euler(rotationOffset);
        }
    }
}

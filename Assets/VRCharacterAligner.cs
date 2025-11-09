using UnityEngine;

public class FollowXRPlayer : MonoBehaviour
{
    [Header("XR Rig or Player Root")]
    [Tooltip("The root object of your XR rig (e.g., XR Origin).")]
    public Transform xrRig;
    public Transform Camoffset;
    public Transform Headpos;

    [Header("Follow Settings")]
    [Tooltip("How quickly the model follows the XR player position.")]
    public float positionSmoothSpeed = 5f;
    [Tooltip("How quickly the model follows the XR player rotation.")]
    public float rotationSmoothSpeed = 5f;

    public void Start()
    {
        Camoffset.position = Headpos.position;
    }
    private void LateUpdate()
    {
        
        if (xrRig == null) return;

        // Smooth follow position
        transform.position = Vector3.Lerp(
            transform.position,
            xrRig.position,
            Time.deltaTime * positionSmoothSpeed
        );

        // Smooth follow rotation
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            xrRig.rotation,
            Time.deltaTime * rotationSmoothSpeed
        );
    }
}

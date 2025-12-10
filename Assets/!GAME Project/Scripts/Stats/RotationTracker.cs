using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR;

public class RotationTracker : MonoBehaviour
{
    public TrackedPoseDriver tpdLeftUp;
    public TrackedPoseDriver tpdLeftDown;
    public TrackedPoseDriver tpdRightUp;
    public TrackedPoseDriver tpdRightDown;



    public void ToggleTPD(TrackedPoseDriver tpd)
    {
        if (tpd.trackingType == TrackedPoseDriver.TrackingType.PositionOnly)
        {
            tpd.trackingType = TrackedPoseDriver.TrackingType.RotationAndPosition;
        }     
        else
        {
            tpd.trackingType = TrackedPoseDriver.TrackingType.PositionOnly;
        }
    }
}

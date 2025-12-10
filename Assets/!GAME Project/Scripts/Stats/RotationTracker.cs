using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public class RotationTracker : MonoBehaviour
{
    [HideInInspector] public TrackedPoseDriver tpdLeftUp;
    [HideInInspector] public TrackedPoseDriver tpdLeftDown;
    [HideInInspector] public TrackedPoseDriver tpdRightUp;
    [HideInInspector] public TrackedPoseDriver tpdRightDown;

    [SerializeField] private PlayerSetUp playerSetUp;

    public GameObject LeftUp;
    public GameObject LeftDown;
    public GameObject RightUp;
    public GameObject RightDown;

    public GameObject LeftTrackerCube;
    public GameObject RightTrackerCube;

    private void Start()
    {
        tpdLeftUp = LeftUp.GetComponent<TrackedPoseDriver>();
        tpdLeftDown = LeftDown.GetComponent<TrackedPoseDriver>();
        tpdRightUp = RightUp.GetComponent<TrackedPoseDriver>();
        tpdRightDown = RightDown.GetComponent<TrackedPoseDriver>();

        UnityEngine.SceneManagement.Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name == "MainMenu")
        {
            
        }
        else
        {
           
            SetRotations();
            EnableRotationCollider();
        }
    }

    private void SetRotations()
    {
        if (playerSetUp.isRightArm)
        {
            RightUp.transform.localRotation = playerSetUp.rotationUp;
            RightDown.transform.localRotation = playerSetUp.rotationDown;
        }
        else
        {
            LeftUp.transform.localRotation = playerSetUp.rotationUp;
            LeftDown.transform.localRotation = playerSetUp.rotationDown;
        }
    }

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

    public void EnableRotationCollider()
    {
       if (playerSetUp.isRightArm)
       {
           RightTrackerCube.GetComponent<RotationCollider>().enabled = true;
       }
       else
       {
           LeftTrackerCube.GetComponent<RotationCollider>().enabled = true;
       }
    }
}

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

    public bool lockRotation;

    private void Start()
    {
        tpdLeftUp = LeftUp.GetComponent<TrackedPoseDriver>();
        tpdLeftDown = LeftDown.GetComponent<TrackedPoseDriver>();
        tpdRightUp = RightUp.GetComponent<TrackedPoseDriver>();
        tpdRightDown = RightDown.GetComponent<TrackedPoseDriver>();

        ToggleVisibility(false);

        UnityEngine.SceneManagement.Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name == "MainMenu")
        {
            lockRotation = false;
        }
        else
        {
            ToggleTPD(tpdLeftUp);
            ToggleTPD(tpdLeftDown);
            ToggleTPD(tpdRightUp);
            ToggleTPD(tpdRightDown);
            SetRotations();
            EnableRotationCollider();
            lockRotation = true;
        }
    }

    public void ToggleLock()
    {
        lockRotation = !lockRotation;
    }

    void LateUpdate()
    {
        if (!lockRotation) return;

        if (playerSetUp.isRightArm)
        {
            LockRotation(RightUp,playerSetUp.rotationUp);
            LockRotation(RightDown, playerSetUp.rotationUp);
        }
        else
        {
            LockRotation(LeftUp, playerSetUp.rotationUp);
            LockRotation(LeftDown, playerSetUp.rotationUp);
        }

    }

    public void LockRotation(GameObject GO, Quaternion qua)
    {
       // Controller rotation after TPD has applied tracking
        Vector3 r = GO.transform.eulerAngles;

        // Example: keep yaw, lock pitch and roll
        r.z = qua.eulerAngles.z;
        r.y = qua.eulerAngles.y;

        GO.transform.rotation = Quaternion.Euler(r);
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

    public void ToggleVisibility(bool on)
    {
        if (playerSetUp.isRightArm)
        {
            LeftUp.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().enabled = false;
            LeftDown.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().enabled = false;
            LeftTrackerCube.GetComponent<MeshRenderer>().enabled = false;

            RightUp.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().enabled = on;
            RightDown.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().enabled = on;
            RightTrackerCube.GetComponent<MeshRenderer>().enabled = on;
        }
        else
        {
            LeftUp.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().enabled = on;
            LeftDown.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().enabled = on;
            LeftTrackerCube.GetComponent<MeshRenderer>().enabled = on;

            RightUp.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().enabled = false;
            RightDown.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().enabled = false;
            RightTrackerCube.GetComponent<MeshRenderer>().enabled = false;
        }
    }

}

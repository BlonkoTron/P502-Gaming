using UnityEngine;

public class ArmSwitcher : MonoBehaviour
{
    [Header("Settings")]
    public PlayerSetUp playerSetUp;

    [Header("Objects")]
    public GameObject[] objectsActiveWhenTrue;
    public GameObject[] objectsActiveWhenFalse;

    [Header("Components")]
    public Behaviour[] componentsActiveWhenTrue;
    public Behaviour[] componentsActiveWhenFalse;

    // We use a nullable bool (bool?) so it forces an update the very first time
    private bool? _lastKnownState = null;

    void Update()
    {
        if (playerSetUp != null)
        {
            // CHECK: Is the value different from the last time we checked?
            if (_lastKnownState != playerSetUp.isRightArm)
            {
                // Only run the heavy logic if the value CHANGED
                SwitchArms(playerSetUp.isRightArm);

                // Update our memory of the state
                _lastKnownState = playerSetUp.isRightArm;
            }
        }
    }

    void SwitchArms(bool isRight)
    {
        // 1. Handle GameObjects
        foreach (GameObject obj in objectsActiveWhenTrue)
        {
            if (obj != null) obj.SetActive(isRight);
        }

        foreach (GameObject obj in objectsActiveWhenFalse)
        {
            if (obj != null) obj.SetActive(!isRight);
        }

        // 2. Handle Components
        foreach (Behaviour comp in componentsActiveWhenTrue)
        {
            if (comp != null) comp.enabled = isRight;
        }

        foreach (Behaviour comp in componentsActiveWhenFalse)
        {
            if (comp != null) comp.enabled = !isRight;
        }
    }
}
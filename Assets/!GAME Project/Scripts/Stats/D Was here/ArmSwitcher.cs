using UnityEngine;

public class ArmSwitcher : MonoBehaviour
{
    [Header("Settings")]
    public PlayerSetUp playerSetUp;

    [Header("Objects (Enable/Disable Whole Object)")]
    public GameObject[] objectsActiveWhenTrue;
    public GameObject[] objectsActiveWhenFalse;

    [Header("Components (Enable/Disable Scripts/Colliders)")]
    // 'Behaviour' lets you drag in Scripts, Colliders, Lights, etc.
    public Behaviour[] componentsActiveWhenTrue;
    public Behaviour[] componentsActiveWhenFalse;

    void Update()
    {
        if (playerSetUp != null)
        {
            SwitchArms(playerSetUp.isRightArm);
        }
    }

    void SwitchArms(bool isRight)
    {
        // --- 1. Handle GameObjects (Whole Objects) ---
        foreach (GameObject obj in objectsActiveWhenTrue)
        {
            if (obj != null) obj.SetActive(isRight);
        }

        foreach (GameObject obj in objectsActiveWhenFalse)
        {
            if (obj != null) obj.SetActive(!isRight);
        }

        // --- 2. Handle Components (Scripts/Colliders) ---
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
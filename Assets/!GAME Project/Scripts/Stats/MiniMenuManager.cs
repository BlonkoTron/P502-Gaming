using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.XR;

public class MiniMenuManager : MonoBehaviour
{
    [SerializeField] GameObject miniMenuCanvas;

    
    private UnityEngine.XR.InputDevice leftController;
    private UnityEngine.XR.InputDevice rightController;

    void Start()
    {
        // Get left controller
        var leftHandedControllers = new List<UnityEngine.XR.InputDevice>();
        InputDevices.GetDevicesAtXRNode(XRNode.LeftHand, leftHandedControllers);
        if (leftHandedControllers.Count > 0)
            leftController = leftHandedControllers[0];

        // Get right controller
        var rightHandedControllers = new List<UnityEngine.XR.InputDevice>();
        InputDevices.GetDevicesAtXRNode(XRNode.RightHand, rightHandedControllers);
        if (rightHandedControllers.Count > 0)
            rightController = rightHandedControllers[0];
    }

    void Update()
    {
        // X button on left controller
        if (leftController.isValid &&
            leftController.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primaryButton, out bool xPressed) &&
            xPressed)
        {
            ToggleMiniMenu();
        }

        // A button on right controller
        if (rightController.isValid &&
            rightController.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primaryButton, out bool aPressed) &&
            aPressed)
        {
            ToggleMiniMenu();
        }
    }

    public void ToggleMiniMenu()
    {
        if (miniMenuCanvas.activeSelf)
        {
            miniMenuCanvas.SetActive(false);
        }
        else
        {
            miniMenuCanvas.SetActive(true);
        }
    }

    public void GoToMainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

}

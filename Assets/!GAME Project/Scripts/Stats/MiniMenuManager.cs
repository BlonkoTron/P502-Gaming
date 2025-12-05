using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class MiniMenuManager : MonoBehaviour
{
    [Header("References")]
    public Transform xrRigOrCamera;
    public GameObject uiCanvas;
    [SerializeField] private PlayerSetUp playerSetUp;

    [Header("Settings")]
    public float distanceFromPlayer = 1.5f;
    public float heightOffset = 0.0f;

    private InputDevice controller;
    private bool isVisible = false;
    private bool wasPrimaryPressed = false;

    private void Update()
    {
        if (!controller.isValid)
            TryInitializeController();

        if (controller.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryPressed))
        {
            if (primaryPressed && !wasPrimaryPressed)
            {
                Debug.Log("Primary button pressed once");
                ToggleCanvas();
            }
            wasPrimaryPressed = primaryPressed;
        }
    }

    private void TryInitializeController()
    {
        if (playerSetUp.isRightArm)
            InitializeRightController();
        else
            InitializeLeftController();
    }

    public void ToggleCanvas()
    {
        isVisible = !isVisible;
        uiCanvas.SetActive(isVisible);

        if (isVisible)
            PositionCanvasInFront();
    }

    private void PositionCanvasInFront()
    {
        if (xrRigOrCamera == null || uiCanvas == null) return;

        Vector3 forward = xrRigOrCamera.forward;
        forward.y = 0f;
        forward.Normalize();

        Vector3 targetPos = xrRigOrCamera.position + forward * distanceFromPlayer;
        targetPos.y += heightOffset;

        uiCanvas.transform.position = targetPos;

        uiCanvas.transform.LookAt(xrRigOrCamera);
        uiCanvas.transform.rotation = Quaternion.Euler(0, uiCanvas.transform.rotation.eulerAngles.y + 180f, 0);
    }

    private void InitializeLeftController()
    {
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller, devices);
        if (devices.Count > 0)
            controller = devices[0];
    }

    private void InitializeRightController()
    {
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Left | InputDeviceCharacteristics.Controller, devices);
        if (devices.Count > 0)
            controller = devices[0];
    }

    public void ShowDebug()
    {
        Debug.Log("UI Button Pressed");
    }
}

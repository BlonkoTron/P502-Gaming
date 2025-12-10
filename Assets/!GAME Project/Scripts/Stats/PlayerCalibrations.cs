using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR;

public class PlayerCalibrations : MonoBehaviour
{
    [SerializeField] private PlayerSetUp playerSetUp;
    [SerializeField] TrackBents trackBents;
    [SerializeField] TrackRotations trackRotations;
    [SerializeField] RotationTracker rotationTracker;

    [Header("Auto Configuration")]
    [SerializeField] GameObject bentInROMText;
    [SerializeField] GameObject bentOutROMText;
    [SerializeField] GameObject rotationUpROMText;
    [SerializeField] GameObject rotationDownROMText;
    
    [SerializeField] private Image bentInButton;
    [SerializeField] private Image bentOutButton;
    [SerializeField] private Image rotationUpButton;
    [SerializeField] private Image rotationDownButton;

    [SerializeField] private GameObject bentInCheckmark;
    [SerializeField] private GameObject bentOutCheckmark;
    [SerializeField] private GameObject rotationUpCheckmark;
    [SerializeField] private GameObject rotationDownCheckmark;

    private int autoConfigNr;

    private UnityEngine.XR.InputDevice controller;

    private void Start()
    {
        bentInROMText.SetActive(false);
        bentOutROMText.SetActive(false);
        rotationUpROMText.SetActive(false);
        rotationDownROMText.SetActive(false);

        bentInCheckmark.SetActive(false);
        bentOutCheckmark.SetActive(false);
        rotationUpCheckmark.SetActive(false);
        rotationDownCheckmark.SetActive(false);
    }


   
    private void FixedUpdate()
    {
        if (controller.TryGetFeatureValue(UnityEngine.XR.CommonUsages.trigger, out float triggerValue))
        {
            bool isPressed = triggerValue > 0.1f; // Adjust threshold if needed

            if (isPressed)
            {
                switch (autoConfigNr)
                {
                    case 0:
                        SetBentROMIn();
                        //Debug.Log("Bent In Angle: " + trackBents.angleDegrees);
                        CheckIfconfigured(playerSetUp.bentROMIn, bentInCheckmark);
                        break;
                    case 1:
                        SetBentROMDown();
                        //Debug.Log("Bent Out Angle: " + trackBents.angleDegrees);
                        CheckIfconfigured(playerSetUp.bentROMOut, bentOutCheckmark);
                        break;
                    case 2:
                        SetRotationROMUp();
                        //Debug.Log("Rotation Up Angle: " + trackRotations.angleDegrees);
                        CheckIfconfigured(playerSetUp.rotationROMUp, rotationUpCheckmark);
                        SetNewRotationUp();
                        break;
                    case 3:
                        SetRotationROMDown();
                        //Debug.Log("Rotation Down Angle: " + trackRotations.angleDegrees);
                        CheckIfconfigured(playerSetUp.rotationROMDown, rotationDownCheckmark);
                        SetNewRotationDown();
                        break;
                }

                if (playerSetUp.bentROMIn > 0 && playerSetUp.bentROMOut > 0 && playerSetUp.rotationROMUp > 0 && playerSetUp.rotationROMDown > 0)
                {
                    playerSetUp.isConfigured = true;
                }
            }
        }
    }

    public void CheckIfconfigured(float ROM, GameObject checkMark)
    {         if (ROM != 0)
        {
            checkMark.SetActive(true);
        }
    }

    public void setAutoConficNr(int nr)
    {
        autoConfigNr = nr;
    }

    public void StartAutoConfig()
    {
        if (playerSetUp.isRightArm)
        {
            InitializeRightController();
        }
        else
        {
            InitializeLeftController();
        }
    }

    private void InitializeRightController()
    {
        List<UnityEngine.XR.InputDevice> devices = new List<UnityEngine.XR.InputDevice>();
        InputDeviceCharacteristics rightControllerCharacteristics = InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller;
        InputDevices.GetDevicesWithCharacteristics(rightControllerCharacteristics, devices);

        if (devices.Count > 0)
        {
            controller = devices[0];
        }
    }

    private void InitializeLeftController()
    {
        List<UnityEngine.XR.InputDevice> devices = new List<UnityEngine.XR.InputDevice>();
        InputDeviceCharacteristics leftControllerCharacteristics = InputDeviceCharacteristics.Left | InputDeviceCharacteristics.Controller;
        InputDevices.GetDevicesWithCharacteristics(leftControllerCharacteristics, devices);
        if (devices.Count > 0)
        {
            controller = devices[0];
        }
    }

    public void ChooseLeftArm()
    {
        playerSetUp.isRightArm = false;
        InitializeLeftController();
        trackBents.UpdateArm();
        trackRotations.UpdateArm();
    }

    public void ChooseRightArm()
    {
        playerSetUp.isRightArm = true;
        InitializeRightController();
        trackBents.UpdateArm();
        trackRotations.UpdateArm();
    }

    public void PressBentIn()
    {
        SetColor(Color.green, bentInButton);
        SetColor(Color.black, bentOutButton);
        SetColor(Color.black, rotationUpButton);
        SetColor(Color.black, rotationDownButton);

        bentInROMText.SetActive(true);
        bentOutROMText.SetActive(false);
        rotationUpROMText.SetActive(false);
        rotationDownROMText.SetActive(false);
    }

    public void PressBentOut()
    {
        SetColor(Color.black, bentInButton);
        SetColor(Color.green, bentOutButton);
        SetColor(Color.black, rotationUpButton);
        SetColor(Color.black, rotationDownButton);

        bentInROMText.SetActive(false);
        bentOutROMText.SetActive(true);
        rotationUpROMText.SetActive(false);
        rotationDownROMText.SetActive(false);
    }

    public void PressRotationUp()
    {
        SetColor(Color.black, bentInButton);
        SetColor(Color.black, bentOutButton);
        SetColor(Color.green, rotationUpButton);
        SetColor(Color.black, rotationDownButton);

        bentInROMText.SetActive(false);
        bentOutROMText.SetActive(false);
        rotationUpROMText.SetActive(true);
        rotationDownROMText.SetActive(false);
    }

    public void PressRotationDown()
    {
        SetColor(Color.black, bentInButton);
        SetColor(Color.black, bentOutButton);
        SetColor(Color.black, rotationUpButton);
        SetColor(Color.green, rotationDownButton);

        bentInROMText.SetActive(false);
        bentOutROMText.SetActive(false);
        rotationUpROMText.SetActive(false);
        rotationDownROMText.SetActive(true);
    }

    public void SetColor(Color color, Image image)
    {
        image.color = color;
    }

    public void SetBentROMIn()
    {
        playerSetUp.bentROMIn = trackBents.angleDegrees;
        //Debug.Log("Bent In Set to: " + playerSetUp.bentROMIn);
    }

    public void SetBentROMDown()
    {
        playerSetUp.bentROMOut = trackBents.angleDegrees;
    }

    public void SetRotationROMUp()
    {
        playerSetUp.rotationROMUp = trackRotations.angleDegrees;
    }

    public void SetRotationROMDown()
    {
        playerSetUp.rotationROMDown = trackRotations.angleDegrees;
    }

    public void SetNewRotationUp()
    {
        if(playerSetUp.isRightArm)
        {
            rotationTracker.ToggleTPD(rotationTracker.tpdRightUp);
            playerSetUp.rotationUp = rotationTracker.tpdRightUp.transform.localRotation;
        }
        else
        {
            rotationTracker.ToggleTPD(rotationTracker.tpdLeftUp);
            playerSetUp.rotationUp = rotationTracker.tpdLeftUp.transform.localRotation;
        }
    }

    public void SetNewRotationDown()
    {
        if (playerSetUp.isRightArm)
        {
            rotationTracker.ToggleTPD(rotationTracker.tpdRightDown);
            playerSetUp.rotationDown = rotationTracker.tpdRightDown.transform.localRotation;
        }
        else
        {
            rotationTracker.ToggleTPD(rotationTracker.tpdLeftDown);
            playerSetUp.rotationDown = rotationTracker.tpdLeftDown.transform.localRotation;
        }
    }


}

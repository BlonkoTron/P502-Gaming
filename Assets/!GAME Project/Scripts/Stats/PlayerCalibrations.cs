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

    [Header("Dropdowns")]
    [SerializeField] private Dropdown bentInDrop;
    [SerializeField] private Dropdown bentOutDrop;
    [SerializeField] private Dropdown rotationUpDrop;
    [SerializeField] private Dropdown rotationDownDrop;

    [Header("Choose Arm")]
    //[SerializeField] private GameObject leftButton;
    //[SerializeField] private GameObject rightButton;

    [Header("Choose Configuration")]
    [SerializeField] private GameObject manuelConfigButton;
    [SerializeField] private GameObject autoConfigButton;

    [Header("Auto Configuration")]
    [SerializeField] TMP_Text bentInROMText;
    [SerializeField] TMP_Text bentOutROMText;
    [SerializeField] TMP_Text rotationUpROMText;
    [SerializeField] TMP_Text rotationDownROMText;
    
    [SerializeField] private Button bentInButton;
    [SerializeField] private Button bentOutButton;
    [SerializeField] private Button rotationUpButton;
    [SerializeField] private Button rotationDownButton;

    public bool configBentIn;
    public bool configBentOut;
    public bool configRotationUp;
    public bool configRotationDown;

    private int autoConfigNr;

    private UnityEngine.XR.InputDevice controller;

    private void Start()
    {
        autoConfigNr = 0;
    }


    //this needs to be changed 
    private void FixedUpdate()
    {
        if (controller.TryGetFeatureValue(UnityEngine.XR.CommonUsages.trigger, out float triggerValue))
        {
            bool isPressed = triggerValue > 0.1f; // Adjust threshold if needed

            if(isPressed)
            {
                switch (autoConfigNr)
                {
                    case 0:
                        Debug.Log("Trigger pressed - Setting Bent ROM In");
                        SetBentROMInAuto();
                        bentInROMText.text = "Bent ROM In: " + playerSetUp.bentROMIn.ToString();
                        ActivateButton(bentInButton);
                        break;
                    case 1:
                        SetBentROMDown();
                        bentOutROMText.text = "Bent ROM Out: " + playerSetUp.bentROMOut.ToString();
                        ActivateButton(bentOutButton);
                        break;
                    case 2:
                        SetRotationROMUp();
                        rotationUpROMText.text = "Rotation ROM Up: " + playerSetUp.rotationROMUp.ToString();
                        ActivateButton(rotationUpButton);
                        break;
                    case 3:
                        SetRotationROMDown();
                        rotationDownROMText.text = "Rotation ROM Down: " + playerSetUp.rotationROMDown.ToString();
                        ActivateButton(rotationDownButton);
                        break;
                }
            }
        }
    }


    public void AddAutoConficNr()
    {
         autoConfigNr++;
    }

    public void ResetAutoConficNr()
    {
        autoConfigNr = 0;
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
        //leftButton.GetComponent<Image>().color = Color.green;
        //rightButton.GetComponent<Image>().color = Color.white;
    }

    public void ChooseRightArm()
    {
        playerSetUp.isRightArm = true;
        InitializeRightController();
        //rightButton.GetComponent<Image>().color = Color.green;
        //leftButton.GetComponent<Image>().color = Color.white;
    }

    public void ChooseManualConfig()
    {
        playerSetUp.isManualConfig = true;
        manuelConfigButton.GetComponent<Image>().color = Color.green;
        autoConfigButton.GetComponent<Image>().color = Color.white;
    }

    public void ChooseAutoConfig()
    {
        playerSetUp.isManualConfig = false;
        autoConfigButton.GetComponent<Image>().color = Color.green;
        manuelConfigButton.GetComponent<Image>().color = Color.white;
    }

    public void UpdateBentROMIn()
    {
         switch (bentInDrop.value)
         {
             case 0:
                    playerSetUp.bentROMIn = 15f;
                    break;
             case 1:
                    playerSetUp.bentROMIn = 25f;
                    break;
             case 2:
                    playerSetUp.bentROMIn = 35f;
                    break;
             case 3:
                    playerSetUp.bentROMIn = 45f;
                    break;
             case 4:
                    playerSetUp.bentROMIn = 55f;
                    break;
             case 5:
                    playerSetUp.bentROMIn = 65f;
                    break;
         }
    }

    public void UpdateBentROMOut()
    {
         switch (bentOutDrop.value)
         {
             case 0:
                    playerSetUp.bentROMOut = 115f;
                    break;
             case 1:
                    playerSetUp.bentROMOut = 125f;
                    break;
             case 2:
                    playerSetUp.bentROMOut = 135f;
                    break;
             case 3:
                    playerSetUp.bentROMOut = 145f;
                    break;
             case 4:
                    playerSetUp.bentROMOut = 155f;
                    break;
             case 5:
                    playerSetUp.bentROMOut = 165f;
                    break;
         }
    }

    public void UpdateRotationROMUp()
    {
         switch (rotationUpDrop.value)
         {
             case 0:
                    playerSetUp.rotationROMUp = 25f;
                    break;
             case 1:
                    playerSetUp.rotationROMUp = 35f;
                    break;
             case 2:
                    playerSetUp.rotationROMUp = 45f;
                    break;
             case 3:
                    playerSetUp.rotationROMUp = 55f;
                    break;
             case 4:
                    playerSetUp.rotationROMUp = 65f;
                    break;
             case 5:
                    playerSetUp.rotationROMUp = 75f;
                    break;
             case 6:
                    playerSetUp.rotationROMUp = 85f;
                    break;
        }
    }

    public void UpdateRotationROMDown()
    {
         switch (rotationDownDrop.value)
         {
             case 0:
                    playerSetUp.rotationROMDown = 25f;
                    break;
             case 1:
                    playerSetUp.rotationROMDown = 35f;
                    break;
             case 2:
                    playerSetUp.rotationROMDown = 45f;
                    break;
             case 3:
                    playerSetUp.rotationROMDown = 55f;
                    break;
             case 4:
                    playerSetUp.rotationROMDown = 65f;
                    break;
             case 5:
                    playerSetUp.rotationROMDown = 75f;
                    break;
             case 6:
                    playerSetUp.rotationROMDown = 85f;
                    break;
        }
    }

    public void SetBentROMInAuto()
    {
       playerSetUp.bentROMIn = trackBents.angleDegrees;
    }

    public void SetBentROMDown()
    {
        playerSetUp.bentROMOut = trackBents.angleDegrees;
    }

    public void SetRotationROMUp()
    {
        playerSetUp.rotationROMUp = 190 + trackRotations.angleDegrees;
    }

    public void SetRotationROMDown()
    {
        playerSetUp.rotationROMDown = 190 - trackRotations.angleDegrees;
    }

    public void ResetAllSetUp()
    {
        playerSetUp.rotationROMUp = 0;
        playerSetUp.rotationROMDown = 0;
        playerSetUp.bentROMIn = 0;
        playerSetUp.bentROMOut = 0;

        playerSetUp.isConfigured = false;
    }

    public void ActivateButton(Button button)
    {
        switch (autoConfigNr)
        {
            case 0:
                if (playerSetUp.bentROMIn != 0)
                {
                    button.interactable = true;
                }
                break;
            case 1:
                if (playerSetUp.bentROMOut != 0)
                {
                    button.interactable = true;
                }
                break;
            case 2:
                if (playerSetUp.rotationROMUp != 0)
                {
                    button.interactable = true;
                }
                break;
            case 3:
                if (playerSetUp.rotationROMDown != 0)
                {
                    button.interactable = true;
                }
                break;
        }
    }




}

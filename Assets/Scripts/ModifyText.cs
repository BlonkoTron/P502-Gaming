using UnityEngine;
using TMPro;

public class ModifyText : MonoBehaviour
{

    public GameObject lowerArmBone;
    public GameObject handBone;
    Quaternion ArmRotation;
    Quaternion HandRotation;
    public TMP_Text xRotationText;
    public TMP_Text yRotationText;
    public TMP_Text zRotationText;
    public TMP_Text xHandRotationText;
    public TMP_Text yHandRotationText;
    public TMP_Text zHandRotationText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        xRotationText.text = "Arm X: ";
        yRotationText.text = "Arm Y: ";
        zRotationText.text = "Arm Z: ";
        xHandRotationText.text = "Hand X: ";
        yHandRotationText.text = "Hand Y: ";
        zHandRotationText.text = "Hand Z: ";
    }

    // Update is called once per frame
    void Update()
    {
        GetRotations();
        xRotationText.text = $"Arm X: {ArmRotation.x}";
        yRotationText.text = $"Arm Y: {ArmRotation.y}";
        zRotationText.text = $"Arm Z: {ArmRotation.z}";

        xHandRotationText.text = $"Hand X: {HandRotation.x}";
        yHandRotationText.text = $"Hand Y: {HandRotation.y}";
        zHandRotationText.text = $"Hand Z: {HandRotation.z}";
    }

    public void GetRotations()
    {
        lowerArmBone.transform.GetLocalPositionAndRotation(out Vector3 localPosition, out Quaternion localRotation);
        ArmRotation.x = localRotation.eulerAngles.x;
        ArmRotation.y = localRotation.eulerAngles.y;
        ArmRotation.z = localRotation.eulerAngles.z;

        handBone.transform.GetLocalPositionAndRotation(out Vector3 localPositionH, out Quaternion localRotationH);
        HandRotation.x = localRotationH.eulerAngles.x;
        HandRotation.y = localRotationH.eulerAngles.y;
        HandRotation.z = localRotationH.eulerAngles.z;
    }

}

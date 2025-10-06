using UnityEngine;
using TMPro;

public class ModifyText : MonoBehaviour
{

    public GameObject lowerArmBone;
    Quaternion Rotation;
    public TMP_Text xRotationText;
    public TMP_Text yRotationText;
    public TMP_Text zRotationText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        xRotationText.text = "X: ";
        yRotationText.text = "Y: ";
        zRotationText.text = "Z: ";
    }

    // Update is called once per frame
    void Update()
    {
        GetRotations();
        xRotationText.text = $"X: {Rotation.x}";
        xRotationText.text = $"Y: {Rotation.y}";
        xRotationText.text = $"Z: {Rotation.z}";
    }

    public void GetRotations()
    {
        lowerArmBone.transform.GetLocalPositionAndRotation(out Vector3 localPosition, out Quaternion localRotation);
        Rotation.x = localPosition.x;
        Rotation.y = localPosition.y;
        Rotation.z = localPosition.z;
    }

}

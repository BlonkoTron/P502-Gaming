using UnityEngine;
using TMPro;

public class ModifyText : MonoBehaviour
{

    public GameObject lowerArmBone;
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
        
    }
}

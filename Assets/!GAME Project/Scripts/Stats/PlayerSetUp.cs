using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSetUp", menuName = "Scriptable Objects/PlayerSetUp")]
public class PlayerSetUp : ScriptableObject
{
    public bool isRightArm;
    public float bentROMIn;
    public float bentROMOut;
    //public float rotationROMUp;
    //public float rotationROMDown;

    public Quaternion rotationUp;
    public Quaternion rotationDown;

    public bool isConfigured;
}

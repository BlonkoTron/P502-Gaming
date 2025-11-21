using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSetUp", menuName = "Scriptable Objects/PlayerSetUp")]
public class PlayerSetUp : ScriptableObject
{
    public bool isRightArm; // true = right arms, false = left arm
    public float bentROMIn;
    public float bentROMOut;
    public float rotationROMUp;
    public float rotationROMDown;

    public bool isConfigured;
    public bool isManualConfig;
}

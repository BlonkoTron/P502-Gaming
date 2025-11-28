using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSetUp", menuName = "Scriptable Objects/PlayerSetUp")]
public class PlayerSetUp : ScriptableObject
{
    public bool isRightArm; // true = left arm, false = right arm  somewhere in the making of the game this got flipped
    public float bentROMIn;
    public float bentROMOut;
    public float rotationROMUp;
    public float rotationROMDown;

    public bool isConfigured;
}

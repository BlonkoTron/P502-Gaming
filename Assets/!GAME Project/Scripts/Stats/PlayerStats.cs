using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "Scriptable Objects/PlayerStats")]
public class PlayerStats : ScriptableObject
{
    public int nrOfBentsIn;
    public int nrOfBentsOut;

    public int nrOfRotationsUp;
    public int nrOfRotationsDown;
}

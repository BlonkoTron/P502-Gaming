using UnityEngine;

public class RotationCollider : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;

    [SerializeField] private GameObject CubeUp;
    [SerializeField] private GameObject CubeDown;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == CubeUp)
        {
            playerStats.nrOfRotationsUp++;
        }
        else if (other.gameObject == CubeDown)
        {
            playerStats.nrOfRotationsDown++;
        }
    }
}

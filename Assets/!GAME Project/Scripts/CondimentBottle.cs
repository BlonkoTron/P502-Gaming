using UnityEngine;

public class CondimentBottle : MonoBehaviour
{
    [SerializeField] private GameObject condimentSpawnPrefab;
    private Collider condimentCollider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        condimentCollider = GetComponentInChildren<Collider>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        
    }
}

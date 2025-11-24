using UnityEngine;

public class CupDispenser : MonoBehaviour
{

    public GameObject prefabToSpawn;

    public Transform spawnLocation;

    public void SpawnObject()
    {
        if (prefabToSpawn == null) return;

        Transform targetTransform = (spawnLocation != null) ? spawnLocation : transform;

        Instantiate(prefabToSpawn, targetTransform.position, targetTransform.rotation);
    }
}


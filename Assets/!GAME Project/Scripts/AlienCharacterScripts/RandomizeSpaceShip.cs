using System.Collections.Generic;
using UnityEngine;

public class RandomizeSpaceShip : MonoBehaviour
{

    [SerializeField] private List<Material> materialList;

    private void Awake()
    {
        int randomMaterial = Random.Range(0, materialList.Count);

        if (GetComponent<Renderer>() != null)
        {
            GetComponent<Renderer>().material = materialList[randomMaterial];
        }
    }

}

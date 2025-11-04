using System.Collections.Generic;
using UnityEngine;

public class RandomizeSpaceShip : MonoBehaviour
{

    [SerializeField] private List<Material> materialList;
    [SerializeField] private GameObject Trail_R;
    [SerializeField] private GameObject Trail_L;


    private void Awake()
    {
        int randomMaterial = Random.Range(0, materialList.Count);

        if (GetComponent<Renderer>() != null)
        {
            GetComponent<Renderer>().material = materialList[randomMaterial];

        }
    }

}

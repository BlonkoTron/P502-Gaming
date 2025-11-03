using System.Collections.Generic;
using UnityEngine;

public class RandomizeAlien : MonoBehaviour
{
    [SerializeField] private List<GameObject> hatList;
    [SerializeField] private List<Material> materialList;
    [SerializeField] private List<GameObject> bodyPartList;

    void Awake()
    {
        foreach (GameObject hat in hatList) {
        
            hat.SetActive(false);

        }

        RandomizeAlienApperance();

    }

    private void RandomizeAlienApperance()
    {
        int randomHat = Random.Range(0, hatList.Count+1);
        if (randomHat != hatList.Count)
        {
            hatList[randomHat].SetActive(true);
        }


        int randomMaterial = Random.Range(0, materialList.Count);
        foreach (GameObject part in bodyPartList) 
        { 
            if(part.GetComponent<Renderer>() != null)
            {
                part.GetComponent<Renderer>().material = materialList[randomMaterial];
            }

        
        }

    }

}

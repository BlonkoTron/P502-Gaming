using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tv : MonoBehaviour
{
    public bool halt = false;

    public int timer;

    //Set up list
    [Header("Ingredient Prefabs")]
    public List<Material> video;

    // Update is called once per frame
    void Update()
    {  
        if (halt == false)
        {
            halt = true;
            StartCoroutine(FiveSecondTimer());
        }
        
    }

    IEnumerator FiveSecondTimer()
    {
        Debug.Log("Timer started!");

        yield return new WaitForSeconds(timer);

        int randomIndex = Random.Range(0, video.Count);

        gameObject.GetComponent<MeshRenderer>().material = video[randomIndex];

        halt = false;
    }
}

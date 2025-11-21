using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Tv : MonoBehaviour
{
    public bool halt = false;

    public int timer;

    public int view = 0 ;

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

        view++;

        if (view == video.Count)
        {
            view = 0;
        }

        gameObject.GetComponent<MeshRenderer>().material = video[view];

        halt = false;
    }
}

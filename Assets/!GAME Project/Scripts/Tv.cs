using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class Tv : MonoBehaviour
{
    public bool halt = false;

    public bool tvtime = false;

    public bool channelshift = false;

    public int timer;

    public int counter = 0;

    public int view = 0 ;

    [SerializeField] private EventReference TVmusics;
    private EventInstance TVmusicwhole;

    [SerializeField] private EventReference TVbreak;
    private EventInstance TVbreakSFX;

    //Set up list
    [Header("Ingredient Prefabs")]
    public List<Material> video;

    // Update is called once per frame
    void Update()
    {
        if (halt == false && tvtime == false)
        {
            TVmusicwhole = Audiomanager.instance.PlaySound(TVmusics, transform.position);
            StartCoroutine(WaitForFMODToFinish(TVmusicwhole));
            halt = true;
        }
        
        Audiomanager.instance.UpdateSoundPosition(TVmusicwhole, transform.position);
        Audiomanager.instance.UpdateSoundPosition(TVbreakSFX, transform.position);

        if (halt == false && tvtime == true && channelshift == false)
        {
            channelshift = true;
            TVbreakSFX = Audiomanager.instance.PlaySound(TVbreak, transform.position);
            StartCoroutine(OneSecondTimer());
        }
        
    }

    IEnumerator OneSecondTimer()
    {
        yield return new WaitForSeconds(1f);

        StartCoroutine(FiveSecondTimer());
    }

    IEnumerator WaitForFMODToFinish(EventInstance instance)
    {
        PLAYBACK_STATE state;

        while (true)
        {
            instance.getPlaybackState(out state);

            if (state == PLAYBACK_STATE.STOPPED)
            {
                Debug.Log("FMOD TV music finished playing!");
                channelshift = true;
                halt = false;
                counter++;
                if (counter == 3)
                {
                    tvtime = false;
                    counter = 0;
                }
               
                break;

                
            }

            yield return null;
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

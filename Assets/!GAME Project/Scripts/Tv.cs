using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FMOD.Studio;
using FMODUnity;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Playables;

public class Tv : MonoBehaviour
{
    public bool halt = false;

    public bool halt2 = false;

    public int timer;

    public int counterdelay = 0;

    public PLAYBACK_STATE TVMUSIC;
    public PLAYBACK_STATE TVADS;


    [SerializeField] private EventReference TVmusics;
    private EventInstance TVmusicwhole;

    [SerializeField] private EventReference TVbreak;
    private EventInstance TVbreakSFX;

    //Set up list of material on TV
    [Header("Ingredient Prefabs")]
    public List<Material> video;

    private void Start()
    {
        gameObject.GetComponent<MeshRenderer>().material = video[0];
    }

    // Update is called once per frame
    void Update()
    {
        Audiomanager.instance.UpdateSoundPosition(TVmusicwhole, transform.position); //keep music position on tv
        Audiomanager.instance.UpdateSoundPosition(TVbreakSFX, transform.position); //keep music position on tv

        if (!TVmusicwhole.isValid() && halt == false) //check if music is valid and plays it once.
        {
            gameObject.GetComponent<MeshRenderer>().material = video[0];
            halt = true;
            TVmusicwhole = Audiomanager.instance.PlaySound(TVmusics, transform.position);
        }

        // 2. Continuously check playback state of the music
        if (TVmusicwhole.isValid())
        {
            TVmusicwhole.getPlaybackState(out TVMUSIC);

            // 3. When music actually finishes, trigger break SFX only once
            if (TVMUSIC == FMOD.Studio.PLAYBACK_STATE.STOPPED && halt == true && halt2 == false)
            {
                TVmusicwhole.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                TVmusicwhole.release();
                TVmusicwhole.clearHandle(); 

                halt2 = true;
                TVbreakSFX = Audiomanager.instance.PlaySound(TVbreak, transform.position);
                StartCoroutine(OneSecondTimer());
            }
        }
    }

    IEnumerator OneSecondTimer()
    {
        if (counterdelay != 3)
        {
            gameObject.GetComponent<MeshRenderer>().material = video[4];
            yield return new WaitForSeconds(1f);
            StartCoroutine(FiveSecondTimer());
        }
        else
        {
            gameObject.GetComponent<MeshRenderer>().material = video[4];
            halt = false;
            halt2 = false;
            counterdelay = 0;
        }
        
    }

    IEnumerator FiveSecondTimer()
    {
        Debug.Log("Timer started!");
        counterdelay++;

        if (counterdelay == 1)
        {
            gameObject.GetComponent<MeshRenderer>().material = video[1];
        }
        else if (counterdelay == 2)
        {
            gameObject.GetComponent<MeshRenderer>().material = video[2];
        }
        else if (counterdelay == 3)
        {
            gameObject.GetComponent<MeshRenderer>().material = video[3];
        }

        yield return new WaitForSeconds(timer);

        StartCoroutine(OneSecondTimer());
    }
}

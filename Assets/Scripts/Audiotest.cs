using FMODUnity;
using FMOD.Studio;
using UnityEngine;
using FMOD;

public class Audiotest : MonoBehaviour
{
    private EventInstance soundInstance;
    private EventInstance soundInstance2;
    [SerializeField] private EventReference Sound1;
    [SerializeField] private EventReference Sound2;

    private void Start()
    {
        // Start and hold the event instance so it can be controlled
        soundInstance = RuntimeManager.CreateInstance(Sound1);
        soundInstance.set3DAttributes(RuntimeUtils.To3DAttributes(transform.position));
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            soundInstance = Audiomanager.instance.PlaySound(Sound1, transform.position);
            soundInstance2 = Audiomanager.instance.PlaySound(Sound2, transform.position);
        }

        if (Input.GetKeyDown(KeyCode.P)) // For pause
        {
            Audiomanager.instance.PauseSound(soundInstance, true);
            Audiomanager.instance.PauseSound(soundInstance2, true);
        }

        if (Input.GetKeyDown(KeyCode.R)) // For resume
        {
            Audiomanager.instance.PauseSound(soundInstance, false);
            Audiomanager.instance.PauseSound(soundInstance2, false);
        }

        if (Input.GetKeyDown(KeyCode.S)) // For stop
        {
            Audiomanager.instance.StopSound(soundInstance);
            Audiomanager.instance.StopSound(soundInstance2);
        }
        //SetEffectActive(false);
        //SetEffectActive(true);
    }

    //Sets a global bool which turns the effect ON/off
    public void SetEffectActive(bool active)
    {
        //If the sound i valid we continue, if not we go back to prevent errors.
        if (!soundInstance.isValid()) return;

        // Get the channel group from FMOD. This is the default group for the track.
        soundInstance.getChannelGroup(out ChannelGroup channelGroup);

        //checks if the channel group is valid, to avoid crashes
        if (channelGroup.hasHandle())
        {
            // we loop through the channelgroup for the sound. Then we see how many SFX are attached to it. 
            int numDSPs;
            channelGroup.getNumDSPs(out numDSPs);

            //then it loops thorugh each one. 
            for (int i = 0; i < numDSPs; i++)
            {
                channelGroup.getDSP(i, out DSP dsp);
                //pulls all the data out to find the effects/filters
                dsp.getInfo(out string name, out uint version, out int channels, out int configWidth, out int configHeight);

                // Replace this with your Lowpass effect name from FMOD Studio. if it finds the "lowpass" it can set the effect ON/OFF
                if (name.Contains("Lowpass"))
                {
                    //when the command SetEffectActive(true/false) is called. it activates/deactivates the filter. Breaks in the end for another call. 
                    dsp.setBypass(!active);
                }

                if (name.Contains("Highpass"))
                {
                    dsp.setBypass(!active);
                }
            }
        }
    }
}
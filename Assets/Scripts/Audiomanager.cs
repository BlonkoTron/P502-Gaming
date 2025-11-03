using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class Audiomanager : MonoBehaviour
{
    public static Audiomanager instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("NOPE");
            return;
        }
        instance = this;
    }

    // Create and start a sound with control
    public EventInstance PlaySound(EventReference sound, Vector3 worldPos)
    {
        EventInstance instance = RuntimeManager.CreateInstance(sound);
        instance.set3DAttributes(RuntimeUtils.To3DAttributes(worldPos));
        instance.start();
        return instance;
    }

    public void StopSound(EventInstance instance)
    {
        instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT); // Or .IMMEDIATE
        instance.release();
    }

    public void PauseSound(EventInstance instance, bool pause)
    {
        instance.setPaused(pause);
    }
}


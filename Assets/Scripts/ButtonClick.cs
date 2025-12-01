using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class ButtonClick : MonoBehaviour
{
    private EventInstance Button;
    [SerializeField] private EventReference Buttonclick;

    private EventInstance Stovebutton;
    [SerializeField] private EventReference Stoveclick;


    // Update is called once per frame
    void Update()
    {
        Audiomanager.instance.UpdateSoundPosition(Button, transform.position);
        Audiomanager.instance.UpdateSoundPosition(Stovebutton, transform.position);
    }

    public void ButtonSound()
    {
        Button = Audiomanager.instance.PlaySound(Buttonclick, transform.position);
    }

    public void StoveSound()
    {
        Stovebutton = Audiomanager.instance.PlaySound(Stoveclick, transform.position);
    }
}

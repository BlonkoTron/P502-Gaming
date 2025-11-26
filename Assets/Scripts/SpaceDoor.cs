using System.Collections;
using UnityEngine;

public class SpaceDoor : MonoBehaviour
{
    //Bool to activate door and set a cooldown for no double presses
    public bool Doorpress;
    public bool cooldown;

    //animator
    public Animator Spacedoor;

    //waittime between open/close
    public float waitTime = 2.0f;

    public void Update()
    {
        if ((Doorpress == true) && (cooldown == false))
        {
            //activate sequence
            opening();
            cooldown = true;
            Doorpress = false;
        }

    }

    public void opening()
    {
        ButtonOpenRoof();
        cooldown = true;
    }

    IEnumerator Timer(float duration)
    {
        //returns after waittime is over so the sequence restarts
        yield return new WaitForSeconds(waitTime);
        Spacedoor.SetBool("Close", true);
        Spacedoor.SetBool("Open", false);
        cooldown = false;
        Debug.Log("timer done");
    }

    public void ButtonOpenRoof()
    {
       

        //activate the animator and sets the tier
        Spacedoor.SetBool("Open", true);
        Spacedoor.SetBool("Close", false);
        StartCoroutine(Timer(waitTime));

    }
}

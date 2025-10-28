using System.Collections;
using UnityEngine;

public class SpaceDoor : MonoBehaviour
{
    public bool Doorpress;
    public bool cooldown;

    public Animator Spacedoor;

    public float waitTime = 2.0f;

    private void Update()
    {
        if ((Doorpress == true) && (cooldown == false))
        {
            opening();
            cooldown = true;
            Doorpress = false;
        }

    }

    public void opening()
    {
        Spacedoor.SetBool("Open", true);
        Spacedoor.SetBool("Close", false);

        StartCoroutine(Timer(waitTime));
    }

    IEnumerator Timer(float duration)
    {
        yield return new WaitForSeconds(waitTime);
        Spacedoor.SetBool("Close", true);
        Spacedoor.SetBool("Open", false);
        cooldown = false;
        Debug.Log("timer done");
    }
}

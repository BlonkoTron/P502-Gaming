using System.Collections;
using UnityEngine;

public class SpaceDoor : MonoBehaviour
{
    public bool Doorpress ;
    public bool cooldown ;

    public Animator Door_L;
    public Animator Door_R;

    public float waitTime = 2.0f;

    private void Update()
    {
        if ((Doorpress = true) && (cooldown = false))
        {
            opening();
            cooldown = true;
        }

    }

    public void opening()
    {
        Door_L.SetBool("Open_L", true);
        Door_R.SetBool("Open_R", true);
        Door_L.SetBool("Close_L", false);
        Door_R.SetBool("Close_R", false);

        StartCoroutine(Timer(waitTime));
    }

    IEnumerator Timer(float duration)
    {
        yield return new WaitForSeconds(waitTime);
        Door_L.SetBool("Open_L", false);
        Door_R.SetBool("Open_R", false);
        Door_L.SetBool("Close_L", true);
        Door_R.SetBool("Close_R", true);
        cooldown = false;
        Debug.Log("timer done");
    }
}

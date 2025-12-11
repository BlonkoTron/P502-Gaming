using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using FMOD.Studio;
using FMODUnity;
using NUnit.Framework.Internal;

public class Bell : MonoBehaviour
{
    public UnityEvent OnBellPressed;

    private EventInstance Bellsound;
    [SerializeField] private EventReference Belltester;

    private bool canBell;

    private void Start()
    {
        OrderController.Instance.OnNewOrderGenerated.AddListener(ResetBell);
    }

    private void Update()
    {
        Audiomanager.instance.UpdateSoundPosition(Bellsound, transform.position);
    }

    /*[SerializeField] private float bellCooldown = 1f;
private float lastBellPressTime;


private void OnTriggerEnter(Collider other)
{
    if (other.CompareTag("Bell") && Time.time>lastBellPressTime+bellCooldown)
    {
        lastBellPressTime = Time.time;
        OnBellPressed.Invoke();
        Debug.Log("Ding");
    }
} */
    public void BellPressed()
    {
        Bellsound = Audiomanager.instance.PlaySound(Belltester, transform.position);

        if (canBell)
        {
            canBell = false;
            OnBellPressed.Invoke();
            
        }
        
        
    }

    private void ResetBell(Order hahaloldenskaldenikkebruge)
    {
        canBell = true;
    }

}


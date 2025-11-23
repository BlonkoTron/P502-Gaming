using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using FMOD.Studio;
using FMODUnity;

public class Bell : MonoBehaviour
{
    public UnityEvent OnBellPressed;

    private EventInstance Bellsound;
    [SerializeField] private EventReference Bellding;

    [SerializeField] private float bellCooldown = 1f;
    private float lastBellPressTime;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bell") && Time.time>lastBellPressTime+bellCooldown)
        {
            Audiomanager.instance.UpdateSoundPosition(Bellsound, transform.position);
            lastBellPressTime = Time.time;
            OnBellPressed.Invoke();
            Debug.Log("Ding");
        }
    }
}

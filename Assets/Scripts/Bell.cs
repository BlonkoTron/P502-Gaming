using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class Bell : MonoBehaviour
{
    public UnityEvent OnBellPressed;
    [SerializeField] private float bellCooldown = 1f;
    private float lastBellPressTime;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bell") && Time.time>lastBellPressTime+bellCooldown)
        {
            lastBellPressTime = Time.time;
            OnBellPressed.Invoke();
            Debug.Log("Ding");
        }
    }
}

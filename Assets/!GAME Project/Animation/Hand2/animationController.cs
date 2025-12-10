using UnityEngine.InputSystem;
using UnityEngine;


public class animationController : MonoBehaviour
{

    public InputActionProperty grab;
    public InputActionProperty trigger;

    public Animator Myanimator;

    // Update is called once per frame
    void Update()
    {
        float grabvalue = grab.action.ReadValue<float>();
        Myanimator.SetFloat("grab", grabvalue);

        float triggervalue = trigger.action.ReadValue<float>();
        Myanimator.SetFloat("trigger", triggervalue);
    }
}
using UnityEngine;
using UnityEngine.InputSystem;

public class AnimationController : MonoBehaviour
{

    public InputActionProperty grabAction;
    public InputActionProperty triggerAction;

    public InputActionProperty grabActionLeft;
    public InputActionProperty triggerActionLeft;

    public Animator handAnimator;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float grabValue = grabAction.action.ReadValue<float>();
        handAnimator.SetFloat("grab", grabValue);


        float triggerValue = triggerAction.action.ReadValue<float>();
        handAnimator.SetFloat("trigger", triggerValue);

        float grabValueLeft = grabActionLeft.action.ReadValue<float>();
        handAnimator.SetFloat("grabLeft", grabValueLeft);

        float triggerValueLeft = triggerActionLeft.action.ReadValue<float>();
        handAnimator.SetFloat("triggerLeft", triggerValueLeft);

    }
}

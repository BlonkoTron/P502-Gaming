using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class HapticTouch : MonoBehaviour
{
    [Header("Haptic Settings")]
    [SerializeField] private float hapticIntensity = 0.5f;
    [SerializeField] private float hapticDuration = 0.2f;

    private XRGrabInteractable grabInteractable;

    void Start()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.AddListener(OnGrab);
            Debug.Log("HapticTouch: Listener added to " + gameObject.name);
        }
        else
        {
            Debug.LogError("HapticTouch: No XRGrabInteractable found on " + gameObject.name);
        }
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        Debug.Log("HapticTouch: Object grabbed!");
        
        // Get the interactor (controller) that grabbed this object
        if (args.interactorObject is XRBaseInputInteractor inputInteractor)
        {
            Debug.Log("HapticTouch: Starting haptic coroutine - Intensity: " + hapticIntensity + ", Duration: " + hapticDuration);
            StartCoroutine(SendHapticFeedback(inputInteractor));
        }
        else
        {
            Debug.LogWarning("HapticTouch: Interactor is not XRBaseInputInteractor, it is: " + args.interactorObject.GetType());
        }
    }

    private IEnumerator SendHapticFeedback(XRBaseInputInteractor interactor)
    {
        // Send haptic impulse to the controller
        interactor.SendHapticImpulse(hapticIntensity, hapticDuration);
        yield return new WaitForSeconds(hapticDuration);
    }

    private void OnDestroy()
    {
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.RemoveListener(OnGrab);
        }
    }
}
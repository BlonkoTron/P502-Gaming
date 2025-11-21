using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class RB_EnableOnDetach : MonoBehaviour
{
    private Rigidbody _rb;
    private XRGrabInteractable _interactable;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _interactable = GetComponent<XRGrabInteractable>();
    }

    void OnEnable()
    {
        // Subscribe to the event when the object is released (from hand or socket)
        _interactable.selectExited.AddListener(OnRelease);
    }

    void OnDisable()
    {
        _interactable.selectExited.RemoveListener(OnRelease);
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        // Force physics back on
        if (_rb != null)
        {
            _rb.isKinematic = false;
            _rb.useGravity = false;

        }
    }
}
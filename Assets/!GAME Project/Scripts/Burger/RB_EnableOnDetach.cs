using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class RB_EnableOnDetach : MonoBehaviour
{
    private Rigidbody _rb;
    private XRGrabInteractable _interactable;

    // Variables to cache the original drag settings
    private float _defaultDrag;
    private float _defaultAngularDrag;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _interactable = GetComponent<XRGrabInteractable>();

        // Cache the correct drag values exactly as they are in the Inspector on startup
        if (_rb != null)
        {
            _defaultDrag = _rb.linearDamping;
            _defaultAngularDrag = _rb.angularDamping;
        }
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
        if (_rb != null)
        {
            // Force physics back on
            _rb.isKinematic = false;

            // NOTE: You had this set to false in your original script. 
            // If you want the object to fall with gravity, make sure this is true!
            _rb.useGravity = false;

            // Force the dampening back to the original cached values
            _rb.linearDamping = _defaultDrag;
            _rb.angularDamping = _defaultAngularDrag;
        }
    }
}
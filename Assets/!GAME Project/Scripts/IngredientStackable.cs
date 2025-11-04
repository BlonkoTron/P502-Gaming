using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(XRGrabInteractable))]
public class IngredientStackable : MonoBehaviour
{
    [Header("Snap Reference Point")]
    public Transform snapBottom; // The point that aligns with the plate's top snap point

    private XRGrabInteractable grabInteractable;
    private PlateManager plateManager;
    private Rigidbody rb;

    private void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        rb = GetComponent<Rigidbody>();

        // Subscribe to grab and release events
        grabInteractable.selectExited.AddListener(OnReleased);
        grabInteractable.selectEntered.AddListener(OnGrabbed);
    }

    private void Start()
    {
        // Option 1: Assign manually in inspector
        // Option 2: Find it automatically
        if (plateManager == null)
            plateManager = FindFirstObjectByType<PlateManager>();
    }

    private void OnGrabbed(SelectEnterEventArgs args)
    {
        // When grabbed off the stack
        if (plateManager != null)
            plateManager.OnIngredientRemoved(this);

        rb.isKinematic = false; // Allow free movement again
    }

    private void OnReleased(SelectExitEventArgs args)
    {
        if (plateManager != null)
        if(this.tag == "Stackable")
        {
             plateManager.TrySnap(this);
        }
    }

    public void LockInPlace(Transform newParent)
    {
        rb.isKinematic = true;
        transform.SetParent(newParent);
    }

    public void Unlock()
    {
        rb.isKinematic = false;
        transform.SetParent(null);
    }

    private void OnDestroy()
    {
        // Unsubscribe for safety
        grabInteractable.selectExited.RemoveListener(OnReleased);
        grabInteractable.selectEntered.RemoveListener(OnGrabbed);
    }
}

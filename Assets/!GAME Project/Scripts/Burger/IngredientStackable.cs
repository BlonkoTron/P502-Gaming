using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using System.Collections.Generic;
using System.Collections;

[RequireComponent(typeof(XRGrabInteractable))]
public class IngredientStackable : MonoBehaviour
{
    [Header("Snap Reference Point")]
    public Transform snapBottom; // The point that aligns with the plate's top snap point

    public Order.BurgerIngredient ingredientType;

    private XRGrabInteractable grabInteractable;
    private PlateManager plateManager;
    private Rigidbody rb;
    private GameObject originalParent;

    private void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        rb = GetComponent<Rigidbody>();
        originalParent = this.gameObject.transform.parent.gameObject;

        // Subscribe to grab and release events
        grabInteractable.selectExited.AddListener(OnReleased);
        grabInteractable.selectEntered.AddListener(OnGrabbed);
    }

    private void Start()
    {
        // Option 2: Find it automatically
        if (plateManager == null)
            plateManager = FindFirstObjectByType<PlateManager>();
    }

    private void OnGrabbed(SelectEnterEventArgs args)
    { 
        rb.isKinematic = false; // Allow free movement again

        // When grabbed off the stack
        if (plateManager != null)
            plateManager.OnIngredientRemoved(this);
    }

    public void OnReleased(SelectExitEventArgs args)
    {
        rb.isKinematic = false; // Ensure physics is enabled
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
        transform.SetParent(originalParent.transform);
    }

    private void OnDestroy()
    {
        // Unsubscribe for safety
        grabInteractable.selectExited.RemoveListener(OnReleased);
        grabInteractable.selectEntered.RemoveListener(OnGrabbed);
    }
}

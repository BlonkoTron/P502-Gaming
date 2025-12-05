using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class Pocket_bateryspawner : MonoBehaviour
{
    [Header("Settings")]
    public GameObject objectToSpawn;              // Prefab to spawn
    public Transform handTransform;               // The VR hand/controller transform
    public XRDirectInteractor directInteractor;   // The hand's XRDirectInteractor
    public InputActionProperty grabAction;        // Input Action (Grip button)
    public float spawnCooldown = 0.2f;

    private bool isInsideTrigger = false;
    private float lastSpawnTime = 0f;

    void OnEnable()
    {
        grabAction.action.performed += OnGrab;
    }

    void OnDisable()
    {
        grabAction.action.performed -= OnGrab;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerHand"))
            isInsideTrigger = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PlayerHand"))
            isInsideTrigger = false;
    }

    private void OnGrab(InputAction.CallbackContext context)
    {
        if (!isInsideTrigger) return;
        if (Time.time - lastSpawnTime < spawnCooldown) return;

        GameObject spawnedObject = Instantiate(objectToSpawn, handTransform.position, handTransform.rotation);

        // Attempt to auto-grab with the XR system
        XRGrabInteractable grabInteractable = spawnedObject.GetComponent<XRGrabInteractable>();
        if (grabInteractable != null && directInteractor != null)
        {
            // Use the interactor's interaction manager to start the grab
            var interactionManager = directInteractor.interactionManager;
            if (interactionManager != null)
            {
                // Newer API call (works in XRIT 2.3+)
                interactionManager.SelectEnter((IXRSelectInteractor)directInteractor, (IXRSelectInteractable)grabInteractable);
            }
        }

        lastSpawnTime = Time.time;
    }
}

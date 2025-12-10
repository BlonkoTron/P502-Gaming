using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class Pocket_bateryspawner : MonoBehaviour
{
    [Header("Settings")]
    public GameObject objectToSpawn;
    public Transform handTransformR;
    public Transform handTransformL;
    public XRDirectInteractor directInteractorR;
    public XRDirectInteractor directInteractorL;

    public InputActionProperty grabActionR;
    public InputActionProperty grabActionL;

    public float spawnCooldown = 0.2f;

    private bool rightHandInside = false;
    private bool leftHandInside = false;
    private float lastSpawnTime = 0f;

    void OnEnable()
    {
        grabActionR.action.performed += OnRightGrab;
        grabActionL.action.performed += OnLeftGrab;
    }

    void OnDisable()
    {
        grabActionR.action.performed -= OnRightGrab;
        grabActionL.action.performed -= OnLeftGrab;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerHand"))
            rightHandInside = true;

        if (other.CompareTag("PlayerHand"))
            leftHandInside = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PlayerHand"))
            rightHandInside = false;

        if (other.CompareTag("PlayerHand"))
            leftHandInside = false;
    }

    private void OnRightGrab(InputAction.CallbackContext ctx)
    {
        if (!rightHandInside) return;
        if (Time.time - lastSpawnTime < spawnCooldown) return;

        SpawnInHand(handTransformR, directInteractorR);
    }

    private void OnLeftGrab(InputAction.CallbackContext ctx)
    {
        if (!leftHandInside) return;
        if (Time.time - lastSpawnTime < spawnCooldown) return;

        SpawnInHand(handTransformL, directInteractorL);
    }

    private void SpawnInHand(Transform hand, XRDirectInteractor interactor)
    {
        GameObject spawnedObject = Instantiate(objectToSpawn, transform.position, transform.rotation);

        XRGrabInteractable grabInteractable = spawnedObject.GetComponent<XRGrabInteractable>();
        if (grabInteractable == null || interactor == null)
            return;

        var manager = interactor.interactionManager;
        if (manager != null)
            manager.SelectEnter((IXRSelectInteractor)interactor,(IXRSelectInteractable)grabInteractable);

        lastSpawnTime = Time.time;
    }
}

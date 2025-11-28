using UnityEngine;
using System.Collections;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using FMOD.Studio;
using FMODUnity;

public class TriggeredRaycast : MonoBehaviour
{

    private EventInstance Saucesquirt;
    [SerializeField] private EventReference saucesound;

    [SerializeField] GameObject parent;

    [Header("Raycast Settings")]
    public bool raycastActive = false;
    public float rayDistance = 10f;

    XRGrabInteractable grab;

    [Header("Sauce to Spawn")]
    public GameObject sauce;

    [Header("Cooldown Settings")]
    public float cooldown = 1f;
    private bool onCooldown = false;

    [Header("Ray Offset (Local Space)")]
    public Vector3 positionOffset = Vector3.zero;

    [Header("Rotation Offset (Local Euler Angles)")]
    public Vector3 rotationOffset = Vector3.zero;

    private void Start()
    {
        grab = GetComponent<XRGrabInteractable>();
    }

    void Update()
    {
        var rightHand = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        var leftHand = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);

        rightHand.TryGetFeatureValue(CommonUsages.trigger, out float triggerValueR);
        leftHand.TryGetFeatureValue(CommonUsages.trigger, out float triggerValueL);

        // Fire only when held & trigger squeezed
        if (grab.isSelected && !onCooldown)
        {
            if (triggerValueR > 0.1f || triggerValueL > 0.1f)
            {
                FireRaycast();
            }
        }
    }

    void FireRaycast()
    {
        RaycastHit hit;

        Vector3 origin = transform.TransformPoint(positionOffset);
        Quaternion rot = transform.rotation * Quaternion.Euler(rotationOffset);
        Vector3 direction = rot * Vector3.forward;

        if (Physics.Raycast(origin, direction, out hit, rayDistance))
        {
            PlateManager plate = hit.collider.GetComponentInParent<PlateManager>();

            if (plate != null)
            {
                // Spawn visually where the ray hits — actual snap happens after physics update
                Vector3 spawnPos = hit.point;

                SpawnSauce(plate, spawnPos);
                Saucesquirt = Audiomanager.instance.PlaySound(saucesound, transform.position);
            }
            else
            {
                StartCoroutine(CooldownRoutine());
            }
        }
        else
        {
            StartCoroutine(CooldownRoutine());
        }

        Debug.DrawRay(origin, direction * rayDistance, Color.red, 0.25f);
    }

    void SpawnSauce(PlateManager plate, Vector3 spawnPosition)
    {
        if (sauce == null)
        {
            Debug.LogError("TriggeredRaycast: Sauce prefab is not assigned!");
            StartCoroutine(CooldownRoutine());
            return;
        }

        // Instantiate normally
        GameObject sauceObj = Instantiate(sauce, spawnPosition, Quaternion.identity, parent: parent.transform);

        IngredientStackable stackable = sauceObj.GetComponent<IngredientStackable>();
        if (stackable == null)
        {
            Debug.LogError("Sauce prefab is missing IngredientStackable!");
            Destroy(sauceObj);
            StartCoroutine(CooldownRoutine());
            return;
        }

        // ---- MAIN FIX: Delay snapping until physics has updated ----
        StartCoroutine(DelayedSnap(plate, stackable));

        StartCoroutine(CooldownRoutine());
    }

    private IEnumerator DelayedSnap(PlateManager plate, IngredientStackable stackable)
    {
        // Wait for physics to stabilize bounding box
        yield return new WaitForFixedUpdate();

        // Now colliders have correct bounds → no giant offset
        plate.TrySnap(stackable);
    }

    IEnumerator CooldownRoutine()
    {
        onCooldown = true;
        yield return new WaitForSeconds(cooldown);
        onCooldown = false;
    }
}

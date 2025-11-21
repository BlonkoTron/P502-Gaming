using UnityEngine;
using System.Collections;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class TriggeredRaycast : MonoBehaviour
{
    [Header("Raycast Settings")]
    public bool raycastActive = false;      // Turn this true to fire ray once
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

    [Header("Spawn Offset (World space)")]
    public Vector3 spawnUpOffset = new Vector3(0f, 0.02f, 0f);

    private void Start()
    {
        grab = GetComponent<XRGrabInteractable>();
    }

    void Update()
    {
        var rightHand = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);

        // === BACK TRIGGER (Index trigger) ===
        if (rightHand.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))
        {
            if (triggerValue > 0.1f)
                Debug.Log("Back trigger pulled: " + triggerValue);
        }

        // If someone set the bool true, and we're not on cooldown:
        if (grab.isSelected && !onCooldown)
        {
            if (triggerValue > 0.1f)
            {
                Debug.Log("shootgoo");
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
            // Look for plate on hit object or its parents
            PlateManager plate = hit.collider.GetComponentInParent<PlateManager>();

            if (plate != null)
            {
                Vector3 spawnPos = hit.point + spawnUpOffset;
                SpawnSauce(plate, spawnPos);
            }
            else
            {
                Debug.Log("Raycast hit something, but no PlateManager found.");
                StartCoroutine(CooldownRoutine());
            }
        }
        else
        {
            Debug.Log("Raycast didn't hit anything.");
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

        // Instantiate at the hit location
        GameObject sauceObj = Instantiate(sauce, spawnPosition, Quaternion.identity);

        IngredientStackable stackable = sauceObj.GetComponent<IngredientStackable>();
        if (stackable == null)
        {
            Debug.LogError("Sauce prefab is missing IngredientStackable!");
            Destroy(sauceObj);
            StartCoroutine(CooldownRoutine());
            return;
        }

        // Snap just like a real ingredient
        plate.TrySnap(stackable);

        StartCoroutine(CooldownRoutine());
    }

    IEnumerator CooldownRoutine()
    {
        onCooldown = true;
        yield return new WaitForSeconds(cooldown);
        onCooldown = false;
    }
}

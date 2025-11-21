using UnityEngine;
using static UnityEngine.Rendering.GPUSort;

public class TriggeredRaycast : MonoBehaviour
{
    public bool raycastActive = false;
    public float rayDistance = 10f;

    public GameObject sauce;

    [Header("Tag Filter")]
    public string targetTag = "Stackable";

    [Header("Ray Offset (Local Space)")]
    public Vector3 positionOffset = Vector3.zero;

    [Header("Rotation Offset (Local Euler Angles)")]
    public Vector3 rotationOffset = Vector3.zero;

    void Update()
    {
        if (raycastActive)
        {
            FireRaycast();
            raycastActive = false;
        }
            
    }

    void FireRaycast()
    {
        RaycastHit hit;

        // Convert local position offset into world space
        Vector3 origin = transform.TransformPoint(positionOffset);

        // Apply rotation offset to direction
        Quaternion rot = transform.rotation * Quaternion.Euler(rotationOffset);
        Vector3 direction = rot * Vector3.forward;

        if (Physics.Raycast(origin, direction, out hit, rayDistance))
        {
            if (hit.collider.CompareTag(targetTag))
            {
                GameObject Sauces = Instantiate(sauce);
                Sauces.transform.position = hit.transform.position;
                IngredientStackable sb = Sauces.GetComponent<IngredientStackable>();
                sb.OnReleased(null);
                //onreleas
            }
            else
            {
                Debug.Log("No burger/snap");
            }
        }

        // Debug ray so you can see it in Scene view
        Debug.DrawRay(origin, direction * rayDistance, Color.red);
    }
}

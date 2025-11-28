using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class Respawn : MonoBehaviour
{
    private Vector3 startPosition;
    private Quaternion startRotation;
    private XRGrabInteractable grabInteractable;

    public bool Grabbed = false;

    [Header("Respawn Settings")]
    [Tooltip("Distance from start position before timer starts")]
    public float distanceThreshold = 1.0f;
    
    [Tooltip("Time in seconds before respawning")]
    public float respawnDelay = 30.0f;

    private Coroutine respawnCoroutine;

    void Start()
    {
        // Save spawn
        startPosition = transform.position;
        startRotation = transform.rotation;

        // Get XR Grab Interactable component
        grabInteractable = GetComponent<XRGrabInteractable>();
        
        if (grabInteractable != null)
        {
            // Subscribe to grab events
            grabInteractable.selectEntered.AddListener(OnGrabbed);
            grabInteractable.selectExited.AddListener(OnReleased);
        }
    }

    void OnDestroy()
    {
        // Unsubscribe from events to prevent memory leaks
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.RemoveListener(OnGrabbed);
            grabInteractable.selectExited.RemoveListener(OnReleased);
        }
    }

    private void OnGrabbed(SelectEnterEventArgs args)
    {
        Grabbed = true;
        
        // Stop respawn timer when grabbed
        if (respawnCoroutine != null)
        {
            StopCoroutine(respawnCoroutine);
            respawnCoroutine = null;
        }
    }

    private void OnReleased(SelectExitEventArgs args)
    {
        Grabbed = false;
    }

    void Update()
    {
        float distanceFromStart = Vector3.Distance(transform.position, startPosition);

        // Check if object has left the start position
        if (distanceFromStart > distanceThreshold)
        {
            // Start coroutine if not already running
            if (respawnCoroutine == null)
            {
                respawnCoroutine = StartCoroutine(RespawnTimer());
            }
        }
        else
        {
            // Stop coroutine if object returns to start position
            if (respawnCoroutine != null)
            {
                StopCoroutine(respawnCoroutine);
                respawnCoroutine = null;
            }
        }
    }

    private IEnumerator RespawnTimer()
    {
        yield return new WaitForSeconds(respawnDelay);
        
        // Only respawn if not grabbed
        if (!Grabbed)
        {
            Respawner();
        }
        
        respawnCoroutine = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check for a tag
        if (other.CompareTag("RespawnZone"))
        {
            // Reset spawn/pos/rot
            transform.position = startPosition;
            transform.rotation = startRotation;

            // Reset physics
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }

            // Stop respawn coroutine
            if (respawnCoroutine != null)
            {
                StopCoroutine(respawnCoroutine);
                respawnCoroutine = null;
            }
        }
    }


    public void Respawner()
    {
        // Reset spawn/pos/rot
        transform.position = startPosition;
        transform.rotation = startRotation;

        // Reset physics
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}



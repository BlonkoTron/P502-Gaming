using UnityEngine;

public class Respawn : MonoBehaviour
{
    private Vector3 startPosition;
    private Quaternion startRotation;

    public bool Grabbed = false;

    [Header("Respawn Settings")]
    [Tooltip("Distance from start position before timer starts")]
    public float distanceThreshold = 1.0f;
    
    [Tooltip("Time in seconds before respawning")]
    public float respawnDelay = 30.0f;

    private float timer = 0f;
    private bool hasLeftStartPosition = false;

    void Start()
    {
        // Save spawn
        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    void Update()
    {
        float distanceFromStart = Vector3.Distance(transform.position, startPosition);

        // Check if object has left the start position
        if (distanceFromStart > distanceThreshold)
        {
            if (!hasLeftStartPosition)
            {
                hasLeftStartPosition = true;
                timer = 0f;
            }

            // Increment timer
            timer += Time.deltaTime;
            
            
            // Respawn after delay
            if (timer >= respawnDelay && !Grabbed)
            {
                Respawner();
                timer = 0f;
                hasLeftStartPosition = false;
            }
            
        }
        else
        {
            // Reset timer if object returns to start position
            if (hasLeftStartPosition)
            {
                hasLeftStartPosition = false;
                timer = 0f;
            }
        }
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

            // Reset timer
            timer = 0f;
            hasLeftStartPosition = false;
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



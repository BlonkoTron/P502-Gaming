using UnityEngine;

public class TurnOnSodaMachine : MonoBehaviour
{
    [Header("Objects to Monitor")]
    [SerializeField] private Transform objectToMonitor; // The object whose rotation we're tracking
    
    [Header("Objects to Enable")]
    [SerializeField] private GameObject objectToEnable1; // First object to enable when rotation detected
    [SerializeField] private GameObject objectToEnable2; // Second object to enable when rotation detected
    
    [Header("Rotation Settings")]
    [SerializeField] private float rotationThreshold = 0.1f; // Minimum rotation change to detect (in degrees)
    
    private Quaternion previousRotation;
    private bool isRotating = false;
    private bool objectsEnabled = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Validate assignments
        if (objectToMonitor == null)
        {
            Debug.LogWarning("Object to monitor not assigned in TurnOnSodaMachine!");
        }
        else
        {
            previousRotation = objectToMonitor.rotation;
        }
        
        if (objectToEnable1 == null)
        {
            Debug.LogWarning("Object to Enable 1 not assigned in TurnOnSodaMachine!");
        }
        
        if (objectToEnable2 == null)
        {
            Debug.LogWarning("Object to Enable 2 not assigned in TurnOnSodaMachine!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (objectToMonitor != null)
        {
            CheckRotation();
        }
    }
    
    // Check if the object is rotating
    private void CheckRotation()
    {
        // Calculate the angle difference between current and previous rotation
        float angle = Quaternion.Angle(previousRotation, objectToMonitor.rotation);
        
        // Check if rotation exceeds threshold
        if (angle > rotationThreshold)
        {
            if (!isRotating)
            {
                isRotating = true;
                Debug.Log($"{objectToMonitor.name} started rotating - angle change: {angle:F2} degrees");
                HandleRotationStarted();
            }
        }
        else
        {
            if (isRotating)
            {
                isRotating = false;
                Debug.Log($"{objectToMonitor.name} stopped rotating");
                HandleRotationStopped();
            }
        }
        
        // Update previous rotation
        previousRotation = objectToMonitor.rotation;
    }
    
    // Called when rotation starts
    private void HandleRotationStarted()
    {
        if (!objectsEnabled)
        {
            EnableObjects();
            objectsEnabled = true;
        }
    }
    
    // Called when rotation stops
    private void HandleRotationStopped()
    {
        if (objectsEnabled)
        {
            DisableObjects();
            objectsEnabled = false;
        }
    }
    
    // Enable both objects
    private void EnableObjects()
    {
        if (objectToEnable1 != null)
        {
            objectToEnable1.SetActive(true);
            Debug.Log($"Enabled: {objectToEnable1.name}");
        }
        
        if (objectToEnable2 != null)
        {
            objectToEnable2.SetActive(true);
            Debug.Log($"Enabled: {objectToEnable2.name}");
        }
    }
    
    // Disable both objects
    private void DisableObjects()
    {
        if (objectToEnable1 != null)
        {
            objectToEnable1.SetActive(false);
            Debug.Log($"Disabled: {objectToEnable1.name}");
        }
        
        if (objectToEnable2 != null)
        {
            objectToEnable2.SetActive(false);
            Debug.Log($"Disabled: {objectToEnable2.name}");
        }
    }
    
    // Public method to manually check if object is rotating
    public bool IsRotating()
    {
        return isRotating;
    }
}

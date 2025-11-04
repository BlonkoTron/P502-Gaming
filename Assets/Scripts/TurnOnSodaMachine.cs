using UnityEngine;

public class TurnOnSodaMachine : MonoBehaviour
{
    [Header("Objects to Monitor")]
    [SerializeField] private Transform buttonToMonitor; // The button whose rotation value determines which object to enable
    [SerializeField] private Transform objectToRotate; // The object that needs to be actively rotating
    
    [Header("Objects to Enable")]
    [SerializeField] private GameObject objectToEnable1; // Object to enable for rotation range 1
    [SerializeField] private GameObject objectToEnable2; // Object to enable for rotation range 2
    
    [Header("Button Rotation Settings")]
    [SerializeField] private Vector3 buttonRotationAxis = Vector3.forward; // Which axis to monitor on the button (X, Y, or Z)
    [SerializeField] private float rotationValue1Min = 0f; // Minimum rotation for object 1
    [SerializeField] private float rotationValue1Max = 90f; // Maximum rotation for object 1
    [SerializeField] private float rotationValue2Min = 90f; // Minimum rotation for object 2
    [SerializeField] private float rotationValue2Max = 180f; // Maximum rotation for object 2
    
    [Header("Wheel Rotation Settings")]
    [SerializeField] private Vector3 wheelRotationAxis = Vector3.up; // Which axis to monitor on the wheel (X, Y, or Z)
    [SerializeField] private float rotationThreshold = 0.1f; // Minimum rotation change to detect active rotation
    
    private Quaternion previousRotation;
    private bool isRotating = false;
    private int currentActiveObject = 0; // 0 = none, 1 = object1, 2 = object2

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Validate assignments
        if (buttonToMonitor == null)
        {
            Debug.LogWarning("Button to monitor not assigned in TurnOnSodaMachine!");
        }
        
        if (objectToRotate == null)
        {
            Debug.LogWarning("Object to rotate not assigned in TurnOnSodaMachine!");
        }
        else
        {
            previousRotation = objectToRotate.rotation;
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
        if (buttonToMonitor != null && objectToRotate != null)
        {
            CheckActiveRotation();
            CheckRotationValue();
        }
    }
    
    // Check if the objectToRotate is actively rotating
    private void CheckActiveRotation()
    {
        // Calculate the angle difference between current and previous rotation
        float angle = Quaternion.Angle(previousRotation, objectToRotate.rotation);
        
        // Check if rotation exceeds threshold
        if (angle > rotationThreshold)
        {
            if (!isRotating)
            {
                isRotating = true;
                Debug.Log($"{objectToRotate.name} started rotating");
            }
        }
        else
        {
            if (isRotating)
            {
                isRotating = false;
                Debug.Log($"{objectToRotate.name} stopped rotating");
                // Disable all objects when rotation stops
                DisableAllObjects();
            }
        }
        
        // Update previous rotation
        previousRotation = objectToRotate.rotation;
    }
    
    // Check the current rotation value and enable appropriate objects (only if actively rotating)
    private void CheckRotationValue()
    {
        // Only enable objects if actively rotating
        if (!isRotating)
        {
            return;
        }
        
        // Get the rotation angle based on the specified axis
        float currentRotation = GetRotationOnAxis();
        
        // Normalize the angle to 0-360 range
        currentRotation = NormalizeAngle(currentRotation);
        
        // Check which rotation range the button is in
        if (IsInRange(currentRotation, rotationValue1Min, rotationValue1Max))
        {
            // Enable object 1, disable object 2
            if (currentActiveObject != 1)
            {
                EnableObject1();
                DisableObject2();
                currentActiveObject = 1;
                Debug.Log($"Button rotation at {currentRotation:F1}° - Enabled Object 1");
            }
        }
        else if (IsInRange(currentRotation, rotationValue2Min, rotationValue2Max))
        {
            // Enable object 2, disable object 1
            if (currentActiveObject != 2)
            {
                DisableObject1();
                EnableObject2();
                currentActiveObject = 2;
                Debug.Log($"Button rotation at {currentRotation:F1}° - Enabled Object 2");
            }
        }
        else
        {
            // Outside both ranges, disable both objects
            if (currentActiveObject != 0)
            {
                DisableObject1();
                DisableObject2();
                currentActiveObject = 0;
                Debug.Log($"Button rotation at {currentRotation:F1}° - Outside valid ranges");
            }
        }
    }
    
    // Get the rotation value on the specified axis for the button
    private float GetRotationOnAxis()
    {
        Vector3 eulerAngles = buttonToMonitor.localEulerAngles;
        
        if (buttonRotationAxis == Vector3.right || buttonRotationAxis.x > 0.5f)
        {
            return eulerAngles.x;
        }
        else if (buttonRotationAxis == Vector3.up || buttonRotationAxis.y > 0.5f)
        {
            return eulerAngles.y;
        }
        else // Default to Z axis
        {
            return eulerAngles.z;
        }
    }
    
    // Get the rotation value on the specified axis for the wheel
    private float GetWheelRotationOnAxis()
    {
        Vector3 eulerAngles = objectToRotate.localEulerAngles;
        
        if (wheelRotationAxis == Vector3.right || wheelRotationAxis.x > 0.5f)
        {
            return eulerAngles.x;
        }
        else if (wheelRotationAxis == Vector3.up || wheelRotationAxis.y > 0.5f)
        {
            return eulerAngles.y;
        }
        else // Default to Z axis
        {
            return eulerAngles.z;
        }
    }
    
    // Normalize angle to 0-360 range
    private float NormalizeAngle(float angle)
    {
        while (angle < 0f)
            angle += 360f;
        while (angle >= 360f)
            angle -= 360f;
        return angle;
    }
    
    // Check if a value is within a range (handles wrapping around 360)
    private bool IsInRange(float value, float min, float max)
    {
        // Normalize all values
        value = NormalizeAngle(value);
        min = NormalizeAngle(min);
        max = NormalizeAngle(max);
        
        // Handle range that wraps around 360
        if (min > max)
        {
            return value >= min || value <= max;
        }
        else
        {
            return value >= min && value <= max;
        }
    }
    
    // Enable object 1
    private void EnableObject1()
    {
        if (objectToEnable1 != null)
        {
            objectToEnable1.SetActive(true);
        }
    }
    
    // Disable object 1
    private void DisableObject1()
    {
        if (objectToEnable1 != null)
        {
            objectToEnable1.SetActive(false);
        }
    }
    
    // Enable object 2
    private void EnableObject2()
    {
        if (objectToEnable2 != null)
        {
            objectToEnable2.SetActive(true);
        }
    }
    
    // Disable object 2
    private void DisableObject2()
    {
        if (objectToEnable2 != null)
        {
            objectToEnable2.SetActive(false);
        }
    }
    
    // Disable all objects
    private void DisableAllObjects()
    {
        if (currentActiveObject != 0)
        {
            DisableObject1();
            DisableObject2();
            currentActiveObject = 0;
            Debug.Log("Rotation stopped - Disabled all objects");
        }
    }
    
    // Public method to get current rotation value
    public float GetCurrentRotation()
    {
        return buttonToMonitor != null ? GetRotationOnAxis() : 0f;
    }
}

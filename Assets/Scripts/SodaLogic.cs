using UnityEngine;

public class SodaLogic : MonoBehaviour
{
    [Header("Target Objects")]
    [SerializeField] private GameObject targetObject; // The object to enable/disable
    [SerializeField] private GameObject MoonJuice; // First object to check if enabled
    [SerializeField] private GameObject NebulaBlast; // Second object to check if enabled
    [SerializeField] private Collider specificCollider; // The specific collider to detect contact with
    
    [Header("Child Object Names")]
    [SerializeField] private string moonJuiceChildName = ""; // Name of child object to check in MoonJuice
    [SerializeField] private string nebulaBlastChildName = ""; // Name of child object to check in NebulaBlast
    
    [Header("Material Settings")]
    [SerializeField] private GameObject objectToChangeMaterial; // The object whose material will change
    [SerializeField] private Material moonJuiceMaterial; // Material to apply when MoonJuice is enabled
    [SerializeField] private Material nebulaBlastMaterial; // Material to apply when NebulaBlast is enabled
    
    [Header("Settings")]
    [SerializeField] private bool enableOnAction = true; // If true, enables object on action; if false, disables it
    [SerializeField] private float requiredContactTime = 3f; // Time in seconds that contact must be maintained
    
    private bool actionTriggered = false;
    private bool isColliding = false;
    private float currentContactTime = 3f;
    private Renderer objectRenderer; // Cached renderer component
    private GameObject moonJuiceChild; // Child object of MoonJuice to check
    private GameObject nebulaBlastChild; // Child object of NebulaBlast to check

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Find MoonJuice and NebulaBlast GameObjects if not assigned
        if (MoonJuice == null)
        {
            MoonJuice = GameObject.FindWithTag("MoonJuice");
            if (MoonJuice == null)
            {
                Debug.LogWarning("MoonJuice GameObject with tag 'MoonJuice' not found in scene!");
            }
            else
            {
                Debug.Log("Found MoonJuice GameObject: " + MoonJuice.name);
            }
        }
        
        if (NebulaBlast == null)
        {
            NebulaBlast = GameObject.FindWithTag("NebulaBlast");
            if (NebulaBlast == null)
            {
                Debug.LogWarning("NebulaBlast GameObject with tag 'NebulaBlast' not found in scene!");
            }
            else
            {
                Debug.Log("Found NebulaBlast GameObject: " + NebulaBlast.name);
            }
        }

        if (specificCollider == null)
        {
            GameObject sodaMachine = GameObject.FindWithTag("SodaMachine");
            if (sodaMachine != null)
            {
                specificCollider = sodaMachine.GetComponent<Collider>();
                if (specificCollider == null)
                {
                    Debug.LogWarning("SODA_machine found but has no Collider component!");
                }
                else
                {
                    Debug.Log("Found collider on SODA_machine: " + specificCollider.name);
                }
            }
            else
            {
                Debug.LogWarning("GameObject with tag 'SodaMachine' not found in scene!");
            }
        }
        
        // Find child objects if names are specified
        if (MoonJuice != null && !string.IsNullOrEmpty(moonJuiceChildName))
        {
            Transform childTransform = MoonJuice.transform.Find(moonJuiceChildName);
            if (childTransform != null)
            {
                moonJuiceChild = childTransform.gameObject;
                Debug.Log($"Found child '{moonJuiceChildName}' in MoonJuice");
            }
            else
            {
                Debug.LogWarning($"Child '{moonJuiceChildName}' not found in MoonJuice!");
            }
        }
        else if (MoonJuice != null)
        {
            Debug.LogWarning("MoonJuice child name not specified!");
        }
        
        if (NebulaBlast != null && !string.IsNullOrEmpty(nebulaBlastChildName))
        {
            Transform childTransform = NebulaBlast.transform.Find(nebulaBlastChildName);
            if (childTransform != null)
            {
                nebulaBlastChild = childTransform.gameObject;
                Debug.Log($"Found child '{nebulaBlastChildName}' in NebulaBlast");
            }
            else
            {
                Debug.LogWarning($"Child '{nebulaBlastChildName}' not found in NebulaBlast!");
            }
        }
        else if (NebulaBlast != null)
        {
            Debug.LogWarning("NebulaBlast child name not specified!");
        }
        
        // Ensure all objects are assigned
        if (targetObject == null)
        {
            Debug.LogWarning("Target object not assigned in SodaLogic!");
        }
        if (specificCollider == null)
        {
            Debug.LogWarning("Specific Collider not assigned in SodaLogic!");
        }
        if (objectToChangeMaterial == null)
        {
            Debug.LogWarning("Object to change material not assigned in SodaLogic!");
        }
        else
        {
            // Cache the renderer component
            objectRenderer = objectToChangeMaterial.GetComponent<Renderer>();
            if (objectRenderer == null)
            {
                Debug.LogError("Object to change material doesn't have a Renderer component!");
            }
        }
        if (moonJuiceMaterial == null)
        {
            Debug.LogWarning("MoonJuice Material not assigned in SodaLogic!");
        }
        if (nebulaBlastMaterial == null)
        {
            Debug.LogWarning("NebulaBlast Material not assigned in SodaLogic!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Check conditions and trigger action if met
        CheckConditionsAndTrigger();
    }
    
    // Check if conditions are met: either object is enabled AND collision is happening
    private void CheckConditionsAndTrigger()
    {
        bool conditionsActive = IsEitherConditionObjectActive();
        
        if (conditionsActive && isColliding && !actionTriggered)
        {
            // Increment contact time while conditions are met
            currentContactTime += Time.deltaTime;
            
            // Check if we've maintained contact for the required duration
            if (currentContactTime >= requiredContactTime)
            {
                // Log which condition object(s) are enabled
                LogActiveConditionObjects();
                
                TriggerAction();
                Debug.Log($"Contact maintained for {currentContactTime:F2} seconds - Action triggered!");
            }
        }
        else
        {
            // Reset contact time when conditions are no longer met
            if (currentContactTime > 0)
            {
                Debug.Log($"Contact broken after {currentContactTime:F2} seconds - Timer reset");
            }
            currentContactTime = 0f;
            
            if (!conditionsActive || !isColliding)
            {
                actionTriggered = false;
            }
        }
    }
    
    // Log which condition objects are currently active
    private void LogActiveConditionObjects()
    {
        // Only check child objects
        bool moonJuiceActive = moonJuiceChild != null && moonJuiceChild.activeInHierarchy;
        bool nebulaBlastActive = nebulaBlastChild != null && nebulaBlastChild.activeInHierarchy;
        
        if (moonJuiceActive && nebulaBlastActive)
        {
            Debug.Log("Both MoonJuice and NebulaBlast were enabled when timer completed");
            // If both are active, prioritize MoonJuice
            ChangeMaterialToMoonJuice();
        }
        else if (moonJuiceActive)
        {
            Debug.Log("MoonJuice was enabled when timer completed");
            ChangeMaterialToMoonJuice();
        }
        else if (nebulaBlastActive)
        {
            Debug.Log("NebulaBlast was enabled when timer completed");
            ChangeMaterialToNebulaBlast();
        }
        else
        {
            Debug.LogWarning("Timer completed but no condition objects were enabled!");
        }
    }
    
    // Change material to MoonJuice material
    private void ChangeMaterialToMoonJuice()
    {
        if (objectRenderer != null && moonJuiceMaterial != null)
        {
            objectRenderer.material = moonJuiceMaterial;
            Debug.Log($"Changed material to MoonJuice material on {objectToChangeMaterial.name}");
        }
    }
    
    // Change material to NebulaBlast material
    private void ChangeMaterialToNebulaBlast()
    {
        if (objectRenderer != null && nebulaBlastMaterial != null)
        {
            objectRenderer.material = nebulaBlastMaterial;
            Debug.Log($"Changed material to NebulaBlast material on {objectToChangeMaterial.name}");
        }
    }
    
    // Check if either of the condition objects is active/enabled
    private bool IsEitherConditionObjectActive()
    {
        // Only check child objects
        bool MoonJuiceActive = moonJuiceChild != null && moonJuiceChild.activeInHierarchy;
        bool NebulaBlastActive = nebulaBlastChild != null && nebulaBlastChild.activeInHierarchy;

        return MoonJuiceActive || NebulaBlastActive;
    }
    
    // Main method to handle the action and toggle object state
    public void TriggerAction()
    {
        if (targetObject != null)
        {
            if (enableOnAction)
            {
                targetObject.SetActive(true);
                Debug.Log($"Enabled object: {targetObject.name}");
            }
            else
            {
                targetObject.SetActive(false);
                Debug.Log($"Disabled object: {targetObject.name}");
            }
            actionTriggered = true;
        }
        else
        {
            Debug.LogError("Cannot trigger action - target object is null!");
        }
    }
    
    // Alternative method to toggle between enabled/disabled
    public void ToggleObject()
    {
        if (targetObject != null)
        {
            bool newState = !targetObject.activeInHierarchy;
            targetObject.SetActive(newState);
            Debug.Log($"Toggled object {targetObject.name} to: {(newState ? "Enabled" : "Disabled")}");
        }
    }
    
    // Method to specifically enable the object
    public void EnableObject()
    {
        if (targetObject != null)
        {
            targetObject.SetActive(true);
            Debug.Log($"Enabled object: {targetObject.name}");
        }
    }
    
    // Method to specifically disable the object
    public void DisableObject()
    {
        if (targetObject != null)
        {
            targetObject.SetActive(false);
            Debug.Log($"Disabled object: {targetObject.name}");
        }
    }
    
    // Collision detection - triggers when collision starts
    private void OnTriggerEnter(Collider other)
    {
        if (specificCollider != null && other == specificCollider)
        {
            isColliding = true;
            Debug.Log($"Collision started with specific collider: {other.name} - Timer started");
        }
    }
    
    // Collision detection - triggers when collision ends
    private void OnTriggerExit(Collider other)
    {
        if (specificCollider != null && other == specificCollider)
        {
            isColliding = false;
            Debug.Log($"Collision ended with specific collider: {other.name} after {currentContactTime:F2} seconds");
            currentContactTime = 0f; // Reset timer
            actionTriggered = false; // Allow action to trigger again when collision resumes
        }
    }
    
    // Alternative collision detection using OnCollisionEnter (for non-trigger colliders)
    private void OnCollisionEnter(Collision collision)
    {
        if (specificCollider != null && collision.collider == specificCollider)
        {
            isColliding = true;
            Debug.Log($"Collision started with specific collider: {collision.gameObject.name} - Timer started");
        }
    }
    
    // Alternative collision detection using OnCollisionExit (for non-trigger colliders)
    private void OnCollisionExit(Collision collision)
    {
        if (specificCollider != null && collision.collider == specificCollider)
        {
            isColliding = false;
            Debug.Log($"Collision ended with specific collider: {collision.gameObject.name} after {currentContactTime:F2} seconds");
            currentContactTime = 0f; // Reset timer
            actionTriggered = false; // Allow action to trigger again when collision resumes
        }
    }
}

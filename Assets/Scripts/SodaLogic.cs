using System.IO;
using UnityEngine;

/// <summary>
/// Manages soda dispensing logic in a VR game environment.
/// Detects when a cup enters specific zones (MoonJuice or NebulaBlast),
/// checks if the corresponding drink is enabled, and after maintaining
/// contact for a required duration, dispenses the drink by changing
/// the cup's material and enabling/disabling target objects.
/// </summary>
public class SodaLogic : MonoBehaviour
{
    // ===== INSPECTOR-ASSIGNED REFERENCES =====
    [Header("Target Objects")]
    [SerializeField] private GameObject targetObject; // The object to enable/disable when action is triggered (e.g., soda dispenser effect)
    [SerializeField] private GameObject MoonJuice; // Parent GameObject for MoonJuice drink option
    [SerializeField] private GameObject NebulaBlast; // Parent GameObject for NebulaBlast drink option
    [SerializeField] private Collider specificCollider; // Legacy: specific collider to detect contact with (for backward compatibility)
    [SerializeField] private Collider NebulaBlastColliderZone; // Collider zone that triggers NebulaBlast dispensing
    [SerializeField] private Collider MoonJuiceColliderZone; // Collider zone that triggers MoonJuice dispensing

    
    [Header("Child Object Names")]
    [SerializeField] private string moonJuiceChildName = ""; // Name of child object within MoonJuice that indicates if this drink is enabled
    [SerializeField] private string nebulaBlastChildName = ""; // Name of child object within NebulaBlast that indicates if this drink is enabled
    
    [Header("Material Settings")]
    [SerializeField] private GameObject objectToChangeMaterial; // The cup/container object whose material will change to match the drink
    [SerializeField] private Material moonJuiceMaterial; // Material to apply to cup when MoonJuice is dispensed
    [SerializeField] private Material nebulaBlastMaterial; // Material to apply to cup when NebulaBlast is dispensed
    
    [Header("Settings")]
    [SerializeField] private bool enableOnAction = true; // If true, enables targetObject on action; if false, disables it
    [SerializeField] private float requiredContactTime = 3f; // Duration (in seconds) that the cup must stay in the zone to trigger dispensing
    
    // ===== PRIVATE STATE VARIABLES =====
    private bool actionTriggered = false; // Tracks if the dispense action has been triggered to prevent repeated triggers
    private bool isColliding = false; // Legacy: tracks collision with specificCollider
    private float currentContactTime = 3f; // Accumulator for time the cup has been in the zone
    private Renderer objectRenderer; // Cached reference to the Renderer component for material changes
    private GameObject moonJuiceChild; // Reference to the found child object in MoonJuice
    private GameObject nebulaBlastChild; // Reference to the found child object in NebulaBlast
    private bool isInMoonJuiceZone = false; // Tracks if the cup is currently in the MoonJuice zone
    private bool isInNebulaBlastZone = false; // Tracks if the cup is currently in the NebulaBlast zone

    // ===== PUBLIC PROPERTIES =====
    public Order.Drink drinkType; // The type of drink that was dispensed (used for order validation)


    /// <summary>
    /// Initializes the script by finding required GameObjects and Colliders via tags,
    /// locating child objects, and validating all necessary references.
    /// This runs once when the GameObject is created.
    /// </summary>
    void Start()
    {
        // ===== FIND COLLIDER ZONES =====
        // Auto-find MoonJuice zone collider if not assigned in inspector
        if (MoonJuiceColliderZone == null)
        {
            MoonJuiceColliderZone = GameObject.FindWithTag("MoonJuiceZone")?.GetComponent<Collider>();
            if (MoonJuiceColliderZone == null)
            {
                Debug.LogWarning("MoonJuiceColliderZone with tag 'MoonJuiceZone' not found in scene!");
            }
            else
            {
                Debug.Log("Found MoonJuiceColliderZone: " + MoonJuiceColliderZone.name);
            }
        }

        if (NebulaBlastColliderZone == null)
        {
            NebulaBlastColliderZone = GameObject.FindWithTag("NebulaBlastZone")?.GetComponent<Collider>();
            if (NebulaBlastColliderZone == null)
            {
                Debug.LogWarning("NebulaBlastColliderZone with tag 'NebulaBlastZone' not found in scene!");
            }
            else
            {
                Debug.Log("Found NebulaBlastColliderZone: " + NebulaBlastColliderZone.name);
            }
        }

        // ===== FIND DRINK GAMEOBJECTS =====
        // Auto-find MoonJuice and NebulaBlast GameObjects if not assigned in inspector
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

        // ===== FIND SODA MACHINE COLLIDER (LEGACY) =====
        // Auto-find the soda machine collider for backward compatibility
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
        // ===== LOCATE CHILD OBJECTS =====
        // Find specific child objects within MoonJuice and NebulaBlast that determine if each drink is available
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
        // ===== VALIDATE REFERENCES =====
        // Check that all required references are properly assigned and log warnings if missing
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

    /// <summary>
    /// Called every frame. Continuously checks if the cup is in a zone,
    /// if the corresponding drink is enabled, and manages the contact timer.
    /// </summary>
    void Update()
    {
        // Check conditions and trigger action if met
        CheckConditionsAndTrigger();
    }
    
    /// <summary>
    /// Checks if the cup is in a valid zone AND the corresponding drink's child object is enabled.
    /// If conditions are met, increments the contact timer. Once the required time is reached,
    /// triggers the drink dispensing action and changes the cup's material.
    /// Resets the timer if the cup leaves the zone or the drink becomes unavailable.
    /// </summary>
    private void CheckConditionsAndTrigger()
    {
        // Check if MoonJuice conditions are met: child object exists, is active, and cup is in the zone
        bool moonJuiceConditionMet = moonJuiceChild != null && moonJuiceChild.activeInHierarchy && isInMoonJuiceZone;
        // Check if NebulaBlast conditions are met: child object exists, is active, and cup is in the zone
        bool nebulaBlastConditionMet = nebulaBlastChild != null && nebulaBlastChild.activeInHierarchy && isInNebulaBlastZone;
        // At least one drink must have its conditions met for dispensing to occur
        bool conditionsActive = moonJuiceConditionMet || nebulaBlastConditionMet;
        
        if (conditionsActive && !actionTriggered)
        {
            // Conditions are met: increment the contact timer
            currentContactTime += Time.deltaTime;
            
            // Check if we've maintained contact for the required duration
            if (currentContactTime >= requiredContactTime)
            {
                // Determine which drink to dispense and change the cup's material accordingly
                LogActiveConditionObjects();
                
                // Enable/disable the target object (e.g., visual effects for dispensing)
                TriggerAction();
                Debug.Log($"Contact maintained for {currentContactTime:F2} seconds - Action triggered!");
            }
        }
        else
        {
            // Conditions no longer met (cup left zone or drink disabled): reset the timer
            if (currentContactTime > 0)
            {
                Debug.Log($"Contact broken after {currentContactTime:F2} seconds - Timer reset");
            }
            currentContactTime = 0f;
            
            // Allow the action to trigger again when conditions are met again
            if (!conditionsActive || !isColliding)
            {
                actionTriggered = false;
            }
        }
    }
    
    /// <summary>
    /// Determines which drink zone and child object combination is active,
    /// logs the information, and triggers the appropriate material change.
    /// If both conditions are met, MoonJuice takes priority.
    /// </summary>
    private void LogActiveConditionObjects()
    {
        // Check which zone the object is in and which child is active
        bool moonJuiceConditionMet = moonJuiceChild != null && moonJuiceChild.activeInHierarchy && isInMoonJuiceZone;
        bool nebulaBlastConditionMet = nebulaBlastChild != null && nebulaBlastChild.activeInHierarchy && isInNebulaBlastZone;
        
        if (moonJuiceConditionMet && nebulaBlastConditionMet)
        {
            Debug.Log("Both MoonJuice zone and NebulaBlast zone conditions met - prioritizing MoonJuice");
            ChangeMaterialToMoonJuice();
        }
        else if (moonJuiceConditionMet)
        {
            Debug.Log("MoonJuice condition met: in MoonJuice zone with MoonJuice child enabled");
            ChangeMaterialToMoonJuice();
        }
        else if (nebulaBlastConditionMet)
        {
            Debug.Log("NebulaBlast condition met: in NebulaBlast zone with NebulaBlast child enabled");
            ChangeMaterialToNebulaBlast();
        }
        else
        {
            Debug.LogWarning("Timer completed but no zone/child condition was met!");
        }
    }
    
    /// <summary>
    /// Changes the cup's material to the MoonJuice material and sets the drink type.
    /// This gives visual feedback to the player that MoonJuice was dispensed.
    /// </summary>
    private void ChangeMaterialToMoonJuice()
    {
        if (objectRenderer != null && moonJuiceMaterial != null)
        {
            objectRenderer.material = moonJuiceMaterial;
            drinkType = Order.Drink.Moon_juice;
            Debug.Log($"Changed material to MoonJuice material on {objectToChangeMaterial.name}");
        }
    }
    
    /// <summary>
    /// Changes the cup's material to the NebulaBlast material and sets the drink type.
    /// This gives visual feedback to the player that NebulaBlast was dispensed.
    /// </summary>
    private void ChangeMaterialToNebulaBlast()
    {
        if (objectRenderer != null && nebulaBlastMaterial != null)
        {
            objectRenderer.material = nebulaBlastMaterial;
            drinkType = Order.Drink.Nebula_blast;
            Debug.Log($"Changed material to NebulaBlast material on {objectToChangeMaterial.name}");
        }
    }
    
    /// <summary>
    /// Checks if at least one of the drink child objects (MoonJuice or NebulaBlast) is active.
    /// Returns true if either drink is currently available for dispensing.
    /// </summary>
    /// <returns>True if at least one drink's child object is active in the hierarchy</returns>
    private bool IsEitherConditionObjectActive()
    {
        // Only check child objects
        bool MoonJuiceActive = moonJuiceChild != null && moonJuiceChild.activeInHierarchy;
        bool NebulaBlastActive = nebulaBlastChild != null && nebulaBlastChild.activeInHierarchy;

        return MoonJuiceActive || NebulaBlastActive;
    }
    
    /// <summary>
    /// Main action method that enables or disables the target object based on the enableOnAction setting.
    /// Typically used to activate visual effects or sounds when a drink is dispensed.
    /// Marks the action as triggered to prevent repeated triggers.
    /// </summary>
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
    
    /// <summary>
    /// Toggles the target object between enabled and disabled states.
    /// Useful for manual control or testing purposes.
    /// </summary>
    public void ToggleObject()
    {
        if (targetObject != null)
        {
            bool newState = !targetObject.activeInHierarchy;
            targetObject.SetActive(newState);
            Debug.Log($"Toggled object {targetObject.name} to: {(newState ? "Enabled" : "Disabled")}");
        }
    }
    
    /// <summary>
    /// Explicitly enables the target object.
    /// Can be called from external scripts or Unity Events.
    /// </summary>
    public void EnableObject()
    {
        if (targetObject != null)
        {
            targetObject.SetActive(true);
            Debug.Log($"Enabled object: {targetObject.name}");
        }
    }
    
    /// <summary>
    /// Explicitly disables the target object.
    /// Can be called from external scripts or Unity Events.
    /// </summary>
    public void DisableObject()
    {
        if (targetObject != null)
        {
            targetObject.SetActive(false);
            Debug.Log($"Disabled object: {targetObject.name}");
        }
    }
    
    /// <summary>
    /// Detects when the cup enters a trigger collider zone.
    /// Sets the appropriate zone flag (isInMoonJuiceZone or isInNebulaBlastZone)
    /// to indicate which drink zone the cup has entered.
    /// </summary>
    /// <param name="other">The collider that was entered</param>
    private void OnTriggerEnter(Collider other)
    {
        if (MoonJuiceColliderZone != null && other == MoonJuiceColliderZone)
        {
            isInMoonJuiceZone = true;
            Debug.Log($"Entered MoonJuice zone: {other.name} - Timer started");
        }
        else if (NebulaBlastColliderZone != null && other == NebulaBlastColliderZone)
        {
            isInNebulaBlastZone = true;
            Debug.Log($"Entered NebulaBlast zone: {other.name} - Timer started");
        }
        
        // Maintain backward compatibility with specificCollider
        if (specificCollider != null && other == specificCollider)
        {
            isColliding = true;
            Debug.Log($"Collision started with specific collider: {other.name} - Timer started");
        }
    }
    
    /// <summary>
    /// Detects when the cup exits a trigger collider zone.
    /// Clears the appropriate zone flag and resets the contact timer,
    /// allowing the action to be triggered again when the cup re-enters.
    /// </summary>
    /// <param name="other">The collider that was exited</param>
    private void OnTriggerExit(Collider other)
    {
        if (MoonJuiceColliderZone != null && other == MoonJuiceColliderZone)
        {
            isInMoonJuiceZone = false;
            Debug.Log($"Exited MoonJuice zone: {other.name} after {currentContactTime:F2} seconds");
            currentContactTime = 0f; // Reset timer
            actionTriggered = false; // Allow action to trigger again when collision resumes
        }
        else if (NebulaBlastColliderZone != null && other == NebulaBlastColliderZone)
        {
            isInNebulaBlastZone = false;
            Debug.Log($"Exited NebulaBlast zone: {other.name} after {currentContactTime:F2} seconds");
            currentContactTime = 0f; // Reset timer
            actionTriggered = false; // Allow action to trigger again when collision resumes
        }
        
        // Maintain backward compatibility with specificCollider
        if (specificCollider != null && other == specificCollider)
        {
            isColliding = false;
            Debug.Log($"Collision ended with specific collider: {other.name} after {currentContactTime:F2} seconds");
            currentContactTime = 0f; // Reset timer
            actionTriggered = false; // Allow action to trigger again when collision resumes
        }
    }
    
    /// <summary>
    /// Alternative collision detection for non-trigger colliders.
    /// Functions identically to OnTriggerEnter but for solid collisions.
    /// Provides flexibility for different collider configurations.
    /// </summary>
    /// <param name="collision">The collision information</param>
    private void OnCollisionEnter(Collision collision)
    {
        if (MoonJuiceColliderZone != null && collision.collider == MoonJuiceColliderZone)
        {
            isInMoonJuiceZone = true;
            Debug.Log($"Entered MoonJuice zone: {collision.gameObject.name} - Timer started");
        }
        else if (NebulaBlastColliderZone != null && collision.collider == NebulaBlastColliderZone)
        {
            isInNebulaBlastZone = true;
            Debug.Log($"Entered NebulaBlast zone: {collision.gameObject.name} - Timer started");
        }
        
        // Maintain backward compatibility with specificCollider
        if (specificCollider != null && collision.collider == specificCollider)
        {
            isColliding = true;
            Debug.Log($"Collision started with specific collider: {collision.gameObject.name} - Timer started");
        }
    }
    
    /// <summary>
    /// Alternative collision detection for non-trigger colliders.
    /// Functions identically to OnTriggerExit but for solid collisions.
    /// Provides flexibility for different collider configurations.
    /// </summary>
    /// <param name="collision">The collision information</param>
    private void OnCollisionExit(Collision collision)
    {
        if (MoonJuiceColliderZone != null && collision.collider == MoonJuiceColliderZone)
        {
            isInMoonJuiceZone = false;
            Debug.Log($"Exited MoonJuice zone: {collision.gameObject.name} after {currentContactTime:F2} seconds");
            currentContactTime = 0f; // Reset timer
            actionTriggered = false; // Allow action to trigger again when collision resumes
        }
        else if (NebulaBlastColliderZone != null && collision.collider == NebulaBlastColliderZone)
        {
            isInNebulaBlastZone = false;
            Debug.Log($"Exited NebulaBlast zone: {collision.gameObject.name} after {currentContactTime:F2} seconds");
            currentContactTime = 0f; // Reset timer
            actionTriggered = false; // Allow action to trigger again when collision resumes
        }
        
        // Maintain backward compatibility with specificCollider
        if (specificCollider != null && collision.collider == specificCollider)
        {
            isColliding = false;
            Debug.Log($"Collision ended with specific collider: {collision.gameObject.name} after {currentContactTime:F2} seconds");
            currentContactTime = 0f; // Reset timer
            actionTriggered = false; // Allow action to trigger again when collision resumes
        }
    }
}

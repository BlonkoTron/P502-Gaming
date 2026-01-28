using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using FMOD.Studio;
using FMODUnity;

/// <summary>
/// Controls a soda machine that can dispense two different drinks (MoonJuice and NebulaBlast).
/// When a drink is dispensed, it enables a visual object (particle effects, liquid stream, etc.)
/// for a set duration, then automatically disables it. Only one drink can be active at a time.
/// </summary>
public class TurnOnSodaMachine : MonoBehaviour
{
    [Header("Objects to Enable")]
    [SerializeField] private GameObject objectToEnable1; // Object to enable when event 1 is triggered
    [SerializeField] private GameObject objectToEnable2; // Object to enable when event 2 is triggered
    
    [Header("Enable Duration")]
    [SerializeField] private float enableDuration = 4f; // Duration in seconds to keep objects enabled
    
    [Header("Unity Events")]
    [Tooltip("Subscribe to this event to enable object 1")]
    public UnityEvent onEnableMoonJuice;
    [Tooltip("Subscribe to this event to enable object 2")]
    public UnityEvent onEnableNebulaBlast;
    [Tooltip("Subscribe to this event to disable all objects")]
    public UnityEvent onDisableAll;
    
    // Tracks which object is currently active: 0 = none, 1 = MoonJuice, 2 = NebulaBlast
    private int currentActiveObject = 0;
    
    // Reference to the running coroutine that will disable objects after the duration expires
    private Coroutine disableCoroutine;

    // FMOD sound instance for the pouring sound effect
    private EventInstance SodaSound;
    
    // Reference to the FMOD event for the soda pouring sound
    [SerializeField] private EventReference SodaPour;

    /// <summary>
    /// Updates the 3D position of the soda pouring sound every frame.
    /// This ensures the sound follows the soda machine if it moves.
    /// </summary>
    private void Update()
    {
        Audiomanager.instance.UpdateSoundPosition(SodaSound, transform.position);
    }
    /// <summary>
    /// Called when the script is enabled. Sets up event listeners.
    /// Initializes Unity Events if they're null and subscribes handler methods.
    /// </summary>
    void OnEnable()
    {
        // Initialize Unity Events if they haven't been created yet
        if (onEnableMoonJuice == null)
            onEnableMoonJuice = new UnityEvent();
        if (onEnableNebulaBlast == null)
            onEnableNebulaBlast = new UnityEvent();
        if (onDisableAll == null)
            onDisableAll = new UnityEvent();
        
        // Subscribe handler methods to the events so they'll be called when the events fire
        onEnableMoonJuice.AddListener(HandleEnableObject1);
        onEnableNebulaBlast.AddListener(HandleEnableObject2);
        onDisableAll.AddListener(HandleDisableAll);
    }

    /// <summary>
    /// Called when the script is disabled. Cleans up event listeners.
    /// This prevents memory leaks by removing subscriptions when the script is no longer active.
    /// </summary>
    void OnDisable()
    {
        // Unsubscribe from Unity Events to prevent memory leaks
        onEnableMoonJuice.RemoveListener(HandleEnableObject1);
        onEnableNebulaBlast.RemoveListener(HandleEnableObject2);
        onDisableAll.RemoveListener(HandleDisableAll);
    }
    
    /// <summary>
    /// Handles the event to dispense MoonJuice (object 1).
    /// Can be called from Unity Events (like UI buttons) or other scripts.
    /// </summary>
    public void HandleEnableObject1()
    {
        // Only proceed if MoonJuice isn't already being dispensed
        if (currentActiveObject != 1)
        {
            EnableObject1();              // Show the MoonJuice dispense effect
            DisableObject2();             // Hide the NebulaBlast effect if it was active
            currentActiveObject = 1;       // Mark MoonJuice as the active drink
            
            // Reset the timer: stop any running disable timer and start a new one
            // This ensures the effect stays visible for the full duration
            if (disableCoroutine != null)
            {
                StopCoroutine(disableCoroutine);
            }
            disableCoroutine = StartCoroutine(DisableAfterDelay());
        }
    }
    
    /// <summary>
    /// Handles the event to dispense NebulaBlast (object 2).
    /// Can be called from Unity Events (like UI buttons) or other scripts.
    /// </summary>
    public void HandleEnableObject2()
    {
        // Only proceed if NebulaBlast isn't already being dispensed
        if (currentActiveObject != 2)
        {
            DisableObject1();             // Hide the MoonJuice effect if it was active
            EnableObject2();              // Show the NebulaBlast dispense effect
            currentActiveObject = 2;       // Mark NebulaBlast as the active drink
            
            // Reset the timer: stop any running disable timer and start a new one
            // This ensures the effect stays visible for the full duration
            if (disableCoroutine != null)
            {
                StopCoroutine(disableCoroutine);
            }
            disableCoroutine = StartCoroutine(DisableAfterDelay());
        }
    }
    
    /// <summary>
    /// Immediately stops all dispensing effects and cleans up.
    /// Can be called from Unity Events or other scripts to cancel the current dispense.
    /// </summary>
    public void HandleDisableAll()
    {
        // Cancel the automatic disable timer if one is running
        if (disableCoroutine != null)
        {
            StopCoroutine(disableCoroutine);
            disableCoroutine = null;
        }
        // Turn off both dispense effects immediately
        DisableAllObjects();
    }
    
    /// <summary>
    /// Coroutine that waits for the specified duration, then automatically turns off all effects.
    /// This creates the timed dispense behavior where effects show for a few seconds then stop.
    /// </summary>
    private IEnumerator DisableAfterDelay()
    {
        // Wait for the configured duration (default 4 seconds)
        yield return new WaitForSeconds(enableDuration);
        
        // Time's up - turn off all dispense effects
        DisableAllObjects();
        disableCoroutine = null;  // Clear the reference since the coroutine is done
    }
    
    /// <summary>
    /// Activates the MoonJuice dispense effect (object 1) and plays the pouring sound.
    /// </summary>
    private void EnableObject1()
    {
        if (objectToEnable1 != null)
        {
            // Play the soda pouring sound at the machine's location
            SodaSound = Audiomanager.instance.PlaySound(SodaPour, transform.position);
            // Show the visual effect (particle system, liquid mesh, etc.)
            objectToEnable1.SetActive(true);
        }
    }
    
    /// <summary>
    /// Deactivates the MoonJuice dispense effect (object 1).
    /// </summary>
    private void DisableObject1()
    {
        if (objectToEnable1 != null)
        {
            // Hide the visual effect
            objectToEnable1.SetActive(false);
        }
    }
    
    /// <summary>
    /// Activates the NebulaBlast dispense effect (object 2) and plays the pouring sound.
    /// </summary>
    private void EnableObject2()
    {
        if (objectToEnable2 != null)
        {
            // Play the soda pouring sound at the machine's location
            SodaSound = Audiomanager.instance.PlaySound(SodaPour, transform.position);
            // Show the visual effect (particle system, liquid mesh, etc.)
            objectToEnable2.SetActive(true);
        }
    }
    
    /// <summary>
    /// Deactivates the NebulaBlast dispense effect (object 2).
    /// </summary>
    private void DisableObject2()
    {
        if (objectToEnable2 != null)
        {
            // Hide the visual effect
            objectToEnable2.SetActive(false);
        }
    }
    
    /// <summary>
    /// Deactivates all dispense effects and resets the machine to idle state.
    /// </summary>
    private void DisableAllObjects()
    {
        // Only do work if something is actually active
        if (currentActiveObject != 0)
        {
            DisableObject1();         // Turn off MoonJuice effect
            DisableObject2();         // Turn off NebulaBlast effect
            currentActiveObject = 0;   // Mark machine as idle (nothing active)
        }
    }
}

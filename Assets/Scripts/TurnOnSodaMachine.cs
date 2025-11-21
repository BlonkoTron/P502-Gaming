using UnityEngine;
using UnityEngine.Events;
using System.Collections;

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
    
    private int currentActiveObject = 0; // 0 = none, 1 = object1, 2 = object2
    private Coroutine disableCoroutine;

    void OnEnable()
    {
        // Subscribe to Unity Events
        if (onEnableMoonJuice == null)
            onEnableMoonJuice = new UnityEvent();
        if (onEnableNebulaBlast == null)
            onEnableNebulaBlast = new UnityEvent();
        if (onDisableAll == null)
            onDisableAll = new UnityEvent();
            
        onEnableMoonJuice.AddListener(HandleEnableObject1);
        onEnableNebulaBlast.AddListener(HandleEnableObject2);
        onDisableAll.AddListener(HandleDisableAll);
    }

    void OnDisable()
    {
        // Unsubscribe from Unity Events
        onEnableMoonJuice.RemoveListener(HandleEnableObject1);
        onEnableNebulaBlast.RemoveListener(HandleEnableObject2);
        onDisableAll.RemoveListener(HandleDisableAll);
    }
    
    // Public methods that can be called from Unity Events or other scripts
    public void HandleEnableObject1()
    {
        if (currentActiveObject != 1)
        {
            EnableObject1();
            DisableObject2();
            currentActiveObject = 1;
            
            // Stop any existing disable coroutine and start a new one
            if (disableCoroutine != null)
            {
                StopCoroutine(disableCoroutine);
            }
            disableCoroutine = StartCoroutine(DisableAfterDelay());
        }
    }
    
    public void HandleEnableObject2()
    {
        if (currentActiveObject != 2)
        {
            DisableObject1();
            EnableObject2();
            currentActiveObject = 2;
            
            // Stop any existing disable coroutine and start a new one
            if (disableCoroutine != null)
            {
                StopCoroutine(disableCoroutine);
            }
            disableCoroutine = StartCoroutine(DisableAfterDelay());
        }
    }
    
    public void HandleDisableAll()
    {
        // Stop any existing disable coroutine
        if (disableCoroutine != null)
        {
            StopCoroutine(disableCoroutine);
            disableCoroutine = null;
        }
        DisableAllObjects();
    }
    
    // Coroutine to disable objects after the specified duration
    private IEnumerator DisableAfterDelay()
    {
        yield return new WaitForSeconds(enableDuration);
        DisableAllObjects();
        disableCoroutine = null;
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
        }
    }
}

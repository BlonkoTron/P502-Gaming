using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using static UnityEngine.ParticleSystem;
using static UnityEngine.XR.ARSubsystems.XRCpuImage;

public class IngredientBasics : MonoBehaviour
{
    public string ingredientName;
    public bool isStackable;
    public GameObject bottomSnapPoint;
    private XRBaseInteractable interactable;

    private void Awake()
    {
        interactable = GetComponent<XRBaseInteractable>();
    }

    private void OnEnable()
    {
        // Subscribe to select event (when picked up)
        interactable.selectEntered.AddListener(OnSelectEntered);
        interactable.selectExited.AddListener(OnSelectExited);
    }

    private void OnDisable()
    {
        // Un-subscribe to select event (when let go)
        interactable.selectEntered.RemoveListener(OnSelectEntered);
        interactable.selectExited.RemoveListener(OnSelectExited);
    }

    //When picked up we activate the snap points if they are inactive
    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        if(bottomSnapPoint.activeSelf == false)
        {
            bottomSnapPoint.SetActive(true);
        }
    }

    private void OnSelectExited(SelectExitEventArgs args)
    {
        
    }

}

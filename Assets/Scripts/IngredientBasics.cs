using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using static UnityEngine.ParticleSystem;
using static UnityEngine.XR.ARSubsystems.XRCpuImage;

public class IngredientBasics : MonoBehaviour
{
    public string ingredientName;
    public bool isStackable;
    public GameObject topSnapPoint;
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
        interactable.selectEntered.RemoveListener(OnSelectEntered);
        interactable.selectExited.RemoveListener(OnSelectExited);
    }

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        if(bottomSnapPoint.activeSelf == false)
        {
            bottomSnapPoint.SetActive(true);
        }
        if(topSnapPoint.activeSelf == false)
        {
            topSnapPoint.SetActive(true);
        }
    }

    private void OnSelectExited(SelectExitEventArgs args)
    {
        
    }

}

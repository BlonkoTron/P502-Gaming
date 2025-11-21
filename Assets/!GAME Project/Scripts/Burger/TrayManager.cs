using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class TrayManager : MonoBehaviour
{
    public static TrayManager Instance;

    private GameObject plateOnTray,sodaOnTray;

    [SerializeField] private XRSocketInteractor plateSocket,sodaSocket;

    public UnityEvent<GameObject> OnPlateAddedToTray;
    public UnityEvent<GameObject> OnSodaAddedToTray;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    private void Start()
    {
        plateSocket.selectEntered.AddListener(SetPlateOnTray);
        plateSocket.selectExited.AddListener(RemovePlateOnTray);
        sodaSocket.selectEntered.AddListener(SetSodaOnTray);
        sodaSocket.selectExited.AddListener(RemoveSodaOnTray);
    }
    private void OnDestroy()
    {
        plateSocket.selectEntered.RemoveListener(SetPlateOnTray);
        plateSocket.selectExited.RemoveListener(RemovePlateOnTray);
        sodaSocket.selectEntered.RemoveListener(SetSodaOnTray);
        sodaSocket.selectExited.RemoveListener(RemoveSodaOnTray);
    }

    private void RemoveSodaOnTray(SelectExitEventArgs arg0)
    {
        sodaOnTray = null;
    }

    private void SetSodaOnTray(SelectEnterEventArgs arg0)
    {
        sodaOnTray = arg0.interactableObject.transform.gameObject;
        Debug.Log(sodaOnTray);
        OnSodaAddedToTray.Invoke(sodaOnTray);
    }

    private void RemovePlateOnTray(SelectExitEventArgs arg0)
    {
        plateOnTray = null;
    }

    private void SetPlateOnTray(SelectEnterEventArgs arg0)
    {
        plateOnTray = arg0.interactableObject.transform.gameObject;
        Debug.Log(plateOnTray);
        OnPlateAddedToTray.Invoke(plateOnTray);
    }
    public List<Order.BurgerIngredient> GetBurgerOnTray()
    {
        if (plateOnTray!=null)
        {
            return plateOnTray.GetComponent<PlateManager>().GetBurgerIngredients();
        }
        return null;
    }
    public Order.Drink GetDrinkOnTray()
    {
        if (sodaOnTray!=null)
        {
            // get the type of drink
        }
        return Order.Drink.none;
    }

}

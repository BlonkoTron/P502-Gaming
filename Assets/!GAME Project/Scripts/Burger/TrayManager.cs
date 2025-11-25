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
        sodaSocket.selectEntered.AddListener(SetSodaOnTray);
        sodaSocket.selectExited.AddListener(RemoveSodaOnTray);
        OrderController.Instance.OnOrderFullfilled.AddListener(ClearTray);
        plateOnTray = FindAnyObjectByType<PlateManager>().gameObject;
    }
    private void OnDestroy()
    {
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
    private void ClearTray(bool fullfilled)
    {
        Destroy(sodaOnTray);
        var plateChild = plateOnTray.GetComponentInChildren<Transform>();
        foreach (Transform child in plateChild)
        {
            if (child.CompareTag("Stackable"))
            {
                Destroy(child.gameObject);
            }
        }
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

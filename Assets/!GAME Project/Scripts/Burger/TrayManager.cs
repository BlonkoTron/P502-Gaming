using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class TrayManager : MonoBehaviour
{
    public static TrayManager Instance;

    private GameObject plateOnTray,sodaOnTray;

    [SerializeField] private XRSocketInteractor plateSocket,sodaSocket;

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

    private void RemoveSodaOnTray(SelectExitEventArgs arg0)
    {
        sodaOnTray = null;
    }

    private void SetSodaOnTray(SelectEnterEventArgs arg0)
    {
        sodaOnTray = arg0.interactableObject.transform.gameObject;
        Debug.Log(sodaOnTray);
    }

    private void RemovePlateOnTray(SelectExitEventArgs arg0)
    {
        plateOnTray = null;
    }

    private void SetPlateOnTray(SelectEnterEventArgs arg0)
    {
        plateOnTray = arg0.interactableObject.transform.gameObject;
        Debug.Log(plateOnTray);
    }
}

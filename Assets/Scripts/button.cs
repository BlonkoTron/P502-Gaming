using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonVR : MonoBehaviour
{
    public GameObject button;
    public bool isPressed;

    void Start()
    {
        isPressed = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        button.transform.localPosition = new Vector3(0, 0.003f, 0);
        isPressed = true;
    }

    private void OnTriggerExit(Collider other)
    {
        button.transform.localPosition = new Vector3(0, 0.015f, 0);
        isPressed = false;
    }
}

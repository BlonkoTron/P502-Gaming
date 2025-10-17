using System.Collections;
using UnityEngine;

public class CookBeef : MonoBehaviour
{
    public Material Raw;
    public Material Cooked;
    public float DesiredCooktime;
    private float Cooktime;

    [SerializeField]
    private float CookTimeRemain;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameObject.GetComponent<MeshRenderer>().material = Raw;
        Cooktime = DesiredCooktime;
    }

    private void OnTriggerStay (Collider other)
    {
        if (other.CompareTag("Grill"))
        {
            CookTimeRemain = DesiredCooktime--;

            if (CookTimeRemain <= 0)
            {
                gameObject.GetComponent<MeshRenderer>().material = Cooked;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        DesiredCooktime = Cooktime;
    }
}

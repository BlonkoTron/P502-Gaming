using System.Collections;
using UnityEngine;

public class CookBeef : MonoBehaviour
{
    public Material Raw;
    public Material Cooked;
    public float Cooktime = 0;

    [SerializeField]
    private float CookTimeRemain = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameObject.GetComponent<MeshRenderer>().material = Raw;
    }

    private void Update()
    {
        
    }
    private void OnTriggerStay (Collider other)
    {
        if (other.CompareTag("Grill"))
        {
            CookTimeRemain = Cooktime--;

            if (CookTimeRemain <= 0)
            {
                CookTimeRemain = 0;
                Cooktime = 0;
                gameObject.GetComponent<MeshRenderer>().material = Cooked;
            }
        }
        else
        {
            CookTimeRemain = Cooktime;
        }
    }
}

using Unity.VRTemplate;
using UnityEngine;

public class Grill : MonoBehaviour
{
    public bool grillOn = false;

    [SerializeField] private float onThreshold = 0.9f;

    [SerializeField] private GameObject KnobObj;
    private XRKnob knob;


    private void Start()
    {
        knob = KnobObj.GetComponent<XRKnob>();
    }

    private void Update()
    {
        if (knob != null)
        {
            if (knob.value > onThreshold)
            { 
                grillOn = true;
                Debug.Log("GRILL POWER ON");
            }
            else
            {
                grillOn = false;
                Debug.Log("GRILL POWER OFF");
            }
        }
    }



}

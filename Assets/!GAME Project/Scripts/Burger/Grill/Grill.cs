using Unity.VRTemplate;
using UnityEngine;
using UnityEngine.UI;

public class Grill : MonoBehaviour
{
    public bool grillOn = false;
    [SerializeField] private float onThreshold = 0.9f;

    [SerializeField] private Animator GrillIcon;
    [SerializeField] private Image GrillBarImage;

    [SerializeField] private Color GrillHalf_C;
    [SerializeField] private Color Grillfull_C;

    [SerializeField] private GameObject KnobObj;
    private XRKnob knob;


    private void Start()
    {
        knob = KnobObj.GetComponent<XRKnob>();
    }

    private void Update()
    {
        if (GrillIcon != null && GrillBarImage != null)
        {
            GrillIcon.SetFloat("name", knob.value);
            GrillBarImage.fillAmount = knob.value;

            if (knob.value >= 0.9f) 
            {
                GrillBarImage.color = Grillfull_C;
            }
            else if (knob.value > 0.5f && knob.value < 0.9f)
            {
                GrillBarImage.color = GrillHalf_C;
            }
            else
            {
                GrillBarImage.color = Color.white;
            }
        }

        if (knob != null)
        {
            if (knob.value > onThreshold)
            { 
                grillOn = true;
                
            }
            else
            {
                grillOn = false;
            }
        }
    }



}

using Unity.VRTemplate;
using UnityEngine;
using UnityEngine.UI;

public class Grill : MonoBehaviour
{

    [Header("GrillStats")]
    public bool grillOn = false;
    [SerializeField] private float onThreshold = 0.9f;
    [SerializeField] private float grillTurnOffRate;
    [SerializeField] private float turnOffTimerMax;
    private float turnOffTimer; 

    [SerializeField] private Animator GrillIcon;
    [SerializeField] private Image GrillBarImage;

    [SerializeField] private Color GrillHalf_C;
    [SerializeField] private Color Grillfull_C;

    [SerializeField] private GameObject KnobObj;
    private XRKnob knob;


    private void Start()
    {
        knob = KnobObj.GetComponent<XRKnob>();
        turnOffTimer = turnOffTimerMax;
    }

    private void Update()
    {
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

            if (GrillIcon != null && GrillBarImage != null)
            {
                GrillIcon.SetFloat("KnobTurn", knob.value);
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

        }

    }

    private void FixedUpdate()
    {

        if (knob.value > 0.05f)
        {
            if (turnOffTimer > 0)
            {
                turnOffTimer -= Time.fixedDeltaTime;
            }
            else
            {
                turnOffTimer = turnOffTimerMax;
                knob.value -= grillTurnOffRate;

            }
        }
    }


}

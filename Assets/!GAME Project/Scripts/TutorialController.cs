using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System;

public class TutorialController : MonoBehaviour
{
    public static TutorialController Instance;

    [SerializeField] private GameObject foodTubeTutorial, cuttingTutorial, beefCookingTutorial, stoveKnobTutorial,bellTutorial,sodaButtonTutorial,sodaHandleTutorial;

    private Hinge_trigger hingeTrigger;
    private Grill grill;
    private XRBaseInteractable knife;
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
        // set all tutorial objects inactive
        GameObject[] objects = new GameObject[] { foodTubeTutorial, cuttingTutorial, beefCookingTutorial, stoveKnobTutorial, bellTutorial,sodaButtonTutorial,sodaHandleTutorial };
        foreach(GameObject t in objects)
        {
            t.SetActive(false);
        }
        // set up event listeners to trigger tutorials
        hingeTrigger = FindAnyObjectByType<Hinge_trigger>();
        var knifeObj = GameObject.FindGameObjectWithTag("Knife");
        if (knifeObj != null)
        {
            knife = knifeObj.GetComponent<XRBaseInteractable>();
        }
        grill = FindAnyObjectByType<Grill>();
        if (grill!=null)
        {
            grill.OnGrillTUrnedOn.AddListener(HideStoveKnobTutorial);
        }
        if (OrderController.Instance!=null)
        {
            OrderController.Instance.OnNewOrderGenerated.AddListener(ShowFoodTubeTutorial);
        }
        if (knife!=null)
        {
            knife.firstSelectEntered.AddListener(HideCuttingTutorial);
        }
    }

    public void ShowFoodTubeTutorial(Order order)
    {
        OrderController.Instance.OnNewOrderGenerated.RemoveListener(ShowFoodTubeTutorial);
        foodTubeTutorial.SetActive(true);
        if (hingeTrigger!=null)
        {
            hingeTrigger.onPulled.AddListener(HideFoodTubeTutorial);
            hingeTrigger.onPulled.AddListener(ShowCuttingTutorial);
        }
    }
    public void HideFoodTubeTutorial()
    {
        hingeTrigger.onPulled.RemoveListener(HideFoodTubeTutorial);
        foodTubeTutorial.SetActive(false);
    }
    public void ShowCuttingTutorial()
    {
        hingeTrigger.onPulled.RemoveListener(ShowCuttingTutorial);
        cuttingTutorial.SetActive(true);
    }
    public void HideCuttingTutorial(SelectEnterEventArgs arg0)
    {
        knife.firstSelectEntered.RemoveListener(HideCuttingTutorial);
        cuttingTutorial.SetActive(false);
        ShowStoveKnobTutorial();
        ShowBeefCookingTutorial();
    }
    public void ShowStoveKnobTutorial()
    {
        stoveKnobTutorial.SetActive(true);
    }
    public void HideStoveKnobTutorial()
    {
        grill.OnGrillTUrnedOn.RemoveListener(HideStoveKnobTutorial);
        stoveKnobTutorial.SetActive(false);
        HideBeefCookingTutorial();
    }
    public void ShowBeefCookingTutorial()
    {
        beefCookingTutorial.SetActive(true);
    }
    public void HideBeefCookingTutorial()
    {
        beefCookingTutorial.SetActive(false);
        ShowSodaButtonTutorial();
    }
    public void ShowBellTutorial()
    {
        bellTutorial.SetActive(true);
        var bell = FindAnyObjectByType<Bell>();
        if (bell!=null)
        {
            bell.OnBellPressed.AddListener(HideBellTutorial);
        }
    }
    public void HideBellTutorial()
    {
        var bell = FindAnyObjectByType<Bell>();
        bell.OnBellPressed.RemoveListener(HideBellTutorial);
        bellTutorial.SetActive(false);
    }
    public void ShowSodaButtonTutorial()
    {
        sodaButtonTutorial.SetActive(true);
    }
    public void HideSodaButtonTutorial()
    {
        sodaButtonTutorial.SetActive(false);
        ShowSodaHandleTutorial();
    }
    public void ShowSodaHandleTutorial()
    {
        sodaHandleTutorial.SetActive(true);
    }
    public void HideSodaHandleTutorial()
    {
        sodaHandleTutorial.SetActive(false);
        ShowBellTutorial();
    }
}

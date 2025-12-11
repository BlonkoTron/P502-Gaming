using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class CustomerController : MonoBehaviour
{
    static public CustomerController Instance;
    [SerializeField] private GameObject Customer;
    private GameObject currentCustomer;

    private EventInstance CustomerArrive;
    [SerializeField] private EventReference Customersound;

    private EventInstance Customerleave;
    [SerializeField] private EventReference CustomerDone;

    public Transform Soundtrans;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        Audiomanager.instance.UpdateSoundPosition(CustomerArrive, Soundtrans.position);
        Audiomanager.instance.UpdateSoundPosition(Customerleave, Soundtrans.position);
    }

    private void Start()
    {
        OrderController.Instance.OnOrderFullfilled.AddListener(EndOrder);
    }
    private void OnDestroy()
    {
        OrderController.Instance.OnOrderFullfilled.RemoveListener(EndOrder);
    }
    public void NewCustomer()
    {
        if (currentCustomer != null)
        {
            Destroy(currentCustomer);
        }
        CustomerArrive = Audiomanager.instance.PlaySound(Customersound, Soundtrans.position);
        currentCustomer = Instantiate(Customer);
        

    }

    public void EndOrder(bool correctOrder)
    {
        if (currentCustomer!=null)
        {
            currentCustomer.GetComponent<Animator>().SetTrigger("OrderDone");
            Customerleave = Audiomanager.instance.PlaySound(CustomerDone, Soundtrans.position);
        }
    }

}

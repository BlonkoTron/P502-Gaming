using UnityEngine;

public class CustomerController : MonoBehaviour
{
    static public CustomerController Instance;
    [SerializeField] private GameObject Customer;
    private GameObject currentCustomer;

    private void Awake()
    {
        Instance = this;
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

        currentCustomer = Instantiate(Customer);

    }

    public void EndOrder(bool correctOrder)
    {
        currentCustomer.GetComponent<Animator>().SetTrigger("OrderDone"); 
    }

}

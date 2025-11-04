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
        NewCustomer();
    }

    public void NewCustomer()
    {
        if (currentCustomer != null)
        {
            Destroy(currentCustomer);
        }

        currentCustomer = Instantiate(Customer);

    }


}

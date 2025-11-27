using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(OrderController))]
public class OrderControllerVisuals : MonoBehaviour
{
    private OrderController _orderController;

    private EventInstance Orders;
    [SerializeField] private EventReference Ordersound;

    [SerializeField] private GameObject receiptPrefab;

    void Awake()
    {
        _orderController = GetComponent<OrderController>();

        _orderController.OnNewOrderGenerated.AddListener(OnNewOrder);
    }
    private void OnNewOrder(Order order)
    {
        PrintReceipt();
    }
    private void OnDestroy()
    {
        _orderController.OnNewOrderGenerated.RemoveListener(OnNewOrder);

    }
    public void PrintReceipt()
    {
        var receiptObj = Instantiate(receiptPrefab, this.transform);
        var receipt = receiptObj.GetComponent<Receipt>();
        if (receipt != null)
        {
            if (OrderController.Instance.ActiveOrder!=null)
            {
                receipt.UpdateReceiptMaterial(OrderController.Instance.ActiveOrder.receiptMaterial);
            }
            Audiomanager.instance.UpdateSoundPosition(Orders, transform.position);
        }
    }

}

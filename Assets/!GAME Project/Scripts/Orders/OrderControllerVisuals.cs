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
    [SerializeField] private GameObject NewOrderParticle;

    void Awake()
    {
        _orderController = GetComponent<OrderController>();

        _orderController.OnNewOrderGenerated.AddListener(OnNewOrder);
    }
    private void OnNewOrder(Order order)
    {
        var receiptObj = Instantiate(receiptPrefab, this.transform);
         var receipt =receiptObj.GetComponent<Receipt>();
        if (receipt!=null)
        {
            receipt.UpdateReceiptMaterial(order.receiptMaterial);
            Audiomanager.instance.UpdateSoundPosition(Orders, transform.position);
        } 
        if (NewOrderParticle!=null)
        {
            Instantiate(NewOrderParticle, transform.position, Quaternion.identity);
        }
    }
    private void OnDestroy()
    {
        _orderController.OnNewOrderGenerated.RemoveListener(OnNewOrder);

    }

}

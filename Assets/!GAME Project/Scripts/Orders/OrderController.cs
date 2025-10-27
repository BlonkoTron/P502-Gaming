using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;


public class OrderController : MonoBehaviour
{
    public static OrderController Instance;

    public OrderDataCollection orderDataCollection;
    private Order activeOrder;

    public UnityEvent OnNewOrderGenerated;
    public Order ActiveOrder
    {
        get { return activeOrder; }   

    }
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
    public Order GenererateNewOrder()
    {
        // get a random index from the data of premade orders and make that the new order
        var newOrderIndex = Random.Range(0, orderDataCollection.OrderDatas.Count);
        var newBurger = orderDataCollection.OrderDatas[newOrderIndex].burgerIngredients;
        var newDrink = orderDataCollection.OrderDatas[newOrderIndex].drink;
        Order newOrder = new Order(newBurger,newDrink);
        activeOrder = newOrder;
        OnNewOrderGenerated.Invoke();
        return newOrder;
    }
}

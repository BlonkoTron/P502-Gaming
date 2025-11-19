using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;


public class OrderController : MonoBehaviour
{
    public static OrderController Instance;

    public OrderDataCollection orderDataCollection;
    private Order activeOrder;
    private int lastOrderIndex;
    public UnityEvent<Order> OnNewOrderGenerated;
    public UnityEvent<bool> OnOrderFullfilled;
    private Bell bell;
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
    private void Start()
    {
        bell = FindAnyObjectByType<Bell>();
        if (bell!=null)
        {
            bell.OnBellPressed.AddListener(CheckOrder);
        }
    }
    private void OnDestroy()
    {
        bell.OnBellPressed.RemoveListener(CheckOrder);

    }
    public void GenererateNewOrder()
    {
        // get a random index from the data of premade orders and make that the new order
        var newOrderIndex = Random.Range(0, orderDataCollection.OrderDatas.Count);
        // reroll if same as last order
        while(newOrderIndex==lastOrderIndex &&orderDataCollection.OrderDatas.Count>1)
        {
            newOrderIndex = Random.Range(0, orderDataCollection.OrderDatas.Count);
        }
        lastOrderIndex = newOrderIndex;
        var newBurger = orderDataCollection.OrderDatas[newOrderIndex].burgerIngredients;
        var newDrink = orderDataCollection.OrderDatas[newOrderIndex].drink;
        var newMat = orderDataCollection.OrderDatas[newOrderIndex].receiptMaterial;
        Order newOrder = new Order(newBurger, newDrink, newMat);
        activeOrder = newOrder;
        OnNewOrderGenerated.Invoke(newOrder);
    }
    public bool IsOrderFullfilled(List<Order.BurgerIngredient> burger, Order.Drink drink)
    {
        bool burgerCorrect = activeOrder.CheckBurgerMatch(burger);
        bool drinkCorrect = activeOrder.CheckDrinkMatch(drink);
        if (burgerCorrect && drinkCorrect) 
        { 
            return true; 
        } else
        {
            return false;
        }
    }
    private void CheckOrder()
    {
        var trayBurger = TrayManager.Instance.GetBurgerOnTray();
        var traySoda = TrayManager.Instance.GetDrinkOnTray();

        bool OrderCorrect = IsOrderFullfilled(trayBurger,traySoda);
        OnOrderFullfilled.Invoke(OrderCorrect);

        if (OrderCorrect)
        {
            Debug.Log("order was correct");
        }
        else { Debug.Log("Order was wrong"); }
    }
}

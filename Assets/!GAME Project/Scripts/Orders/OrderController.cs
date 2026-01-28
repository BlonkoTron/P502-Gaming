using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;


public class OrderController : MonoBehaviour
{
    public static OrderController Instance;
    // collection of all possible orders. used for getting a random order
    public OrderDataCollection orderDataCollection;
    // set the sequence of orders in the inspector
    public OrderDataCollection orderQueue;

    private Order activeOrder;
    private int lastOrderIndex;
    public UnityEvent<Order> OnNewOrderGenerated;
    public UnityEvent<bool> OnOrderFullfilled;
    private Bell bell;
    [HideInInspector] public int correctOrdersServed = 0;
    public Order ActiveOrder
    {
        get { return activeOrder; }   

    }
    public Queue<Order> premadeOrderQueue = new Queue<Order>();
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
        GenerateOrderQueue();
        // check order when bell is pressed
        bell = FindAnyObjectByType<Bell>();
        if (bell!=null)
        {
            bell.OnBellPressed.AddListener(CheckOrder);
        }
    }
    private void OnDestroy()
    {
        bell = FindAnyObjectByType<Bell>();
        if (bell != null)
        {
            bell.OnBellPressed.AddListener(CheckOrder);
        }
    }
    public void GenererateNewOrder()
    {
        if (premadeOrderQueue.Count>0)
        {
            activeOrder = GetPremadeOrder();
        } else
        {
            activeOrder = GetRandomOrder();
        }
        OnNewOrderGenerated.Invoke(activeOrder);
    }
    private void GenerateOrderQueue()
    {
        for (int i=0;i<orderQueue.OrderDatas.Count; i++)
        {
            var newBurger = orderQueue.OrderDatas[i].burgerIngredients;
            var newDrink = orderQueue.OrderDatas[i].drink;
            var newMat = orderQueue.OrderDatas[i].receiptMaterial;
            Order newOrder = new Order(newBurger, newDrink, newMat);
            premadeOrderQueue.Enqueue(newOrder);
        }
    }
    private Order GetRandomOrder()
    {
        // get a random index from the data of premade orders and make that the new order
        var newOrderIndex = Random.Range(0, orderDataCollection.OrderDatas.Count);
        // reroll if same as last order
        while (newOrderIndex == lastOrderIndex && orderDataCollection.OrderDatas.Count > 1)
        {
            newOrderIndex = Random.Range(0, orderDataCollection.OrderDatas.Count);
        }
        lastOrderIndex = newOrderIndex;
        var newBurger = orderDataCollection.OrderDatas[newOrderIndex].burgerIngredients;
        var newDrink = orderDataCollection.OrderDatas[newOrderIndex].drink;
        var newMat = orderDataCollection.OrderDatas[newOrderIndex].receiptMaterial;
        Order newOrder = new Order(newBurger, newDrink, newMat);
        return newOrder;
    }
    private Order GetPremadeOrder()
    {
        Order newOrder = premadeOrderQueue.Dequeue();
        return newOrder;
    }
    public bool IsOrderFullfilled(List<Order.BurgerIngredient> burger, Order.Drink drink)
    {
        if (activeOrder==null) { Debug.Log("No active order"); return false; }
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
    // starts the checks for if order is fullfilled correctly or not
    private void CheckOrder()
    {
        if (activeOrder==null)
        {
            Debug.Log("No active order");
            return; 
        }
        var trayBurger = TrayManager.Instance.GetBurgerOnTray();
        var traySoda = TrayManager.Instance.GetDrinkOnTray();

        bool OrderCorrect = IsOrderFullfilled(trayBurger,traySoda);

        if (OrderCorrect)
        {
            correctOrdersServed++;
            Debug.Log("order was correct");
        }
        else { Debug.Log("Order was wrong"); }

        OnOrderFullfilled.Invoke(OrderCorrect);
    }
}

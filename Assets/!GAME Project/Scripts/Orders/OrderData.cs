using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "OrderData", menuName = "Scriptable Objects/OrderData")]
public class OrderData : ScriptableObject
{
    public Material receiptMaterial;
    public List<Order.BurgerIngredient> burgerIngredients;
    public Order.Drink drink;
}

using System.Collections.Generic;
using UnityEngine;
public class Order
{

    public Order(List<BurgerIngredient> burger,Drink drink, Material receiptMat)
    {
        Burger = burger;
        SideOrderDrink = drink;
        receiptMaterial = receiptMat;
    }
    public enum BurgerIngredient {
        beef,
        salad,
        tomato,
        ketflup,
        Groole
    }
    public enum Drink {
        none,
        red,
        pink
    }

    public List<BurgerIngredient> Burger;
    public Drink SideOrderDrink;
    public Material receiptMaterial;

}

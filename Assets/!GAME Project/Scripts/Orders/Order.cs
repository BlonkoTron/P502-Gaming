using System.Collections.Generic;
using UnityEngine;
public class Order
{
    public List<BurgerIngredient> Burger;
    public Drink SideOrderDrink;
    public Material receiptMaterial;
    public Order(List<BurgerIngredient> burger,Drink drink, Material receiptMat)
    {
        Burger = burger;
        SideOrderDrink = drink;
        receiptMaterial = receiptMat;
    }
    public enum BurgerIngredient {
        Beef,
        Salad,
        Tomato,
        Ketflup,
        Groole,
        Top_bun,
        Bottom_bun
    }
    public enum Drink {
        none,
        Moon_juice,
        Nebula_blast
    }

    public bool CheckBurgerMatch(List<BurgerIngredient> otherBurger)
    {
        if (otherBurger==null) { return false; }
        int count = 0;
        foreach (BurgerIngredient item in otherBurger)
        {
            if (Burger.Contains(item))
            {
                count++;
            }
        }
        if (count==Burger.Count)
        {
            return true;
        }
        return false;
    }
    public bool CheckDrinkMatch(Drink drink)
    {
        if (drink==SideOrderDrink) { return true; }
        return false;
    }
}

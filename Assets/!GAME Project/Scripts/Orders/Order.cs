using System.Collections.Generic;
public class Order
{

    public Order(List<BurgerIngredient> burger,Drink drink)
    {
        Burger = burger;
        SideOrderDrink = drink;
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

}

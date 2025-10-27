using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "OrderDataCollection", menuName = "Scriptable Objects/OrderDataCollection")]
public class OrderDataCollection : ScriptableObject
{
    public List<OrderData> OrderDatas;
}

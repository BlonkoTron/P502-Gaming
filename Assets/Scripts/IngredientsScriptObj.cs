using UnityEngine;

[CreateAssetMenu(fileName = "IngredientsScriptObj", menuName = "Scriptable Objects/IngredientsScriptObj")]
public class IngredientsScriptObj : ScriptableObject
{
    public string ingredientName;
    public bool isStackable;
    public bool canStackAbove;
    public float heightOffset;
}

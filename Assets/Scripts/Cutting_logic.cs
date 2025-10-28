using System.Collections.Generic;
using UnityEngine;

public class Cutting_logic : MonoBehaviour
{
    [Header("Current ingredient")]
    public GameObject Ingredient;

    [Header("Cut ingredient")]
    public GameObject Cut_Ingredient;

    private Transform Ingredient_position;

    private void Update()
    {
        Ingredient_position = Ingredient.transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Knife"))
        {
            Destroy(Ingredient);
            Instantiate(Cut_Ingredient,Ingredient_position.transform.position, Quaternion.identity);
        }
    }
}

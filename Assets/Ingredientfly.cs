using System.Collections.Generic;
using UnityEngine;

public class IngredientFly : MonoBehaviour
{
    //Set up list
    [Header("Ingredient Prefabs")]
    public List<GameObject> ingredients;

    //Set up spawn settings
    [Header("Spawn Settings")]
    public Transform spawnPos;
    public float shootForce = 5f;

    //Sets the max amount of food
    public int maxFoodLaunches = 3;

    //Counter for maxFoodLaunches + trigger
    private int counter = 0;
    public bool trigger = false;

    void Update()
    {
        if (trigger)
        {
            LaunchRandomIngredient();
        }
    }

    void LaunchRandomIngredient()
    {
        if (ingredients.Count == 0)
        {
            //Creates a 1 time instance
            trigger = false;
            return;
        }

        // Create randomIndex and picks a random ingredient prefab from list
        int randomIndex = Random.Range(0, ingredients.Count);

        //New gameobject which is the chosen ranodm ingidient
        GameObject chosenIngredient = ingredients[randomIndex];

        // Spawn it from "spawnpos gameobject"
        GameObject obj = Instantiate(chosenIngredient, spawnPos.position, spawnPos.rotation);

        // Launch in random direction
        Rigidbody rb = obj.GetComponent<Rigidbody>();

        if (rb != null)
        {
            Vector3 randomDirection = Random.onUnitSphere;
            rb.AddForce(randomDirection * shootForce, ForceMode.Impulse);
        }

        // Counts food
        counter++;

        // Stop after maxFoodLaunches
        if (counter >= maxFoodLaunches)
        {
            counter = 0;
            trigger = false;
        }
    }
}

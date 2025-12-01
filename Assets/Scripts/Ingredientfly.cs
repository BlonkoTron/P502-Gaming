using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class IngredientFly : MonoBehaviour
{

    [SerializeField] GameObject parent;
    


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

    // Random spawn pos
    public float MinY = 0;
    public float MinX = 0;
    public float MinZ = 0;
    public float MaxY = 0;
    public float MaxX = 0;
    public float MaxZ = 0;


    private EventInstance FoodtubeSound;
    [SerializeField] private EventReference FodtubeSFX;
    private void Update()
    {
        Audiomanager.instance.UpdateSoundPosition(FoodtubeSound, transform.position);

        if (trigger == true)
        {
            LaunchRandomIngredient();
        }
    }
    public void LaunchRandomIngredient()
    {
        trigger = false;
        if (ingredients.Count == 0)
        {
            //Creates a 1 time instance
            return;
        }

        float x = Random.Range(MinX, MaxX);
        float y = Random.Range(MinY, MaxY);
        float z = Random.Range(MinZ, MaxZ);

        FoodtubeSound = Audiomanager.instance.PlaySound(FodtubeSFX, transform.position);

        //create a for loop for the amount of foodlaunches needed
        for (var i = 0; i < maxFoodLaunches; i++)
        {
            // Counts food
            counter++;

            // Create randomIndex and picks a random ingredient prefab from list
            int randomIndex = Random.Range(0, ingredients.Count);

            //New gameobject which is the chosen ranodm ingidient
            GameObject chosenIngredient = ingredients[randomIndex];

            // Spawn it from "spawnpos gameobject"
            GameObject obj = Instantiate(chosenIngredient, spawnPos.position, Quaternion.identity, parent:parent.transform);

            // Launch in random direction
            Rigidbody rb = obj.GetComponent<Rigidbody>();

            if (rb != null)
            {
                Vector3 randomDirection = Random.onUnitSphere;
                rb.AddForce(randomDirection * shootForce, ForceMode.Impulse);
            }
        }

        // Stop after maxFoodLaunches
        if (counter >= maxFoodLaunches)
        {
            counter = 0;
        }
    }
}

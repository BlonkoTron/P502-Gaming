using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class Cutting_logic : MonoBehaviour
{
    [Header("Current ingredient")]
    public GameObject Ingredient;

    private EventInstance Cutsound;
    [SerializeField] private EventReference Cutting;

    [Header("Cut ingredient")]
    public GameObject Cut_Ingredient;

    //Get the current position of the ingredient which needs to be cut
    private Transform Ingredient_position;

    private void Update()
    {
        //Always updates the position to the provate transform, so it always spawns on top of the soon to be deleted object
        Ingredient_position = Ingredient.transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        //If the knife is collided with the ingredient, it destroys it for another (which is the cut ingredient)
        if (other.gameObject.CompareTag("Knife"))
        {
            Destroy(Ingredient);
            Cutsound = Audiomanager.instance.PlaySound(Cutting, transform.position);
            Instantiate(Cut_Ingredient,Ingredient_position.transform.position, Quaternion.identity);
        }
    }
}

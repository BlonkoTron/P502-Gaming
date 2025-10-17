using UnityEngine;

public class Ingredientfly : MonoBehaviour
{
    public GameObject ingredient;
    public GameObject spawnpos;

    public bool Trigger = false;

    public float shootForce = 5;

    // Update is called once per frame
    void Update()
    {
        if (Trigger == true)
        {
            Cubelaunch();
        }
    }

    public void Cubelaunch()
    {
        //Declares the spawn position
        GameObject obj = Instantiate(ingredient, transform.position = spawnpos.transform.position, transform.rotation = spawnpos.transform.rotation);

        //Get rigidbody
        Rigidbody rb = obj.GetComponent<Rigidbody>();

        //Assign random direction to prefab/ingredient
        Vector3 randomDirection = Random.onUnitSphere;
        rb.AddForce(randomDirection * shootForce, ForceMode.Impulse);

        //resets the trigger
        Trigger = false;
        
    }
}

using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Simulation;

public class FlyToShoot : MonoBehaviour
{
    //door setup
    public Transform Outposition;

    public Rigidbody Rigidbody;

    public float forceStrength = 10f;

    public SpaceDoor Spacedoorscript;


    private void Start()
    {
        // get components needed
        Spacedoorscript = GameObject.Find("Roof").GetComponent<SpaceDoor>();
        Outposition = GameObject.Find("Ingredient-kill-collider").transform;
        Rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Spacedoorscript.cooldown == true)
        {
            flyouts();
        }
    }

    public void flyouts()
    {
        //give each ingredient in the world a force, so they fly towards the roof
        Vector3 direction = (Outposition.position - transform.position).normalized;
        Debug.Log("forceadd");
        // Apply force toward the target
        Rigidbody.AddForce(direction * forceStrength);
    }
}

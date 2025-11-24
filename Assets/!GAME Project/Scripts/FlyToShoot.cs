using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Simulation;

public class FlyToShoot : MonoBehaviour
{

    public Transform Outposition;

    public Rigidbody Rigidbody;

    public float forceStrength = 10f;

    public SpaceDoor Spacedoorscript;


    private void Start()
    {
        Spacedoorscript = GameObject.Find("Roof").GetComponent<SpaceDoor>();
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
        Vector3 direction = (Outposition.position - transform.position).normalized;
        Debug.Log("forceadd");
        // Apply force toward the target
        Rigidbody.AddForce(direction * forceStrength);
    }
}

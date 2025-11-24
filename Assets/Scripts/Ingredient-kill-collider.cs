using UnityEngine;

public class Ingredient_kill_collider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
       
            Destroy(other.gameObject);
    }
}

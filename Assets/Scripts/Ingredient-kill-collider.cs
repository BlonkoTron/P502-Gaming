using UnityEngine;

public class Ingredient_kill_collider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ingredient"))
        {
            Destroy(other.gameObject);
        }
    }
}

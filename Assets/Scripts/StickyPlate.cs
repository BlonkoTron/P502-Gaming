using UnityEngine;

public class StickyPlater : MonoBehaviour
{
    private Rigidbody ingredientrigid;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ingredient"))
        {
            ingredientrigid = other.gameObject.GetComponent<Rigidbody>();
            ingredientrigid.isKinematic = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        ingredientrigid = other.gameObject.GetComponent<Rigidbody>();
        ingredientrigid.isKinematic = false;
    }
}

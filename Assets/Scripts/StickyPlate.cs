using UnityEngine;

public class StickyPlater : MonoBehaviour
{
    private Rigidbody ingredientrigid;
    private void OnTriggerEnter(Collider other)
    {
        ingredientrigid = other.gameObject.GetComponent<Rigidbody>();
        ingredientrigid.isKinematic = true;
        ingredientrigid.transform.rotation = Quaternion.identity;
    }

    private void OnTriggerExit(Collider other)
    {
        ingredientrigid = other.gameObject.GetComponent<Rigidbody>();
        ingredientrigid.isKinematic = false;
    }
}

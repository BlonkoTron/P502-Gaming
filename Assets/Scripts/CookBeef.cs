using System.Collections;
using UnityEngine;

public class CookBeef : MonoBehaviour
{
    // Materials for the different stages of cooking
    public Material Raw;
    public Material Halfcooked;
    public Material Cooked;
    public Material Burnt;

    // Reference to beef object to change material
    public GameObject beef;

    // Time (in seconds) the beef needs to fully cook
    [SerializeField] private float DesiredCooktime = 1000f;

    // Runtime values (not saved in inspector)
    [SerializeField] private float Cooktime;
    private float HalfwayPoint;
    private float OvercookTimer = 10f;

    // How much time the player has to flip
    public float fliptimethreshold = 5f;

    // State flags
    [SerializeField] private bool isflipped = false;
    [SerializeField] private bool halfCookedReached = false;

    void Start()
    {
        // Initialize runtime values
        Cooktime = DesiredCooktime;
        HalfwayPoint = DesiredCooktime / 2f;

        // Default material
        beef.GetComponent<MeshRenderer>().material = Raw;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Grill"))
            return;

        // Reduce cooking time
        Cooktime -= Time.deltaTime;

        // ---- HALF COOKED SIDE ----
        if (Cooktime <= HalfwayPoint && !isflipped)
        {
            beef.GetComponent<MeshRenderer>().material = Halfcooked;
            halfCookedReached = true;

            // If player never flips → burn
            if (Cooktime <= 0f)
            {
                beef.GetComponent<MeshRenderer>().material = Burnt;
            }
        }

        // ---- SECOND SIDE ----
        if (halfCookedReached && isflipped)
        {
            if (Cooktime <= 0f)
            {
                // Cooked first
                beef.GetComponent<MeshRenderer>().material = Cooked;

                // Start burning countdown
                OvercookTimer -= Time.deltaTime;

                if (OvercookTimer <= 0f)
                {
                    beef.GetComponent<MeshRenderer>().material = Burnt;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Grill"))
        {
            // Start checking rotation after leaving the grill
            StartCoroutine(CheckFlip());
        }
    }

    IEnumerator CheckFlip()
    {
        // Give player some time to flip
        yield return new WaitForSeconds(fliptimethreshold);

        float yRot = transform.eulerAngles.y;

        // If burger was turned around
        if (Mathf.Abs(yRot - 180f) < 20f && halfCookedReached)
        {
            isflipped = true;
        }
    }
}

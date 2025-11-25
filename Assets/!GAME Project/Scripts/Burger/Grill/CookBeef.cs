using System.Collections;
using UnityEngine;

[RequireComponent(typeof(IngredientStackable))]
public class CookBeef : MonoBehaviour
{
    // Materials for the different stages of cooking
    public Material Raw;
    public Material HalfCooked;
    public Material Cooked;
    public Material Burnt;

    // Reference to beef object to change material
    public GameObject beef;

    // Time (in seconds) the beef needs to fully cook
    [SerializeField] private float DesiredCooktime = 1000f;

    // Runtime values (not saved in inspector)
    private float Cooktime;
    private float HalfwayPoint;
    private float OvercookTimer = 10f;

    // How much time the player has to flip
    public float flipTimeThreshold = 5f;

    // State flags
    private bool isflipped = false;
    private bool halfCookedReached = false;
    private bool isCooked = false;
    private bool isBurnt = false;

    [SerializeField] private GameObject cookedParticle, burntParticle;


    private IngredientStackable ingredientStackable;
    private Grill grill;

    void Start()
    {
        // Initialize runtime values
        Cooktime = DesiredCooktime;
        HalfwayPoint = DesiredCooktime / 2f;
        ingredientStackable = GetComponent<IngredientStackable>();
        ingredientStackable.enabled = false;
        grill = null;

        // Default material
        beef.GetComponent<MeshRenderer>().material = Raw;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Grill"))
            return;

        if (grill == null)
        {
            grill = other.GetComponent<Grill>();
        }
        else if (grill.grillOn)
        {
            // Reduce cooking time
            Cooktime -= Time.deltaTime;

            // ---- HALF COOKED SIDE ----
            if (Cooktime <= HalfwayPoint && !isflipped)
            {
                if (!halfCookedReached)
                {
                    BeefHalfCooked();
                }

                // If player never flips → burn
                if (Cooktime <= 0f && !isBurnt)
                {
                    BeefBurnt();
                }
            }
            // ---- SECOND SIDE ----
            if (halfCookedReached && isflipped)
            {
                if (Cooktime <= 0f && !isBurnt)
                {
                    // Cooked first
                    if (!isCooked)
                    {
                        BeefCooked();
                    }
                    // Start burning countdown
                    OvercookTimer -= Time.deltaTime;

                    if (OvercookTimer <= 0f && !isBurnt)
                    {
                        BeefBurnt();
                    }
                }
            }
        }
    }
    private void BeefHalfCooked()
    {
        beef.GetComponent<MeshRenderer>().material = HalfCooked;
        halfCookedReached = true;
        Instantiate(cookedParticle, transform.position, Quaternion.identity);
    }
    private void BeefCooked()
    {
        beef.GetComponent<MeshRenderer>().material = Cooked;
        Instantiate(cookedParticle, transform.position, Quaternion.identity);
        ingredientStackable.enabled = true;
        isCooked = true;
    }
    private void BeefBurnt()
    {
        beef.GetComponent<MeshRenderer>().material = Burnt;
        Instantiate(burntParticle, transform.position, Quaternion.identity);
        ingredientStackable.enabled = false;
        isBurnt = true;
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
        // Give player time to flip
        yield return new WaitForSeconds(flipTimeThreshold);

        float yRot = transform.eulerAngles.y;

        // If burger is turned around
        if (Mathf.Abs(yRot - 180f) < 20f && halfCookedReached)
        {
            isflipped = true;
        }
    }
}

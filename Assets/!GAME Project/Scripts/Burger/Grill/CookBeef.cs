using System.Collections;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class CookBeef : MonoBehaviour
{
    // Materials for the different stages of cooking
    public Material Raw;
    public Material Halfcooked;
    public Material Cooked;
    public Material Burnt;

    private EventInstance beefcook;
    [SerializeField] private EventReference beefed;

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

    [SerializeField] private GameObject cookedParticle, burntParticle;


    private Grill grill;

    void Start()
    {
        // Initialize runtime values
        Cooktime = DesiredCooktime;
        HalfwayPoint = DesiredCooktime / 2f;

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
        else
        {
            if (grill.grillOn)
            {
                // Reduce cooking time
                Cooktime -= Time.deltaTime;
                Audiomanager.instance.UpdateSoundPosition(beefcook, transform.position);


                // ---- HALF COOKED SIDE ----
                if (Cooktime <= HalfwayPoint && !isflipped)
                {
                    beef.GetComponent<MeshRenderer>().material = Halfcooked;
                    halfCookedReached = true;
                    Instantiate(cookedParticle, transform.position,Quaternion.identity);

                    // If player never flips → burn
                    if (Cooktime <= 0f)
                    {
                        beef.GetComponent<MeshRenderer>().material = Burnt;
                        Instantiate(burntParticle, transform.position, Quaternion.identity);

                    }
                }

                // ---- SECOND SIDE ----
                if (halfCookedReached && isflipped)
                {
                    if (Cooktime <= 0f)
                    {
                        // Cooked first
                        beef.GetComponent<MeshRenderer>().material = Cooked;
                        Instantiate(cookedParticle, transform.position, Quaternion.identity);


                        // Start burning countdown
                        OvercookTimer -= Time.deltaTime;

                        if (OvercookTimer <= 0f)
                        {
                            beef.GetComponent<MeshRenderer>().material = Burnt;
                            Instantiate(burntParticle, transform.position, Quaternion.identity);
                        }
                    }
                }
            }
            
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Grill"))
        {
            Audiomanager.instance.StopSound(beefcook);
            // Start checking rotation after leaving the grill
            StartCoroutine(CheckFlip());
        }
    }

    IEnumerator CheckFlip()
    {
        // Give player time to flip
        yield return new WaitForSeconds(fliptimethreshold);

        float yRot = transform.eulerAngles.y;

        // If burger is turned around
        if (Mathf.Abs(yRot - 180f) < 20f && halfCookedReached)
        {
            isflipped = true;
        }
    }
}

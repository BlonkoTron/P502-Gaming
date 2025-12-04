using System.Collections;
using FMOD.Studio;
using FMODUnity;
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
    public bool isflipped = false;
    private bool halfCookedReached = false;
    private bool isCooked = false;
    private bool isBurnt = false;

    private bool Soundblock = false;

    private EventInstance CookBeefSound;
    [SerializeField] private EventReference Cookbeefsfx;

    private EventInstance HalfCookedsound;
    [SerializeField] private EventReference HalfcookedSFX;

    private EventInstance CookedDoneSound;
    [SerializeField] private EventReference CookeddoneSFX;

    private EventInstance BurntSound;
    [SerializeField] private EventReference BurntSFX;

    [SerializeField] private GameObject cookedParticle, burntParticle, cookingSteam;


    private IngredientStackable ingredientStackable;
    private Grill grill;

    private void Update()
    {
        Audiomanager.instance.UpdateSoundPosition(CookBeefSound, transform.position);
        Audiomanager.instance.UpdateSoundPosition(HalfCookedsound, transform.position);
        Audiomanager.instance.UpdateSoundPosition(CookedDoneSound, transform.position);
        Audiomanager.instance.UpdateSoundPosition(BurntSound, transform.position);
    }

    void Start()
    {
        // Initialize runtime values
        Cooktime = DesiredCooktime;
        HalfwayPoint = DesiredCooktime / 2f;
        ingredientStackable = GetComponent<IngredientStackable>();
        ingredientStackable.enabled = false;
        grill = null;

        if (cookingSteam != null) 
        {
            cookingSteam.gameObject.SetActive(false);
        }
        

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
            if (Soundblock == false)
            {
                CookBeefSound = Audiomanager.instance.PlaySound(Cookbeefsfx, transform.position);
                Soundblock = true;
            }

            // Reduce cooking time
            Cooktime -= Time.deltaTime;

            if (!cookingSteam.gameObject.activeSelf)
            {
                cookingSteam.gameObject.SetActive(true);
            }

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
                //CookBeefSound = Audiomanager.instance.PlaySound(Cookbeefsfx, transform.position);
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
        HalfCookedsound = Audiomanager.instance.PlaySound(HalfcookedSFX, transform.position);
        beef.GetComponent<MeshRenderer>().material = HalfCooked;
        halfCookedReached = true;
        Instantiate(cookedParticle, transform.position, Quaternion.identity);
    }
    private void BeefCooked()
    {
        CookedDoneSound = Audiomanager.instance.PlaySound(CookeddoneSFX, transform.position);
        beef.GetComponent<MeshRenderer>().material = Cooked;
        Instantiate(cookedParticle, transform.position, Quaternion.identity);
        ingredientStackable.enabled = true;
        isCooked = true;
    }
    private void BeefBurnt()
    {
        BurntSound = Audiomanager.instance.PlaySound(BurntSFX, transform.position);
        beef.GetComponent<MeshRenderer>().material = Burnt;
        Instantiate(burntParticle, transform.position, Quaternion.identity);
        ingredientStackable.enabled = false;
        isBurnt = true;
    }

    private void OnTriggerExit(Collider other)
    {
        Soundblock = false;
        Audiomanager.instance.StopSound(CookBeefSound);

        if (other.CompareTag("Grill"))
        {
            // Start checking rotation after leaving the grill
            if (cookingSteam.gameObject.activeSelf)
            {
                cookingSteam.gameObject.SetActive(false);
            }

            StartCoroutine(CheckFlip());
        }
    }

    IEnumerator CheckFlip()
    {
        // Give player time to flip
        yield return new WaitForSeconds(flipTimeThreshold);
        isflipped = true;
        //float zRot = transform.eulerAngles.z;
        //float xRot = transform.eulerAngles.x;

        // If burger is turned around
        //if (Mathf.Abs(zRot - 180f) < 20f || Mathf.Abs(xRot - 180f) < 20f && halfCookedReached)
        //{
            
        //}
    }

    }

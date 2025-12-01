using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class KnifeSpring : MonoBehaviour
{
    public LineRenderer line;
    [SerializeField] Transform pos1;
    [SerializeField] Transform pos2;

    private EventInstance KnifeSound;
    [SerializeField] private EventReference Knifesfx;

    void Start()
    {
        line.positionCount = 2;
    }

    void Update()
    {
        line.SetPosition(0, pos1.position);
        line.SetPosition(1, pos2.position);
        Audiomanager.instance.UpdateSoundPosition(KnifeSound, transform.position);
    }

    public void KnifePickup()
    {
        KnifeSound = Audiomanager.instance.PlaySound(Knifesfx, transform.position);
    }
}
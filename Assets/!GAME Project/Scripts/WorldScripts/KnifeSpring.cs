using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeSpring : MonoBehaviour
{
    public LineRenderer line;
    [SerializeField] Transform pos1;
    [SerializeField] Transform pos2;
    void Start()
    {
        line.positionCount = 2;
    }

    void Update()
    {
        line.SetPosition(0, pos1.position);
        line.SetPosition(1, pos2.position);
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlwaysPlayParticle : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ParticleSystemRenderer renderer = GetComponent<ParticleSystemRenderer>();
        renderer.bounds = new Bounds(new Vector3(0, 0, 0), new Vector3(Mathf.Infinity, Mathf.Infinity, Mathf.Infinity));
    }
}

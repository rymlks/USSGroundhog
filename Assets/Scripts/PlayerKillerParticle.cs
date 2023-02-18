using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKillerParticle : MonoBehaviour
{
    public ParticleSystem particleSystem;
    public string deathReason;

    void Start()
    {
        particleSystem = GetComponentInParent<ParticleSystem>();
    }

    void OnParticleCollision(GameObject other)
    {
        if(other.tag == "Player")
            GameManager.instance.CommitDie(deathReason);
    }
}

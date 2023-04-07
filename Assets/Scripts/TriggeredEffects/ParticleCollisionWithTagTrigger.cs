using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCollisionWithTagTrigger : AbstractTrigger
{
    public ParticleSystem particleSystem;
    public string tagToTriggerOn = "Player";

    protected override void Start()
    {
        base.Start();
        if (particleSystem == null)
        {
            particleSystem = GetComponentInParent<ParticleSystem>();
        }
    }

    void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag(tagToTriggerOn))
        {
            this.Engage();
        }
    }
}
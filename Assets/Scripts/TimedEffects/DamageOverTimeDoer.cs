using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOverTimeDoer : PlayerKiller
{
    public float multiplier = 1f;
    private PlayerHealthState playerHealthStatus;

    void Start()
    {
        this.playerHealthStatus = FindObjectOfType<PlayerHealthState>();
    }

    protected override void OnTriggerEnter(Collider triggeredCollider)
    {
    }
    private void OnTriggerStay(Collider triggeredCollider)
    {
        if (isCollisionWithPlayer(triggeredCollider))
        {
            if (!immunityIsGranted())
            {
                DoConsequences();
            }
        }
    }
    
    protected override void DoConsequences()
    {
        this.playerHealthStatus.Hurt(deathReason, Time.deltaTime);
    }
}

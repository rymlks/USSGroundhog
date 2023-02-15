using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSuffocator : PlayerKiller
{
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
        FindObjectOfType<PlayerHealthState>().Suffocate(Time.deltaTime);
    }
}

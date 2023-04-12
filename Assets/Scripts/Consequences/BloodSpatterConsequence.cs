#nullable enable
using System.Collections;
using System.Collections.Generic;
using Consequences;
using Triggers;
using UnityEngine;

public class BloodSpatterConsequence : MonoBehaviour, IConsequence
{
    public ParticleSystem spatterParticleSystem;

    void Start()
    {
        if (spatterParticleSystem == null)
        {
            Debug.Log("Particle system consequence failed, associated inspector value unset.");
            Destroy(this);
        }
    }

    public void execute(TriggerData? data)
    {

        Vector3 positionToSplatAt = this.gameObject.transform.position; 
        if (data?.triggerLocation != null)
        {
            positionToSplatAt = data.triggerLocation.Value;
        }

        spatterParticleSystem.transform.position = positionToSplatAt;

        spatterParticleSystem.Play();

    }
}

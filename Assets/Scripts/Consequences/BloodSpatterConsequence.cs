#nullable enable
using System.Collections;
using System.Collections.Generic;
using Triggers;
using UnityEngine;

public class BloodSpatterConsequence : MonoBehaviour, IConsequence
{
    public ParticleSystem spatterParticleSystem;

    public void execute(TriggerData? data)
    {

        var emitParams = new ParticleSystem.EmitParams();
        Vector3 positionToSplatAt = this.gameObject.transform.position; 
        if (data?.triggerLocation != null)
        {
            positionToSplatAt = data.triggerLocation.Value;
        }

        emitParams.position = positionToSplatAt;


        spatterParticleSystem.transform.position = positionToSplatAt;

        Debug.Log("bloodSpatter.transform.position is equal to " + positionToSplatAt);

        spatterParticleSystem.Play();

    }
}

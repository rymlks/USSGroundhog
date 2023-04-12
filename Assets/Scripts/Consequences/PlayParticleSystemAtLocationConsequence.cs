#nullable enable
using System.Collections;
using System.Collections.Generic;
using Consequences;
using Triggers;
using UnityEngine;

public class PlayParticleSystemAtLocationConsequence : MonoBehaviour, IConsequence
{
    public ParticleSystem toPlay;

    void Start()
    {
        if (toPlay == null)
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

        toPlay.transform.position = positionToSplatAt;

        toPlay.Play();

    }
}

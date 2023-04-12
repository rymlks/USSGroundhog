using O3DWB;
using System.Collections;
using System.Collections.Generic;
using Triggers;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class ParticleCollisionWithTagTrigger : AbstractTrigger
{
    public ParticleSystem particleSystem;
    public string tagToTriggerOn = "Player";
    protected List<ParticleCollisionEvent> collisionEvents;

    protected override void Start()
    {
        base.Start();
        collisionEvents = new List<ParticleCollisionEvent>();

        if (particleSystem == null)
        {
            particleSystem = GetComponentInParent<ParticleSystem>();
        }

    }

    void OnParticleCollision(GameObject other)
    {
        particleSystem.GetCollisionEvents(other, collisionEvents);

        // Rigidbody rb = other.GetComponent<Rigidbody>();
        // int i = 0;

        if (other.CompareTag(tagToTriggerOn))
        {

            // while (i < numCollisionEvents)
            // {
            //     if (rb)
            //     {
            //
            //         Vector3 position = collisionEvents[i].intersection;
            //         Debug.Log(collisionEvents[i].intersection);
            //
            //         Spatter(position);
            //
            //     }
            //     i++;
            // }

            this.Engage(new TriggerData(this.tagToTriggerOn+ " object collided with " + this.particleSystem.name + " particle", collisionEvents[^1].intersection));
        }

    }

}
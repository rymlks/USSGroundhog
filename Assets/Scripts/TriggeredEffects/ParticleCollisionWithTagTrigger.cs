using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class ParticleCollisionWithTagTrigger : AbstractTrigger
{
    public ParticleSystem part;
    public string tagToTriggerOn = "Player";
    public List<ParticleCollisionEvent> collisionEvents;

    protected override void Start()
    {
        base.Start();
        collisionEvents = new List<ParticleCollisionEvent>();

        if (part == null)
        {
            part = GetComponentInParent<ParticleSystem>();
        }

    }

    void OnParticleCollision(GameObject other)
    {
        int numCollisionEvents = part.GetCollisionEvents(other, collisionEvents);

        Rigidbody rb = other.GetComponent<Rigidbody>();
        int i = 0;

        if (other.CompareTag(tagToTriggerOn))
        {

            while (i < numCollisionEvents)
            {
                if (rb)
                {
                    Debug.Log(collisionEvents[i].intersection);
                    //Debug.Log(collisionEvents[i].velocity);

                }
                i++;
            }

            this.Engage();
        }

    }

}
using O3DWB;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class ParticleCollisionWithTagTrigger : AbstractTrigger
{
    public ParticleSystem part;
    public string tagToTriggerOn = "Player";
    public ParticleSystem bloodSpatter;
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

    void Spatter(Vector3 position)
    {

        var emitParams = new ParticleSystem.EmitParams();
        emitParams.position = position;
        

        bloodSpatter.transform.position = position;

        Debug.Log("bloodSpatter.transform.position is equal to " + position);
        
        bloodSpatter.Play();

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

                    Vector3 position = collisionEvents[i].intersection;

                    Spatter(position);

                }
                i++;
            }

            this.Engage();
        }

    }

}
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

    void Spatter(Vector3 position, Vector3 velocity)
    {
        // ParticleSystem.Particle.position

        var emitParams = new ParticleSystem.EmitParams();
        emitParams.position = position;
        //emitParams.velocity = velocity;

        bloodSpatter.transform.position = position;

        Debug.Log("emitParams.position and velocity valures are equal to " + position + " and " + velocity);
        //bloodSpatter.Emit(emitParams, 1);
        bloodSpatter.Play();

        //bloodSpatter.Emit(emitParams,1);

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
                    Debug.Log(collisionEvents[i].intersection);
                    //Debug.Log(collisionEvents[i].velocity);

                    Vector3 position = collisionEvents[i].intersection;
                    Vector3 velocity = collisionEvents[i].velocity;

                    // make Blood spatter happen at collisionEvents[i].intersection

                    Spatter(position, velocity);

                }
                i++;
            }

            this.Engage();
        }

    }

}
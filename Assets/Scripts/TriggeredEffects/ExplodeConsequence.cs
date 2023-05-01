using System.Collections;
using System.Collections.Generic;
using Consequences;
using KinematicCharacterController.Examples;
using Triggers;
using UnityEngine;

public class ExplodeConsequence : AbstractConsequence
{
    public float explosionStrength = 10;
    public bool persist = true;
    public GameObject ExplosionPrefab;
    public GameObject ExampleCamera;

    public bool dontRespawn = false;
    public void Start()
    {
        this.GetComponent<Collider>().isTrigger = true;

    }

    private void createDefaultExplosion()
    {
        gameObject.GetComponent<SoundFXManager>().Play_Small_explosion();
        var explode = Instantiate(ExplosionPrefab);
        explode.transform.position = transform.position;
    }

    public override void execute(TriggerData? data)
    {
        foreach(var part in GetComponentsInChildren<ParticleSystem>())
        {

            part.Play();

        }

        if (gameObject.CompareTag("exploding_canister"))
        {
            createDefaultExplosion();

        } else if (gameObject.CompareTag("exploding_tank"))
        {
            gameObject.GetComponent<SoundFXManager>().Play_Tank_explosion();

            var explode = Instantiate(ExplosionPrefab);
            explode.transform.position = transform.position;

        }  else if (gameObject.CompareTag("exploding_crate"))
        {

            gameObject.GetComponent<SoundFXManager>().Play_Crate_explosion();

            var explode = Instantiate(ExplosionPrefab);
            explode.transform.position = transform.position;

        } else
        {
            createDefaultExplosion();
        }

        GameManager.instance.CommitDie(new Dictionary<string, object>() {
            {"explosion", explosionVector3(data).normalized * explosionStrength},
            {"ragdoll", true},
            {"dontRespawn", dontRespawn}
        });
        if (!persist)
        {
            Destroy(gameObject);
        }
    }

    private Vector3 explosionVector3(TriggerData data)
    {
        if (data != null)
        {
            return (Vector3) (data.triggerLocation - transform.position);
        }
        else return transform.position;
    }
}

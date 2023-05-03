#nullable enable
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
    protected SoundEffectPlayer soundEffectPlayer;

    public bool dontRespawn = false;

    public void Start()
    {
        this.GetComponent<Collider>().isTrigger = true;
        this.soundEffectPlayer = FindObjectOfType<SoundEffectPlayer>();
    }

    private void createDefaultExplosion()
    {
        var explode = Instantiate(ExplosionPrefab);
        explode.transform.position = transform.position;
    }

    public override void execute(TriggerData? data)
    {
        foreach (var part in GetComponentsInChildren<ParticleSystem>())
        {
            part.Play();
        }

        createDefaultExplosion();
        
        GameManager.instance.CommitDie(new Dictionary<string, object>()
        {
            {"explosion", explosionVector3(data).normalized * explosionStrength},
            {"ragdoll", true},
            {"dontRespawn", dontRespawn}
        });
        if (!persist)
        {
            Destroy(gameObject);
        }
    }

    private Vector3 explosionVector3(TriggerData? data)
    {
        if (data != null)
        {
            return (Vector3) (data.triggerLocation - transform.position);
        }
        else
        {
            return transform.position;
        }
    }
}
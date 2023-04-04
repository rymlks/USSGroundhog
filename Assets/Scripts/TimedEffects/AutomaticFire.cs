using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticFire : MonoBehaviour, IConsequence
{
    public float secondsToFire = 1f;
    
    public ParticleSystem casings;
    public ParticleSystem bullets;

    protected float _lastExecutedTime = float.MinValue;
    private bool _firedLastFrame;

    void Start()
    {

        if (casings == null)
        {

            casings = this.transform.Find("CartridgeCasingParticleSystem").GetComponent<ParticleSystem>();

        }

        if (bullets == null)
        {

            bullets = this.transform.Find("BulletParticleSystem").GetComponent<ParticleSystem>();

        }

    }

    void Update()
    {

        if (_firedLastFrame && !ShouldFireThisFrame())
        {
            //stop sound
            this.GetComponent<SoundFXManager>().Stop_Turret_Firing();
            StopAllParticleSystems();
        }
        else if (!_firedLastFrame && ShouldFireThisFrame())
        {
            //start sound
            this.GetComponent<SoundFXManager>().Play_Turret_Firing();
            StartAllParticleSystems();
        }
        _firedLastFrame = ShouldFireThisFrame();

    }


    // void FixedUpdate()
    // {
    //     if (_firedLastFrame && !ShouldFireThisFrame())
    //     {
    //         StopAllParticleSystems();
    //     }
    //     else if (!_firedLastFrame && ShouldFireThisFrame())
    //     {
    //         StartAllParticleSystems();
    //     }
    // }

    private void StartAllParticleSystems()
    {
        Debug.Log("starting");
        bullets.Play();
        casings.Play();

    }

    private void StopAllParticleSystems()
    {

        Debug.Log("stopping");
        bullets.Stop();
        casings.Stop();

    }

    private bool ShouldFireThisFrame()
    {
        return this._lastExecutedTime + this.secondsToFire > Time.time;
    }

    public void execute()
    {
        Debug.Log("firing at time " + Time.time);
        this._lastExecutedTime = Time.time;
    }
}

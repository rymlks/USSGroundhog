using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticFire : MonoBehaviour, IConsequence
{
    public float secondsToFire = 1f;
    
    public ParticleSystem casings;
    public ParticleSystem bullets;

    protected float _lastExecutedTime;
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
        }
        else if (!_firedLastFrame && ShouldFireThisFrame())
        {
            //start sound
        }

        _firedLastFrame = ShouldFireThisFrame();
    }


    void FixedUpdate()
    {
        if(ShouldFireThisFrame())
        {
            
        }
        else{
            
        }
    }

    private bool ShouldFireThisFrame()
    {
        return this._lastExecutedTime + this.secondsToFire > Time.time;
    }

    public void execute()
    {
        this._lastExecutedTime = Time.time;
    }
}

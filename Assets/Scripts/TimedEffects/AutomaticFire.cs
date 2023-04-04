using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticFire : MonoBehaviour, IConsequence
{
    public float secondsToFire = 1f;
    
    public ParticleSystem casings;
    public ParticleSystem bullets;

    protected float _lastExecutedTime;
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

    void FixedUpdate()
    {
        if(ShouldFireThisFrame()){
            
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

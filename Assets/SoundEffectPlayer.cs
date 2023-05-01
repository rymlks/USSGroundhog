using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectPlayer : MonoBehaviour
{
    public AudioClip Small_explosion;
    public AudioClip Tank_explosion;
    public AudioClip Crate_explosion;
    public AudioClip Door_Slide;
    public AudioClip PA_Jingle;
    public AudioClip Ship_Alarm;
    public AudioClip Turret_Firing;
    public AudioClip Turret_Firing_Tail;

    protected AudioSource source;

    void Start()
    {
        if (this.source == null)
        {
            this.source = GetComponentInChildren<AudioSource>();
        }
    }

    public void PlayExplosionSound(string explosionDescription)
    {
        if (explosionDescription.Equals("exploding_tank"))
        {
            source.clip = this.Tank_explosion;
            source.Play();
        }  else if (explosionDescription.Equals("exploding_crate"))
        {
            source.clip = this.Crate_explosion;
            source.Play();
        }
        else
        {
            source.clip = Small_explosion;
            source.Play();
        }
    }
}

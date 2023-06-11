using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;

public class SoundEffectPlayer : MonoBehaviour
{
    public AudioClip Small_explosion;
    public AudioClip Tank_explosion;
    public AudioClip Crate_explosion;
    public AudioClip Door_Slide;
    public AudioClip PA_Jingle;
    public AudioClip Turret_Firing;
    public AudioClip Turret_Firing_Tail;

    protected AudioSource source;

    void Start()
    {
        if (this.source == null)
        {
            this.source = GetComponentInChildren<AudioSource>();
        }
        this.SetSFXVolume();
    }

    public void PlaySound(AudioClip toPlay)
    {
        source.clip = toPlay;
        source.Play();
    }

    public void PlayTurretStart()
    {
        source.clip = Turret_Firing;
        source.Play();
    }

    public void PlayTurretStop()
    {
        source.clip = Turret_Firing_Tail;
        source.Play();
    }

    public void SetSFXVolume()
    {
        this.source.volume = GameSettings.instance.SFXVolumePercentage / 100f;
    }
}

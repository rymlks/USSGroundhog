using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFXManager : MonoBehaviour
{

    public AudioSource Small_explosion;
    public AudioSource Tank_explosion;
    public AudioSource Crate_explosion;
    public AudioSource Door_Slide;
    public AudioSource PA_Jingle;
    public AudioSource Ship_Alarm;
    public AudioSource Turret_Firing;
    public AudioSource Turret_Firing_Tail;

    private static void PlayMeMaybe(AudioSource source)
    {
        if (source != null)
        {
            source.Play();
        }
    }
    
    private static void StopMeMaybe(AudioSource source)
    {
        if (source != null)
        {
            source.Stop();
        }
    }



    public void Play_Small_explosion()
    {

        PlayMeMaybe(Small_explosion);

    }
    public void Play_Tank_explosion()
    {

        PlayMeMaybe(Tank_explosion);

    }
    public void Play_Crate_explosion()
    {

        PlayMeMaybe(Crate_explosion);

    }

    public void Play_Door_Slide()
    {
        PlayMeMaybe(Door_Slide);

    }

    public void Play_PA_Jingle()
    {
        PlayMeMaybe(PA_Jingle);

    }

    public void Play_Ship_Alarm()
    {
        PlayMeMaybe(Ship_Alarm);

    }

    public void Play_Turret_Firing()
    {
        PlayMeMaybe(Turret_Firing);
    }

    public void Stop_Turret_Firing()
    {
        StopMeMaybe(Turret_Firing);
        PlayMeMaybe(Turret_Firing_Tail);
    }

}

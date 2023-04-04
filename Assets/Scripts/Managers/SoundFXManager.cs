using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFXManager : MonoBehaviour
{
    [SerializeField] private AudioListener _audioListener;
    [SerializeField] private AudioSource _audioSource;

    public AudioSource Small_explosion;
    public AudioSource Tank_explosion;
    public AudioSource Crate_explosion;
    public AudioSource Door_Slide;
    public AudioSource PA_Jingle;
    public AudioSource Ship_Alarm;

    public AudioListener AudioListener { get => _audioListener; set => _audioListener = value; }
    public AudioSource AudioSource { get => _audioSource; set => _audioSource = value; }

    public void LoadClip(string clipName)
    {
        //AudioClip audioClip = Load
    }

    public void PlayAudio()
    {
        AudioSource.Play();
    }

    public void StopAudio()
    {
        AudioSource.Stop();
    }



    public void Play_Small_explosion()
    {

        Small_explosion.Play();

    }
    public void Play_Tank_explosion()
    {

        Tank_explosion.Play();

    }    
    public void Play_Crate_explosion()
    {

        Crate_explosion.Play();

    }

    public void Play_Door_Slide()
    {
        Door_Slide.Play();

    }

    public void Play_PA_Jingle()
    {
        PA_Jingle.Play();

    }

    public void Play_Ship_Alarm()
    {
        Ship_Alarm.Play();

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audiosfx : MonoBehaviour
{
    public AudioSource Small_explosion;
    public AudioSource Door_Slide;
    public AudioSource PA_Jingle;
    public AudioSource Ship_Alarm;

    public void Play_Small_explosion()
    {

        Small_explosion.Play();

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

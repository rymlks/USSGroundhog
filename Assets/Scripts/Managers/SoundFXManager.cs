using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFXManager : MonoBehaviour
{
    [SerializeField] private AudioListener _audioListener;
    [SerializeField] private AudioSource _audioSource;

    public AudioListener AudioListener { get => _audioListener; set => _audioListener = value; }
    public AudioSource AudioSource { get => _audioSource; set => _audioSource = value; }

    public void LoadClip(string clipName)
    {
        AudioClip audioClip = Load
    }

    public void PlayAudio()
    {
        AudioSource.Play();
    }

    public void StopAudio()
    {
        AudioSource.Stop();
    }
}

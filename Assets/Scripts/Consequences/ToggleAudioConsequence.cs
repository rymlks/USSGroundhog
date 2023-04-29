#nullable enable
using System.Collections;
using System.Collections.Generic;
using Consequences;
using Triggers;
using UnityEngine;

public class ToggleAudioConsequence : AbstractConsequence
{
    public AudioSource sourceOfAudio;

    void Start()
    {
        if (this.sourceOfAudio == null)
        {
            this.sourceOfAudio = GetComponent<AudioSource>();
        }
    }


    public override void execute(TriggerData? data)
    {
        if (this.sourceOfAudio.isPlaying)
        {
            this.sourceOfAudio.Stop();
        }
        else
        {
            this.sourceOfAudio.Play();
        }
    }
}

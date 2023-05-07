using System.Collections;
using System.Collections.Generic;
using Consequences;
using Triggers;
using UnityEngine;

public class PlaySoundConsequence : AbstractConsequence
{
    public AudioClip soundToPlay;
    private SoundEffectPlayer _soundEffectPlayer;

    void Start()
    {
        this._soundEffectPlayer = FindObjectOfType<SoundEffectPlayer>();
    }

    public override void execute(TriggerData? data)
    {
        if (soundToPlay != null)
        {
            this._soundEffectPlayer.PlaySound(soundToPlay);
        }
    }
}

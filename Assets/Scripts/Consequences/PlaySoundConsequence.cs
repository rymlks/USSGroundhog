using Audio;
using Triggers;
using UnityEngine;

namespace Consequences
{
    public class PlaySoundConsequence : AbstractConsequence
    {
        public AudioClip soundToPlay;
        private SoundEffectPlayer _soundEffectPlayer;

        void Start()
        {
            this._soundEffectPlayer = FindObjectOfType<SoundEffectPlayer>();
        }

        public override void Execute(TriggerData? data)
        {
            if (soundToPlay != null)
            {
                this._soundEffectPlayer.PlaySound(soundToPlay);
            }
        }
    }
}

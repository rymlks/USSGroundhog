#nullable enable
using Audio;
using Triggers;
using UnityEngine;

namespace Consequences
{
    public class PlayMusicForTimeConsequence : AbstractConsequence
    {
        public AudioClip music;
        public float secondsToPlay;
    
        public override void Execute(TriggerData? data)
        {
            FindObjectOfType<MusicStack>().PushMusicToStack(music, secondsToPlay);
        }
    }
}

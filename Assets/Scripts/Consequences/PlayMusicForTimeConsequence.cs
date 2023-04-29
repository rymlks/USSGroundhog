using Audio;
using Triggers;
using UnityEngine;

namespace Consequences
{
    public class PlayMusicForTimeConsequence : AbstractConsequence
    {
        public AudioClip music;
        public float secondsToPlay;
    
        public override void execute(TriggerData? data)
        {
            FindObjectOfType<MusicStack>().AddMusicToStack(music, secondsToPlay);
        }
    }
}

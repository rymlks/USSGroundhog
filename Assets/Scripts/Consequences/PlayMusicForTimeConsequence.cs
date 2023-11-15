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
        public bool useMusicStack = true;
        private System.DateTime _startTime;

        private AudioSource _audioSource;

        public void Start()
        {
            _audioSource = GetComponent<AudioSource>();
            _startTime = System.DateTime.Now;
        }
    
        public override void Execute(TriggerData? data)
        {
            if (useMusicStack)
            {
                FindObjectOfType<MusicStack>().PushMusicToStack(music, secondsToPlay);
            }
            else
            {
                _audioSource.playOnAwake = true;
                _startTime = System.DateTime.Now;

            }
        }

        public void Update()
        {
            if (_audioSource.playOnAwake && (System.DateTime.Now - _startTime).Seconds > secondsToPlay)
            {
                _audioSource.playOnAwake = false;
            }
        }
    }
}

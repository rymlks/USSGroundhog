using UnityEngine;
using static StaticUtils.UnityUtil;

namespace Consequences
{
    public class ParticleRepeatingConsequence : AbstractInterruptibleConsequence
    {
        private float _isRepeatingSince = float.MinValue;
        private ParticleSystem _toRepeat;
        private float _repeatUntil = float.MinValue + 1000f;
        private ParticleSystem _particles;

        public void Repeat(ParticleSystem toReplicate, float duration)
        {
            _isRepeatingSince = Time.time;
            _repeatUntil = Time.time + duration;
            _toRepeat = toReplicate;
        }

        public bool IsCurrentlyRepeating()
        {
            var currentTime = Time.time;
            return currentTime >= _isRepeatingSince && currentTime < _repeatUntil;
        }

        protected void Start()
        {
            this._particles = GetComponent<ParticleSystem>();
        }

        protected void Update()
        {
            if (IsCurrentlyRepeating())
            {
                PlayParticlesIfNotPlaying(_particles);
            }
        }
    }
}
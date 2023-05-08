#nullable enable
using Triggers;
using UnityEngine;

namespace Consequences
{
    public class AutomaticFireConsequence : AbstractConsequence
    {
        public float secondsToFire = 1f;
    
        public ParticleSystem casings;
        public ParticleSystem bullets;

        protected float _lastExecutedTime = float.MinValue;
        private bool _firedLastFrame;
        private SoundEffectPlayer _soundFXManager;

        void Start()
        {
            if (casings == null)
            {
                casings = this.transform.Find("CartridgeCasingParticleSystem").GetComponent<ParticleSystem>();
            }

            if (bullets == null)
            {
                bullets = this.transform.Find("BulletParticleSystem").GetComponent<ParticleSystem>();
            }

            if (_soundFXManager == null)
            {
                this._soundFXManager = GetComponent<SoundEffectPlayer>();
            }
        }

        void Update()
        {
            if (_firedLastFrame && !ShouldFireThisFrame())
            {
                _soundFXManager.PlayTurretStop();
                StopAllParticleSystems();
            }
            else if (!_firedLastFrame && ShouldFireThisFrame())
            {
                _soundFXManager.PlayTurretStart();
                StartAllParticleSystems();
            }
            _firedLastFrame = ShouldFireThisFrame();
        }

        private void StartAllParticleSystems()
        {
            bullets.Play();
            casings.Play();
        }

        private void StopAllParticleSystems()
        {
            bullets.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            casings.Stop();
        }

        private bool ShouldFireThisFrame()
        {
            return this._lastExecutedTime + this.secondsToFire > Time.time;
        }

        public override void Execute(TriggerData? data)
        {
            this._lastExecutedTime = Time.time;
        }
    }
}
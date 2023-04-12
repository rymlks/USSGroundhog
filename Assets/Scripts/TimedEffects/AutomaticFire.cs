using Triggers;
using UnityEngine;

namespace TimedEffects
{
    public class AutomaticFire : MonoBehaviour, IConsequence
    {
        public float secondsToFire = 1f;
    
        public ParticleSystem casings;
        public ParticleSystem bullets;

        protected float _lastExecutedTime = float.MinValue;
        private bool _firedLastFrame;
        private SoundFXManager _soundFXManager;

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
                this._soundFXManager = GetComponent<SoundFXManager>();
            }
        }

        void Update()
        {
            if (_firedLastFrame && !ShouldFireThisFrame())
            {
                _soundFXManager.Stop_Turret_Firing();
                StopAllParticleSystems();
            }
            else if (!_firedLastFrame && ShouldFireThisFrame())
            {
                _soundFXManager.Play_Turret_Firing();
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

        public void execute(TriggerData? data)
        {
            this._lastExecutedTime = Time.time;
        }
    }
}
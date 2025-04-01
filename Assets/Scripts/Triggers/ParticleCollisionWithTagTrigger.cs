using System.Collections.Generic;
using Consequences;
using UnityEngine;
using UnityEngine.Serialization;

namespace Triggers
{
    public class ParticleCollisionWithTagTrigger : AbstractTrigger
    {
        [FormerlySerializedAs("particleSystem")]
        public ParticleSystem particles;

        public string tagToTriggerOn = "Player";
        private List<ParticleCollisionEvent> _collisionEvents;
        private readonly float _defaultRepetitionDurationSeconds = 1.0f;

        protected override void Start()
        {
            base.Start();
            _collisionEvents = new List<ParticleCollisionEvent>();

            if (particles == null)
            {
                particles = GetComponentInParent<ParticleSystem>();
            }
        }

        void OnParticleCollision(GameObject other)
        {
            ParticleRepeatingConsequence repeater = other.GetComponent<ParticleRepeatingConsequence>();
            if (other.CompareTag(tagToTriggerOn))
            {
                this.Engage(new TriggerData(
                    this.tagToTriggerOn + " object collided with " + this.particles.name + " particle",
                    _collisionEvents[^1].intersection));
                if (this.destroy)
                {
                    Destroy(this.gameObject);
                }
            }
            else if (repeater != null)
            {
                repeater.Repeat(particles, _defaultRepetitionDurationSeconds);
            }
        }
    }
}
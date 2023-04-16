using System.Collections.Generic;
using UnityEngine;

namespace Triggers
{
    public class ParticleCollisionWithTagTrigger : AbstractTrigger
    {
        public ParticleSystem particleSystem;
        public string tagToTriggerOn = "Player";
        protected List<ParticleCollisionEvent> collisionEvents;
        public int CollisionEvents { get => collisionEvents.Count; }


        protected override void Start()
        {
            base.Start();
            collisionEvents = new List<ParticleCollisionEvent>();

            if (particleSystem == null)
            {
                particleSystem = GetComponentInParent<ParticleSystem>();
            }

        }

        void OnParticleCollision(GameObject other)
        {
            particleSystem.GetCollisionEvents(other, collisionEvents);

            if (other.CompareTag(tagToTriggerOn))
            {
                if(tagToTriggerOn.Equals("Player")) Debug.LogError($"<color=yellow>Collided with Player</color>");

                this.Engage(new TriggerData(this.tagToTriggerOn+ " object collided with " + this.particleSystem.name + " particle", collisionEvents[^1].intersection));
            }

        }

    }
}
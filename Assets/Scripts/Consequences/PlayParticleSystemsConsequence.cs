#nullable enable
using Triggers;
using UnityEngine;

namespace Consequences
{
    public class PlayParticleSystemsConsequence : AbstractConsequence
    {
        public ParticleSystem[] toPlay;

        void Start()
        {
            if (toPlay == null || toPlay.Length < 1)
            {
                Debug.Log("Particle system consequence failed, associated inspector value unset.");
                Destroy(this);
            }
        }

        public override void execute(TriggerData? data)
        {
            foreach (ParticleSystem system in toPlay)
            {
                system.Play();
            }
        }
    }
}

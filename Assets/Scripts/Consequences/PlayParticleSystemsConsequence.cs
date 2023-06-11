#nullable enable
using Triggers;
using UnityEngine;

namespace Consequences
{
    public class PlayParticleSystemsConsequence : DetachableConsequence
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

        public override void Execute(TriggerData? data)
        {
            GameObject detachedParent = new GameObject();
            foreach (ParticleSystem system in toPlay)
            {
                if (detachBeforePlaying)
                {
                    system.transform.SetParent(detachedParent.transform, true);
                }
                system.Play();
            }

            if (!detachBeforePlaying)
            {
                Destroy(detachedParent);
            }
        }
    }
}

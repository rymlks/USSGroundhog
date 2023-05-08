#nullable enable
using Triggers;
using UnityEngine;

namespace Consequences
{
    public class PlayParticleSystemsInChildrenConsequence : PlayParticleSystemsConsequence
    {
        public GameObject particleSystemRoot;

        void Start()
        {
            if (toPlay.Length > 0)
            {
                Debug.Log(
                    "PlayParticleSystemsInChildrenConsequence misused; set the GameObject field or use PlayParticleSystemsConsequence");
            }

            if (particleSystemRoot == null)
            {
                particleSystemRoot = this.gameObject;
            }

            this.toPlay = particleSystemRoot.GetComponentsInChildren<ParticleSystem>();
        }
    }
}
#nullable enable
using Triggers;
using UnityEngine;

namespace Consequences
{
    public class PlayParticleSystemAtLocationConsequence : AbstractConsequence
    {
        public ParticleSystem toPlay;

        void Start()
        {
            if (toPlay == null)
            {
                Debug.Log("Particle system consequence failed, associated inspector value unset.");
                Destroy(this);
            }
        }

        public override void execute(TriggerData? data)
        {

            Vector3 positionToSplatAt = this.gameObject.transform.position; 
            if (data?.triggerLocation != null)
            {
                positionToSplatAt = data.triggerLocation.Value;
            }

            toPlay.transform.position = positionToSplatAt;

            toPlay.Play();

        }
    }
}

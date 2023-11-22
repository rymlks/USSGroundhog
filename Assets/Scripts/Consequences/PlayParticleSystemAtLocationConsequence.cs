#nullable enable
using Triggers;
using UnityEngine;

namespace Consequences
{
    public class PlayParticleSystemAtLocationConsequence : DetachableConsequence
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

        public override void Execute(TriggerData? data)
        {

            Vector3 positionToSplatAt = this.gameObject.transform.position; 
            if (data?.triggerLocation != null)
            {
                positionToSplatAt = data.triggerLocation.Value;
            }

            toPlay.transform.position = positionToSplatAt;
            if (detachBeforePlaying)
            {
                toPlay.transform.SetParent(new GameObject().transform, true);
            }

            toPlay.Play();

        }
    }
}

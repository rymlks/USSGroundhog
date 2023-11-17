#nullable enable
using Triggers;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace Consequences
{
    public class InstantiatePrefabConsequence : AbstractCancelableConsequence
    {
        public GameObject toInstantiate;
        public Transform parentToInstantiateUnder;
        [Tooltip("Normally the Trigger will dictate what location a Consequence should occur at, but this allows for overriding that.")]
        public Transform transformOverride;
        
        public ParticleSystem cancelEffect;

        protected GameObject instantiated;

        public override void Execute(TriggerData? data)
        {

            if (transformOverride != null)
            {
                instantiated = GameObject.Instantiate(toInstantiate, transformOverride.position,
                    transformOverride.rotation, parentToInstantiateUnder);
            }

            else if (data?.triggerLocation != null)
            {
                instantiated = GameObject.Instantiate(toInstantiate, data.triggerLocation.Value,
                    transformOverride.rotation, parentToInstantiateUnder);
            }
            else
            {
                instantiated = GameObject.Instantiate(toInstantiate, parentToInstantiateUnder);
            }
        }

        public override void Cancel(TriggerData? data)
        {
            if (instantiated != null)
            {
                if (cancelEffect != null)
                {
                    cancelEffect.transform.position = instantiated.transform.position;
                    cancelEffect.Play();
                }
                Destroy(instantiated);
            }
        }
    }
}

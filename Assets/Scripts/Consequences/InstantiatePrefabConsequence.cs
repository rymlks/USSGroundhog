using Triggers;
using UnityEditor;
using UnityEngine;

namespace Consequences
{
    public class InstantiatePrefabConsequence : AbstractCancelableConsequence
    {
        public GameObject toInstantiate;
        public Transform parentToInstantiateUnder;
        public Transform transformToAppearAt;

        protected GameObject instantiated;

        public override void Execute(TriggerData? data)
        {
            if (data?.triggerLocation == null)
            {
                instantiated = GameObject.Instantiate(toInstantiate, transformToAppearAt.position,
                    transformToAppearAt.rotation, parentToInstantiateUnder);
            }
            else
            {
                instantiated = GameObject.Instantiate(toInstantiate, data.triggerLocation.Value,
                    transformToAppearAt.rotation, parentToInstantiateUnder);
            }
        }

        public override void Cancel(TriggerData? data)
        {
            Destroy(instantiated);
        }
    }
}

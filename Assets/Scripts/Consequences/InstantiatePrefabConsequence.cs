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

        protected Object instantiated;
        
        public override void Execute(TriggerData? data)
        {
            instantiated = GameObject.Instantiate(toInstantiate, transformToAppearAt.position, transformToAppearAt.rotation, parentToInstantiateUnder);
        }

        public override void Cancel(TriggerData? data)
        {
            Destroy(instantiated);
        }
    }
}

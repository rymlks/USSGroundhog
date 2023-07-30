#nullable enable
using StaticUtils;
using Triggers;
using UnityEngine;

namespace Consequences
{
    public class DispenseRandomItemConsequence : AbstractCancelableConsequence, ICancelableConsequence
    {
        public GameObject[] itemPrefabsToDispense;
        public Vector3 locationToDispenseAt;
        protected GameObject spawned = null;

        void Start()
        {
            if (locationToDispenseAt == Vector3.zero)
            {
                locationToDispenseAt = this.transform.position + this.transform.forward;
            }
        }

        public override void Execute(TriggerData? data)
        {
            this.spawned = Instantiate(itemPrefabsToDispense[UnityUtil.RandomNumberBetweenZeroAnd(itemPrefabsToDispense.Length)]);
        }

        public override void Cancel(TriggerData? data)
        {
            Destroy(this.spawned);
        }
    }
}

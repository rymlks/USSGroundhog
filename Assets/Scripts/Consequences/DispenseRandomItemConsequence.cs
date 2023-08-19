#nullable enable
using StaticUtils;
using Triggers;
using UnityEngine;

namespace Consequences
{
    public class DispenseRandomItemConsequence : AbstractCancelableConsequence
    {
        public GameObject[] itemPrefabsToDispense;
        public Vector3 locationToDispenseAt;
        public Transform transformToDispenseAt;
        protected GameObject spawned = null;

        void Start()
        {
            if (transformToDispenseAt != null)
            {
                this.locationToDispenseAt = transformToDispenseAt.position;
            }
            else if (locationToDispenseAt == Vector3.zero)
            {
                locationToDispenseAt = this.transform.position + this.transform.forward;
            }
        }

        public override void Execute(TriggerData? data)
        {
            Debug.Log("Executing");
            this.spawned =
                Instantiate(itemPrefabsToDispense[UnityUtil.RandomNumberBetweenZeroAnd(itemPrefabsToDispense.Length)],
                    locationToDispenseAt, UnityUtil.RandomQuaternion());
        }

        public override void Cancel(TriggerData? data)
        {
            Destroy(this.spawned);
        }
    }
}

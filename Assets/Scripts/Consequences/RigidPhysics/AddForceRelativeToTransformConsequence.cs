#nullable enable
using Triggers;
using UnityEngine;

namespace Consequences.RigidPhysics
{
    public class AddForceRelativeToTransformConsequence : AddForceConsequence
    {
        public Transform relativeTo;

        
        protected override void Start()
        {
            base.Start();
            if (relativeTo == null)
            {
                relativeTo = GameObject.FindWithTag("Player").transform;
            }
        }
        public override void Execute(TriggerData? data)
        {
            var vectorUntoTarget = (this.transform.position - relativeTo.position).normalized;
            toApplyTo.AddForce((vectorUntoTarget + force.normalized).normalized * force.magnitude, forceMode);
        }
    }
}

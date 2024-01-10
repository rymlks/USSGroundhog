using Triggers;
using UnityEngine;

namespace Consequences.RigidPhysics
{
    public class AddForceConsequence : AbstractRigidbodyAffectingConsequence
    {
        public override void Execute(TriggerData? data)
        {
            applyForce(force);
        }

        protected void applyForce(Vector3 toApply)
        {
            toApplyTo.AddForce(toApply * Time.fixedDeltaTime, forceMode);
        }
    }
}

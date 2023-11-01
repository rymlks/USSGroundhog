using Triggers;
using UnityEngine;

namespace Consequences.RigidPhysics
{
    public class AddTorqueConsequence : AbstractRigidbodyAffectingConsequence
    {
        public override void Execute(TriggerData? data)
        {
            toApplyTo.AddTorque(force * Time.fixedDeltaTime, forceMode);
        }
    }
}

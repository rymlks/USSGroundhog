using Triggers;
using UnityEngine;

namespace Consequences.RigidPhysics
{
    public class AddForceConsequence : AbstractRigidbodyAffectingConsequence
    {
        public override void Execute(TriggerData? data)
        {
            toApplyTo.AddForce(force * Time.fixedDeltaTime, forceMode);
        }
    }
}

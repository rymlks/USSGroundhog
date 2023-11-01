using Triggers;
using UnityEngine;

namespace Consequences.RigidPhysics
{
    public class ActivateRigidbodyConsequence : AbstractConsequence
    {
        public bool destroy = false;
        public UnityEngine.Rigidbody rb;
        public Vector3 angularForce;
    
        public override void Execute(TriggerData? data)
        {
            rb.isKinematic = false;
            rb.angularVelocity = angularForce;
            if (destroy)
            {
                Destroy(gameObject);
            }
        }
    }
}

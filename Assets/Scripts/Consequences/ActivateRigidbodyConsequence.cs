using Triggers;
using UnityEngine;

namespace Consequences
{
    public class ActivateRigidbodyConsequence : AbstractConsequence
    {
        public bool destroy = false;
        public Rigidbody rb;
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

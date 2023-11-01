using UnityEngine;

namespace Consequences.RigidPhysics
{
    public abstract class AbstractRigidbodyAffectingConsequence : AbstractConsequence
    {
        public Vector3 force;
        public ForceMode forceMode = ForceMode.Force;
        public UnityEngine.Rigidbody toApplyTo;

        protected virtual void Start()
        {
            if (this.toApplyTo == null)
            {
                this.toApplyTo = GetComponent<UnityEngine.Rigidbody>();
                if (toApplyTo == null)
                {
                    toApplyTo = GetComponentInChildren<UnityEngine.Rigidbody>();
                }
            }
        }
    }
}

using Triggers;
using UnityEngine;

namespace Consequences.RigidPhysics
{
    public class AddRandomTorqueConsequence : AddTorqueConsequence
    {
        public Vector3 minimumAdditionalTorque;
        public Vector3 maximumAdditionalTorque;

        public void Start()
        {
            base.Start();
            this.force += new Vector3(UnityEngine.Random.Range(minimumAdditionalTorque.x, maximumAdditionalTorque.x),
                UnityEngine.Random.Range(minimumAdditionalTorque.y, maximumAdditionalTorque.y),
                UnityEngine.Random.Range(minimumAdditionalTorque.z, maximumAdditionalTorque.z));
        }
    }
}

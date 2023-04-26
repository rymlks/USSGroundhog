#nullable enable
using KinematicCharacterController;
using Triggers;
using UnityEngine;

namespace Consequences
{
    public class MovePlayerConsequence : MonoBehaviour, IConsequence
    {
        public Vector3 relativeDestination;
        protected KinematicCharacterMotor motor;
        
        void Start()
        {
            this.motor = FindObjectOfType<KinematicCharacterMotor>();
        }

        public void execute(TriggerData? data)
        {
            this.motor.SetPosition(this.motor.TransientPosition + relativeDestination);
        }
    }
}

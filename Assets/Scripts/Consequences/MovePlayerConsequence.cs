#nullable enable
using KinematicCharacterController;
using StaticUtils;
using Triggers;
using UnityEngine;

namespace Consequences
{
    public class MovePlayerConsequence : AbstractConsequence
    {
        public Vector3 relativeDestination;
        protected KinematicCharacterMotor motor;
        
        void Start()
        {
            this.motor = FindObjectOfType<KinematicCharacterMotor>();
        }

        public override void Execute(TriggerData? data)
        {
            UnityUtil.MoveAndRotatePlayer(relativeDestination, Quaternion.identity, motor);
        }
    }
}

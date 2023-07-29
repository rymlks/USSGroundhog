#nullable enable
using KinematicCharacterController;
using StaticUtils;
using Triggers;
using Unity.VisualScripting;
using UnityEngine;

namespace Consequences
{
    public class RotatePlayerConsequence : AbstractPlayerConsequence
    { 
        public Vector3 relativeRotation =  Vector3.zero;

        public override void Execute(TriggerData? data)
        {
            if (!this.relativeRotation.Equals(Vector3.zero))
            {
                UnityUtil.MoveAndRotatePlayer(Vector3.zero, relativeRotation, motor, camera);
            }
        }
    }
}
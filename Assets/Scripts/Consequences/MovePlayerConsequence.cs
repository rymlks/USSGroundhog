#nullable enable
using KinematicCharacterController;
using StaticUtils;
using Triggers;
using UnityEngine;

namespace Consequences
{
    public class MovePlayerConsequence : AbstractPlayerConsequence
    {
        public Vector3 relativeDestination;

        public override void Execute(TriggerData? data)
        {
            UnityUtil.MoveAndRotatePlayer(relativeDestination, Quaternion.identity, motor, camera);
        }
    }
}

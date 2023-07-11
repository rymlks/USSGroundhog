#nullable enable
using KinematicCharacterController;
using Triggers;
using UnityEngine;

namespace Consequences
{
    public abstract class AbstractPlayerConsequence : AbstractConsequence
    {
        protected KinematicCharacterMotor motor;
        protected Camera camera;
        
        void Start()
        {
            this.motor = FindObjectOfType<KinematicCharacterMotor>();
            this.camera = FindObjectOfType<FinalCharacterCamera>().Camera;
        }
    }
}

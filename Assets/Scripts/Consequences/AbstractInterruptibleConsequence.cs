#nullable enable
using Triggers;
using UnityEngine;

namespace Consequences
{
    public abstract class AbstractInterruptibleConsequence : AbstractConsequence, IInterruptibleConsequence
    {
        protected bool started = false;
        protected float _startTime = float.PositiveInfinity;

        
        public void Interrupt(TriggerData? data)
        {
            this.started = false;
        }

        public override void Execute(TriggerData? data)
        {
            this.started = true;
            this._startTime = Time.time;
        }
    }
    
}
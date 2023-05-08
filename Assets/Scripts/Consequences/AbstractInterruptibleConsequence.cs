#nullable enable
using Triggers;

namespace Consequences
{
    public abstract class AbstractInterruptibleConsequence : AbstractConsequence, IInterruptibleConsequence
    {
        protected bool started = false;
        
        public void Interrupt(TriggerData? data)
        {
            this.started = false;
        }

        public override void execute(TriggerData? data)
        {
            this.started = true;
        }
    }
    
}
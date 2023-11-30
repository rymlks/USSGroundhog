#nullable enable
using Triggers;

namespace Consequences.TriggerAffecting
{
    public class FireTriggersConsequence : AbstractTriggerAffectingConsequence
    {
        public override void Execute(TriggerData? data)
        {
            this.toToggle.Engage(data);
            foreach (AbstractTrigger trigger in additionalTriggers)
            {
                trigger.Engage(data);
            }
        }
    }
}
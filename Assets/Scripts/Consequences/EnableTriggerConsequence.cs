#nullable enable
using Triggers;

namespace Consequences
{
    public class EnableTriggerConsequence : AbstractTriggerAffectingConsequence
    {
        public override void Execute(TriggerData? data)
        {
            this.toToggle.enabled = true;
        }
    }
}
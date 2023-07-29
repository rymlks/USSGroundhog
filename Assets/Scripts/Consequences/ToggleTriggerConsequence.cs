#nullable enable
using Triggers;

namespace Consequences
{
    public class ToggleTriggerConsequence : AbstractTriggerAffectingConsequence
    {
        public override void Execute(TriggerData? data)
        {
            this.toToggle.enabled = !this.toToggle.enabled;
        }
    }
}
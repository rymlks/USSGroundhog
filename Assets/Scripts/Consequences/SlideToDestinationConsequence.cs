#nullable enable
using Triggers;

namespace Consequences
{
    public class SlideToDestinationConsequence : SlideToDestination, IConsequence
    {
        public void execute(TriggerData? data)
        {
            this.start = true;
        }
    }
}
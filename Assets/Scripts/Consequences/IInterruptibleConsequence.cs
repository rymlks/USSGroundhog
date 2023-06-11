#nullable enable
using Triggers;

namespace Consequences
{
    public interface IInterruptibleConsequence : IConsequence
    {
        public void Interrupt(TriggerData? data);
    }
}
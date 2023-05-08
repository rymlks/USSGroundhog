#nullable enable
using Triggers;

namespace Consequences
{
    public interface IConsequence
    {
        void Execute(TriggerData? data);
    }
}

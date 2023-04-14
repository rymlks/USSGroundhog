#nullable enable
using Triggers;

namespace Consequences
{
    public interface IConsequence
    {
        void execute(TriggerData? data);
    }
}

#nullable enable
using Triggers;

namespace Consequences
{
    public interface ICancelableConsequence: IConsequence
    {
        void Cancel(TriggerData? data);
    }
}

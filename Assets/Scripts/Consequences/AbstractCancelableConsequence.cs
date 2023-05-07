#nullable enable
using Triggers;
using UnityEngine;

namespace Consequences
{
    public abstract class AbstractCancelableConsequence : AbstractConsequence, ICancelableConsequence
    {
        public abstract void Cancel(TriggerData? data);
    }
}

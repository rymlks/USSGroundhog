#nullable enable
using Triggers;
using UnityEngine;

namespace Consequences
{
    public abstract class AbstractConsequence : MonoBehaviour, IConsequence
    {
        public abstract void Execute(TriggerData? data);
    }
}

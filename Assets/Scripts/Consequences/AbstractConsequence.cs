using Triggers;
using UnityEngine;

namespace Consequences
{
    public abstract class AbstractConsequence : MonoBehaviour, IConsequence
    {
        public abstract void execute();

        public virtual void execute(TriggerData data)
        {
            this.execute();
        }
    }
}
#nullable enable
using Triggers;
using UnityEngine;

namespace Consequences.ConsequenceAffecting
{
    public class CancelConsequenceConsequence : AbstractConsequence
    {
        public AbstractCancelableConsequence toCancel;
        
        void Start()
        {
            if (this.toCancel == null)
            {
                this.toCancel = GetComponent<AbstractCancelableConsequence>();
            }
        }

        public override void Execute(TriggerData? data)
        {
            toCancel.Cancel(data);
        }
    }
}

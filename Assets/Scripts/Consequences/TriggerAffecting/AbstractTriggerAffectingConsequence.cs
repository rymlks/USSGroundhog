using Triggers;

namespace Consequences.TriggerAffecting
{
    public abstract class AbstractTriggerAffectingConsequence : AbstractConsequence
    {
        public AbstractTrigger toToggle;
        public AbstractTrigger[] additionalTriggers;
        void Start()
        {
            if (this.toToggle == null)
            {
                this.toToggle = GetComponent<AbstractTrigger>();
            }
        }
    }
}
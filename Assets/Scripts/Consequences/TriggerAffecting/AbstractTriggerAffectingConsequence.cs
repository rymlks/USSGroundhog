using Triggers;

namespace Consequences.TriggerAffecting
{
    public abstract class AbstractTriggerAffectingConsequence : AbstractConsequence
    {
        public AbstractTrigger toToggle;

        void Start()
        {
            if (this.toToggle == null)
            {
                this.toToggle = GetComponent<AbstractTrigger>();
            }
        }
    }
}
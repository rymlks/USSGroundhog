using Triggers;

namespace Consequences
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
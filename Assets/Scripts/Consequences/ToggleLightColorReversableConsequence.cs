#nullable enable
using Triggers;
using UnityEngine;

namespace Consequences
{
    public class ToggleLightColorReversableConsequence : AbstractCancelableConsequence
    {
        public Color goingTo;
        public Light lightToToggle;
        protected Color comingFrom;

        void Start()
        {
            if (this.lightToToggle == null)
            {
                this.lightToToggle = this.GetComponent<Light>();
            }
            this.comingFrom = this.lightToToggle.color;
        }

        public override void Execute(TriggerData? data)
        {
            if (!lightToToggle) return;
            this.lightToToggle.color = goingTo;
        }

        public override void Cancel(TriggerData? data)
        {
            if (!lightToToggle) return;
            this.lightToToggle.color = comingFrom;
        }
    }
}
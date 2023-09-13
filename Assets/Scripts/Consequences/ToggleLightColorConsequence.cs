#nullable enable
using Triggers;
using UnityEngine;

namespace Consequences
{
    public class ToggleLightColorConsequence : AbstractConsequence
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

            if (this.lightToToggle.color == comingFrom)
            {
                this.lightToToggle.color = goingTo;
            }
            else if (this.lightToToggle.color == goingTo)
            {
                this.lightToToggle.color = this.comingFrom;
            }
        }
    }
}
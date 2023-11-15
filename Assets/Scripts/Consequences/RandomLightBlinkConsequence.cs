#nullable enable
using Triggers;
using UnityEngine;

namespace Consequences
{
    public class RandomLightBlinkConsequence : AbstractInterruptibleConsequence
    {
        public Light lightToBlink;

        public float minSecondsBetweenSwitching = 2;
        public float maxSecondsBetweenSwitching = 4;
        private float nextSwitchTime;

        void Start()
        {
            if (this.lightToBlink == null)
            {
                this.lightToBlink = this.GetComponent<Light>();
            }
        }

        public override void Execute(TriggerData? data)
        {
            base.Execute(data);
            generateNextSwitchTime();
        }

        void Update()
        {
            if (this.started && Time.time >= nextSwitchTime)
            {
                doSwitch();
                generateNextSwitchTime();
            }
        }

        private void doSwitch()
        {
            this.lightToBlink.enabled = !this.lightToBlink.enabled;
        }

        private void generateNextSwitchTime()
        {
            this.nextSwitchTime = Time.time + UnityEngine.Random.Range(minSecondsBetweenSwitching, maxSecondsBetweenSwitching);
        }
    }
}
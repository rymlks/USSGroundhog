using System.Collections.Generic;
using System.Linq;
using Triggers;
using UnityEngine;

namespace Consequences
{
    public class BlinkActiveStatusConsequence : AbstractInterruptibleConsequence
    {
        private float nextSwitchTime;

        public float minSecondsBetweenSwitching = 2;
        public float maxSecondsBetweenSwitching = 4;
        public GameObject toBlink;
        
        private List<Transform> controlledObjects;

        void Start()
        {
            if (this.toBlink == null)
            {
                toBlink = this.gameObject;
            }

            this.controlledObjects = toBlink.GetComponentsInChildren<Transform>().ToList();
            this.controlledObjects.Remove(this.transform);
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
            foreach (Transform switchableTransform in this.controlledObjects)
            {
                switchableTransform.gameObject.SetActive(!switchableTransform.gameObject.activeSelf);
            }
        }

        private void generateNextSwitchTime()
        {
            this.nextSwitchTime = Time.time + UnityEngine.Random.Range(minSecondsBetweenSwitching, maxSecondsBetweenSwitching);
        }
    }
}
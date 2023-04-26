#nullable enable
using System.Collections.Generic;
using System.Linq;
using Triggers;
using UnityEngine;

namespace Consequences
{
    public class ExecuteAfterTimeElapsedConsequence : MonoBehaviour, IConsequence
    {
        public float waitTimeSeconds = 2f;
        public bool includeChildren = false;
        protected float startTime = float.PositiveInfinity;
        protected TriggerData triggerDataFromActivator;
        protected bool isActive = true;

        void Update()
        {
            if (this.isActive && Time.time > this.startTime + this.waitTimeSeconds)
            {
                this.executeLinkedConsequences();
            }
        }

        private void executeLinkedConsequences()
        {
            getLinkedConsequences().ForEach(consequence => consequence.execute(triggerDataFromActivator));
            this.isActive = false;
        }

        protected List<IConsequence> getLinkedConsequences()
        {
            if (this.includeChildren)
            {
                return GetComponentsInChildren<IConsequence>().ToList();
            }
            else
            {
                return GetComponents<IConsequence>().ToList();
            }
        }

        public void execute(TriggerData? data)
        {
            this.startTime = Time.time;
            this.triggerDataFromActivator = data;
        }
    }
}
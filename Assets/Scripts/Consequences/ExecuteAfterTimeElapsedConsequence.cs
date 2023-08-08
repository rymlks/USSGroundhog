#nullable enable
using System.Collections.Generic;
using System.Linq;
using Triggers;
using UnityEngine;

namespace Consequences
{
    public class ExecuteAfterTimeElapsedConsequence : AbstractConsequence
    {
        public float waitTimeSeconds = 2f;
        public bool includeChildren = false;
        public bool useFixedUpdate = false;

        public GameObject consequenceObject;
        
        protected float startTime = float.PositiveInfinity;
        protected TriggerData triggerDataFromActivator;
        protected bool isActive = true;
        private bool _isconsequenceObjectNull;

        private void Start()
        {
            if (this.consequenceObject == null)
            {
                this.consequenceObject = this.gameObject;
            }
        }

        void Update()
        {
            if (!useFixedUpdate)
            {
                this.checkForTimeElapsed();
            }
        }

        void FixedUpdate()
        {
            if (useFixedUpdate)
            {
                this.checkForTimeElapsed();
            }
        }

        private void checkForTimeElapsed()
        {
            if (this.isActive && Time.time > this.startTime + this.waitTimeSeconds)
            {
                this.executeLinkedConsequences();
            }
        }

        private void executeLinkedConsequences()
        {
            getLinkedConsequences().ForEach(consequence => consequence.Execute(triggerDataFromActivator));
            this.isActive = false;
        }

        protected List<IConsequence> getLinkedConsequences()
        {
            if (this.includeChildren)
            {
                return consequenceObject.GetComponentsInChildren<IConsequence>().ToList();
            }
            else
            {
                return consequenceObject.GetComponents<IConsequence>().ToList();
            }
        }

        public override void Execute(TriggerData? data)
        {
            this.startTime = Time.time;
            this.triggerDataFromActivator = data;
            this.isActive = true;
        }
    }
}
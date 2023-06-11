#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using Triggers;
using UnityEngine;

namespace Consequences
{
    public class ToggleBehavioursConsequence : AbstractCancelableConsequence
    {
        public GameObject componentRoot;
        public GameObject[] additionalRoots;
        private List<GameObject> allRoots;
        public string componentType = "light";
        public bool inChildren = true;

        public void Start()
        {
            if (this.componentRoot == null)
            {
                componentRoot = this.gameObject;
            }

            this.allRoots = new List<GameObject>(){componentRoot};
            if (additionalRoots != null && additionalRoots.Length > 0)
            {
                allRoots.AddRange(additionalRoots);
            }
        }

        public override void Execute(TriggerData? data)
        {
            foreach (Behaviour enablable in getRelevantBehaviours())
            {
                enablable.enabled = !enablable.enabled;
            }
        }
        
        public override void Cancel(TriggerData? data)
        {
            Execute(data);
        }

        protected List<Behaviour> getRelevantBehaviours()
        {
            List<Component> toToggle = new List<Component>();
            Type toAffect = typeof(Component);
            if (componentType == "light")
            {
                toAffect = typeof(Light);
            }

            foreach (var rootObject in allRoots)
            {
                if (inChildren)
                {
                    toToggle.AddRange(rootObject.GetComponentsInChildren(toAffect, true));
                }
                else
                {
                    toToggle.AddRange(rootObject.GetComponents(toAffect));
                }
            }
            return allRoots.SelectMany(rootObject =>
            {
                return inChildren
                    ? rootObject.GetComponentsInChildren(toAffect, true)
                    : rootObject.GetComponents(toAffect);
                
            }).OfType<Behaviour>().ToList();
        }
    }
}
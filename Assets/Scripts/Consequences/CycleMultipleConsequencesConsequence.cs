#nullable enable
using System.Collections.Generic;
using System.Linq;
using StaticUtils;
using Triggers;
using UnityEngine;

namespace Consequences
{
    public class CycleMultipleConsequencesConsequence : AbstractConsequence
    {
        public GameObject[] toCycleBetween;
        protected int currentIndex = -1;
    
        void Start()
        {
            if (this.toCycleBetween == null || this.toCycleBetween.Length < 1)
            {
                this.toCycleBetween = UnityUtil.GetImmediateChildGameObjects(this.gameObject).ToArray();
            }
        }

        public override void Execute(TriggerData? data)
        {
            this.currentIndex = (this.currentIndex + 1) % toCycleBetween.Length;
            foreach (IConsequence consequence in getNextAffectedConsequences())
            {
                consequence.Execute(data);
            }
        }

        private List<IConsequence> getNextAffectedConsequences()
        {
            return toCycleBetween[currentIndex].GetComponents<AbstractConsequence>().Where(cons => cons.enabled).Cast<IConsequence>().ToList();
        }
    }
}

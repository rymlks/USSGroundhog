#nullable enable
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
            foreach (IConsequence consequence in toCycleBetween[currentIndex].GetComponents<IConsequence>())
            {
                consequence.Execute(data);
            }
        }
    }
}

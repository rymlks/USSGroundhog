#nullable enable
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
                this.toCycleBetween = UnityUtil.GetChildGameObjects(this.gameObject).ToArray();
            }
        }

        public override void execute(TriggerData? data)
        {
            this.currentIndex = (this.currentIndex + 1) % toCycleBetween.Length;
            foreach (IConsequence consequence in toCycleBetween[currentIndex].GetComponents<IConsequence>())
            {
                consequence.execute(data);
            }
        }
    }
}
#nullable enable
using Triggers;
using UnityEngine;

namespace Consequences
{
    public class CycleMultipleConsequencesConsequence : MonoBehaviour, IConsequence
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

        public void execute(TriggerData? data)
        {
            this.currentIndex = (this.currentIndex + 1) % toCycleBetween.Length;
            foreach (IConsequence consequence in toCycleBetween[currentIndex].GetComponents<IConsequence>())
            {
                consequence.execute(data);
            }
        }
    }
}

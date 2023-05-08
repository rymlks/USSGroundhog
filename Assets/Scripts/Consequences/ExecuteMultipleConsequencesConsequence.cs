#nullable enable
using System.Collections.Generic;
using System.Linq;
using Triggers;
using UnityEngine;

namespace Consequences
{
    public class ExecuteMultipleConsequencesConsequence : AbstractConsequence
    {
        [SerializeField]
        protected GameObject[] objectsWithConsequences;

        [SerializeField] 
        protected bool executeInChildren = false;
        
        public override void Execute(TriggerData? data)
        {
            foreach (var consequenceObject in objectsWithConsequences)
            {
                if (executeInChildren)
                {
                    foreach (var consequence in consequenceObject.GetComponentsInChildren<IConsequence>())
                    {
                        consequence.Execute(data);
                    }
                }
                else
                {

                    foreach (var consequence in consequenceObject.GetComponents<IConsequence>())
                    {
                        consequence.Execute(data);
                    }
                }
            }
        }
    }
}

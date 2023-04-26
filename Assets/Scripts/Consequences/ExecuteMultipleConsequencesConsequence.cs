#nullable enable
using System.Collections.Generic;
using System.Linq;
using Triggers;
using UnityEngine;

namespace Consequences
{
    public class ExecuteMultipleConsequencesConsequence : MonoBehaviour, IConsequence
    {
        [SerializeField]
        protected GameObject[] objectsWithConsequences;

        [SerializeField] 
        protected bool executeInChildren = false;
        
        public void execute(TriggerData? data)
        {
            foreach (var consequenceObject in objectsWithConsequences)
            {
                if (executeInChildren)
                {
                    foreach (var consequence in consequenceObject.GetComponentsInChildren<IConsequence>())
                    {
                        consequence.execute(data);
                    }
                }
                else
                {

                    foreach (var consequence in consequenceObject.GetComponents<IConsequence>())
                    {
                        consequence.execute(data);
                    }
                }
            }
        }
    }
}

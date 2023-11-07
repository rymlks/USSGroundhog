using Triggers;
using UnityEngine;

namespace Consequences
{
    public class DestroyTriggeringGameObjectConsequence : AbstractConsequence
    {
        public override void Execute(TriggerData? data)
        {
            if (data == null || data.triggeringObject == null)
            {
                Debug.Log(this.GetType().Name + " not provided triggering GameObject reference");
            }
            else
            {
                GameObject.Destroy(data.triggeringObject);
            }
        }
    }
}

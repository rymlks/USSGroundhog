#nullable enable
using Triggers;
using UnityEngine;

namespace Consequences
{
    public class ChangeTextureConsequence : AbstractConsequence
    {
        public Material newMat;
    
        public override void Execute(TriggerData? data)
        {
            if (newMat != null)
            {
                GetComponentInChildren<SkinnedMeshRenderer>().material = newMat;
            }
        }
    }
}

#nullable enable
using Triggers;
using UnityEngine;

namespace Consequences
{
    public class ChangeTextureConsequence : AbstractConsequence
    {
        public SkinnedMeshRenderer toChangeTextureOf;
        public Material newMat;

        void Start()
        {
            if (toChangeTextureOf == null)
            {
                toChangeTextureOf = GetComponentInChildren<SkinnedMeshRenderer>();
            }
        }

        public override void Execute(TriggerData? data)
        {
            if (newMat != null)
            {
                toChangeTextureOf.material = newMat;
            }
        }
    }
}

#nullable enable
using Triggers;
using UnityEngine;

namespace Consequences
{
    public class ChangeTextureConsequence : AbstractCancelableConsequence
    {
        public SkinnedMeshRenderer toChangeTextureOf;
        public Material newMat;
        public bool toggle = false;

        protected Material startMat;
        
        void Start()
        {
            if (toChangeTextureOf == null)
            {
                toChangeTextureOf = GetComponentInChildren<SkinnedMeshRenderer>();
            }
            startMat = toChangeTextureOf.material;
        }

        public override void Execute(TriggerData? data)
        {
            if (newMat != null)
            {
                toChangeTextureOf.material = newMat;
                if (toggle)
                {
                    newMat = startMat;
                    startMat = toChangeTextureOf.material;
                }
            }
        }

        public override void Cancel(TriggerData? data)
        {
			    if(toggle) {
				    this.Execute(data);
			    } else {
				    GetComponentInChildren<SkinnedMeshRenderer>().material = startMat;
			    }
        }
    }
}

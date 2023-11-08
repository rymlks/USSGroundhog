#nullable enable
using Triggers;
using UnityEngine;

namespace Consequences
{
    public class ChangeTextureConsequence : AbstractCancelableConsequence
    {
        public Material newMat;
        public bool toggle = false;

        protected Material startMat;

        public void Start()
        {
            startMat = GetComponentInChildren<SkinnedMeshRenderer>().material;
        }
        public override void Execute(TriggerData? data)
        {
            if (newMat != null)
            {
                GetComponentInChildren<SkinnedMeshRenderer>().material = newMat;
                if (toggle)
                {
                    newMat = startMat;
                    startMat = GetComponentInChildren<SkinnedMeshRenderer>().material;
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

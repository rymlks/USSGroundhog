#nullable enable
using Triggers;
using UnityEngine;

namespace Consequences
{
    public class RotateConsequence : AbstractConsequence
    {

        public float speed;
        public float max = float.PositiveInfinity;

        protected float rotato = 0;
        protected bool active = false;

        void Start()
        {
            if (max <= 0)
            {
                max = float.PositiveInfinity;
            }
            if(speed == 0){
                speed = 1;
            }
        }
        
        void FixedUpdate()
        {
            if (active)
            {
                rotato += speed;
                if (Mathf.Abs(rotato) >= Mathf.Abs(max))
                {
                    rotato = max;
                }

                transform.localRotation = Quaternion.Euler(0, rotato, 0);
            }
        }

        public override void execute(TriggerData? data)
        {
            this.active = true;
        }
    }
}

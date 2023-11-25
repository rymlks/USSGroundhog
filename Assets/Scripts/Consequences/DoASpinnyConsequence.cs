using System;
using StaticUtils;
using Triggers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Consequences
{
    public class DoASpinnyConsequence : AbstractInterruptibleConsequence, ICancelableConsequence
    {
        public GameObject toSpin;
        public float speed;
        public float maxSpeed = float.NegativeInfinity;
        public float max = float.PositiveInfinity;
        public Vector3 axis = Vector3.up;

        protected Quaternion initialRotation;
        protected float rotationScalar;

        private float rotato = 0;
        private bool goBack = false;

        protected virtual void Start()
        {
            if (toSpin == null)
            {
                this.toSpin = this.gameObject;
            }

            if (max <= 0)
            {
                max = float.PositiveInfinity;
            }
            if(speed == 0){
                speed = 1;
            }

            if (maxSpeed < speed)
            {
                maxSpeed = speed;
            }

            this.rotationScalar =
                Math.Abs(maxSpeed - speed) < 0.001 ? speed : speed + (Random.value * (maxSpeed - speed)); 

            this.initialRotation = this.toSpin.transform.localRotation;
        }


        void FixedUpdate()
        {
            if (started && !goBack)
            {
                rotato += rotationScalar;
                if (Mathf.Abs(rotato) >= Mathf.Abs(max))
                {
                    rotato = max;
                    if (speed < 0)
                    {
                        rotato = -rotato;
                    }
                }

                toSpin.transform.localRotation = this.initialRotation * Quaternion.Euler(rotato * axis.x, rotato * axis.y, rotato * axis.z);
            } else if (started && goBack)
            {
                float oldRotato = rotato;
                rotato -= rotationScalar;
                if (rotato * oldRotato <= 0)
                {
                    rotato = 0;
                }

                toSpin.transform.localRotation = this.initialRotation * Quaternion.Euler(rotato * axis.x, rotato * axis.y, rotato * axis.z);

            }
        }

        public void Cancel(TriggerData data)
        {
            goBack = true;
        }

        public override void Execute(TriggerData data)
        {
            base.Execute(data);
            goBack = false;
        }
    }
}

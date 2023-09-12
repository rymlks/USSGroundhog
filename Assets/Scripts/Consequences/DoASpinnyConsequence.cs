using System;
using StaticUtils;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Consequences
{
    public class DoASpinnyConsequence : AbstractInterruptibleConsequence
    {
        public GameObject toSpin;
        public float speed;
        public float maxSpeed = float.NegativeInfinity;
        public float max = float.PositiveInfinity;
        public Vector3 axis = Vector3.up;

        protected Quaternion initialRotation;
        protected float rotationScalar;
    
        private float rotato = 0;

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
            if (started)
            {
                rotato += rotationScalar;
                if (Mathf.Abs(rotato) >= Mathf.Abs(max))
                {
                    rotato = max;
                }

                toSpin.transform.localRotation *= Quaternion.Euler(rotationScalar * axis.x, rotationScalar * axis.y, rotationScalar * axis.z);
            }
        }
    }
}

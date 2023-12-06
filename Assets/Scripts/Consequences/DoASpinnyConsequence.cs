using System;
using StaticUtils;
using Triggers;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
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
        public bool loop = false;

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

            if (speed == 0)
            {
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
                if (!goBack)
                {
                    rotato += rotationScalar;
                    if (Mathf.Abs(rotato) >= Mathf.Abs(max))
                    {
                        DestinationReached();
                    }
                }
                else
                {
                    float oldRotato = rotato;
                    rotato -= rotationScalar;
                    if (rotato * oldRotato <= 0)
                    {
                        DestinationReached();
                    }
                }

                toSpin.transform.localRotation = this.initialRotation *
                                                 Quaternion.Euler(rotato * axis.x, rotato * axis.y,
                                                     rotato * axis.z);
            }
        }

        public void DestinationReached()
        {
            if (goBack)
            {
                rotato = 0;
            }
            else
            {
                rotato = max;
            }

            if (loop)
            {
                rotationScalar *= -1;
                max *= -1;
            }
            else
            {
                started = false;
            }
        }

        public void Cancel(TriggerData? data)
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
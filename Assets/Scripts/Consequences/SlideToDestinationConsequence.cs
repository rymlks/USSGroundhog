#nullable enable
using Triggers;
using UnityEngine;

namespace Consequences
{
    public class SlideToDestinationConsequence : AbstractCancelableConsequence
    {
        public Transform destination;
        public float speed = 0.1f;
        public bool forward = true;
        public Transform objectToSlide = null;
        public bool shouldReturn = true;
        public bool shouldReturnOnce = false;
        protected Vector3 startPos;
        protected bool started = false;

        private void Start()
        {
            destination.parent = null;
            if (objectToSlide == null)
            {
                objectToSlide = transform;
            }
            startPos = objectToSlide.transform.position;
        }

        protected Transform getTransform()
        {
            if (this.objectToSlide)
            {
                return objectToSlide;
            }
            else
            {
                return this.transform;
            }
        }

        void Update()
        {
            if (started)
            {
                Vector3 dest = forward ? destination.position : startPos;
                if ((dest - getTransform().position).magnitude > speed)
                {
                    getTransform().position +=
                        (dest - getTransform().position).normalized * speed * Time.deltaTime * 50;
                }
                else
                {
                    getTransform().position = dest;
                    
                    if (shouldReturn)
                    {
                        forward = !forward;
                        if (shouldReturnOnce && forward)
                        {
                            started = false;
                        }
                    }
                    else
                    {
                        started = false;
                    }
                }
            }
        }

        public override void Execute(TriggerData? data)
        {
            this.started = true;
            this.forward = true;
        }

        public override void Cancel(TriggerData? data)
        {
            this.forward = false;
            this.started = true;
        }
    }
}
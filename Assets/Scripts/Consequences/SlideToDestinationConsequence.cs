#nullable enable
using Triggers;
using UnityEngine;

namespace Consequences
{
    public class SlideToDestinationConsequence : MonoBehaviour, IConsequence
    {
        public Transform destination;
        public float speed = 0.1f;
        public bool start = false;
        public bool forward = true;
        public Transform objectToSlide = null;
        protected Vector3 startPos;
        public bool shouldReturn = true;

        private void Start()
        {
            destination.parent = null;
            startPos = transform.position;

            if (objectToSlide == null)
            {
                objectToSlide = transform;
            }
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
            if (start)
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
                    }
                }
            }
        }

        public void execute(TriggerData? data)
        {
            this.start = true;
        }
    }
}
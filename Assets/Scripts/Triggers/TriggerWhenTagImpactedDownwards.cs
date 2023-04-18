using UnityEngine;

namespace Triggers
{
    public class TriggerWhenTagImpactedDownwards : AbstractTrigger
    {
        public float heightOffset = -1f;
        public string tagToKill = "Player";

        protected bool isAboveObject(Collision collision)
        {
            return collision.transform.position.y <= this.transform.position.y + heightOffset;
        }

        protected bool isMovingDown()
        {
            return !this.GetComponent<SlideToDestination>().forward;
        }

        void OnCollisionEnter(Collision other)
        {
            if (ShouldActOnObject(other))
            {
                if (isAboveObject(other) && isMovingDown())
                {
                    this.Engage(new TriggerData(other.gameObject.name + " struck by " + this.gameObject.name, other.contacts[0].point));
                }
            }
        }

        private bool ShouldActOnObject(Collision other)
        {
            return other.gameObject.CompareTag(tagToKill);
        }
    }
}

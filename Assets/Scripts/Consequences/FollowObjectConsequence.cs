using UnityEngine;

namespace Consequences
{
    public class FollowObjectConsequence : AbstractInterruptibleConsequence
    {
        public GameObject player;
        public GameObject toMove;
        public float speed;

        void Start()
        {
            if (this.toMove == null)
            {
                this.toMove = this.gameObject;
            }
        }

        void Update()
        {
            if (started)
            {
                toMove.transform.position +=
                    (player.transform.position + new Vector3(0, 0.75f, 0) - transform.position).normalized *
                    (speed * Time.deltaTime);
            }
        }
    }
}
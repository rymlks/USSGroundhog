using KinematicCharacterController;
using UnityEngine;

namespace Consequences
{
    public class FollowPlayerConsequence : FollowObjectConsequence
    {
        protected override void Start()
        {
            base.Start();
            if (this.player == null)
            {
                this.player = GameObject.FindObjectOfType<KinematicCharacterMotor>().gameObject;
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
using UnityEngine;

namespace Triggers
{
    public class TriggerWhileObjectVisible : AbstractTrigger
    {
        public string tagToWatchFor = "Player";
        public Transform viewpoint;
        public float maximumDistanceToSee = 100f;
        private GameObject _target;
        public GameObject turretLeftRightPivot;
        public GameObject turretUpDownPivot;
        public float speed = 20;
        public Vector3 targetOffset = Vector3.zero;

        protected override void Start()
        {
            base.Start();
            if (viewpoint == null)
            {
                viewpoint = this.transform;
            }

            this._target = GameObject.FindWithTag(tagToWatchFor);
        }

        void Update()
        {
            if (CanSeeTarget())
            {
                this.TurnToFace();
                this.Engage();
            }
        }

        private void TurnToFace()
        {
                
            Quaternion rotTarget = Quaternion.LookRotation(_target.transform.position - this.transform.position + this.targetOffset);

            this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, rotTarget, speed * Time.deltaTime);

            float leftRightTurretPivot = this.transform.rotation.eulerAngles.y - turretLeftRightPivot.transform.eulerAngles.y;

            turretLeftRightPivot.transform.Rotate(0, leftRightTurretPivot, 0);

        }

        protected bool CanSeeTarget()
        {
            var hitInfo = turretLock();

            if (hitInfo.transform != null && hitInfo.transform.CompareTag(tagToWatchFor))
            {

                return true;

            } else
            {
                return false;

            };
        }

        private RaycastHit turretLock()
        {
            Physics.Raycast(new Ray(this.viewpoint.position, _target.transform.position - this.viewpoint.position),
                out RaycastHit hitInfo, maximumDistanceToSee);

            return hitInfo;
        }

    }
}


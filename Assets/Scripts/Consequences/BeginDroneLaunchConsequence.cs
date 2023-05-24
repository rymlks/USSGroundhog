#nullable enable
using Triggers;
using UnityEngine;

namespace Consequences
{
    public class BeginDroneLaunchConsequence : MonoBehaviour, IConsequence
    {

        public GameObject drone;
        public GameObject dronePrefab;
        public GameObject explosionPrefab;

        protected bool started = true;
        protected float explosionTime = -1f;

        protected bool arePrerequisitesMet()
        {
            return false;
        }

        void Update()
        {
            if (this.explosionTime > 0 && this.explosionTime <= Time.time)
            {
                CauseExplosion();
                this.explosionTime = -1f;
                Destroy(drone);
            }
        }

        private void CauseExplosion()
        {
            GameObject.Instantiate(explosionPrefab, drone.transform.position, Quaternion.identity);
            Destroy(this.drone);
        }

        public void Execute(TriggerData? data)
        {
            this.BeginLaunch(drone);
            if (!this.arePrerequisitesMet())
            {
                ScheduleExplosion();
            }
        }

        private void ScheduleExplosion()
        {
            this.explosionTime = Time.time + 7.5f;
        }

        private void BeginLaunch(GameObject toLaunch)
        {
            this.started = true;
            MoveObjectAlongWaypointsConsequence movement = toLaunch.GetComponent<MoveObjectAlongWaypointsConsequence>();
            movement.Execute(null);
        }
    }
}

#nullable enable
using Triggers;
using UnityEngine;

namespace Consequences
{
    public class MoveObjectAlongWaypointsConsequence : MonoBehaviour, IConsequence
    {
        public GameObject toMove;
        public bool loop = false;
        public Vector3[] waypoints;
        public float timeToTake = 10f;
        
        private bool _active = false;
        private float _timePerWaypoint;
        private int _currentWaypointIndex = -1;
        private float _timeStartedCurrentWaypoint;
        private Vector3 _positionWhenStartedCurrentWaypoint;

        void Start()
        {
            this._timePerWaypoint = timeToTake / (float)waypoints.Length;
            if (toMove == null)
            {
                toMove = this.gameObject;
            }
        }
    
        void Update()
        {
            if (_active)
            {
                if (_currentWaypointIndex == -1 || _timePerWaypoint < timeElapsedCurrentWaypoint())
                {
                    setDestinationToNextWaypoint();
                }
            }

            if (_active)
            {
                move();
            }
        }

        private float timeElapsedCurrentWaypoint()
        {
            return (Time.time - _timeStartedCurrentWaypoint);
        }

        private void move()
        {
            toMove.transform.localPosition = Vector3.Lerp(_positionWhenStartedCurrentWaypoint, waypoints[_currentWaypointIndex],
                Mathf.Min(timeElapsedCurrentWaypoint() / _timePerWaypoint, 1f));
        }

        private void setDestinationToNextWaypoint()
        {
            this._currentWaypointIndex++;
            this._timeStartedCurrentWaypoint = Time.time;
            _positionWhenStartedCurrentWaypoint = this.transform.localPosition;
            if (journeyHasFinished())
            {
                this._currentWaypointIndex = 0;
                if(!loop)
                {
                    Reset();
                }
            }
        }

        void Reset()
        {
            this._active = false;
            this._currentWaypointIndex = -1;
            this._positionWhenStartedCurrentWaypoint = this.transform.position;
        }


        private bool journeyHasFinished()
        {
            return this._currentWaypointIndex >= this.waypoints.Length;
        }

        public void execute(TriggerData? data)
        {
            this._active = true;
        }
    }
}

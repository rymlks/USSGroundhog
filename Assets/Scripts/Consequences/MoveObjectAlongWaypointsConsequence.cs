#nullable enable
using Triggers;
using UnityEngine;

namespace Consequences
{
    public class MoveObjectAlongWaypointsConsequence : MonoBehaviour, IConsequence
    {
        public GameObject toMove;
        public bool loop = false;
        public float timeToTake = 10f;
        public bool coordinatesAreRelative = false;
        public Vector3[] waypoints;
        
        protected bool _active = false;
        protected float _timePerWaypoint;
        protected int _currentWaypointIndex = -1;
        protected float _timeStartedCurrentWaypoint;
        protected Vector3 _positionWhenStartedCurrentWaypoint;

        protected virtual void Start()
        {
            this._timePerWaypoint = timeToTake / (float)waypoints.Length;
            if (toMove == null)
            {
                toMove = this.gameObject;
            }

            if (coordinatesAreRelative)
            {
                for (int i = 0; i < waypoints.Length; i++)
                {
                    waypoints[i] = waypoints[i] + toMove.transform.localPosition;
                }
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

        protected float timeElapsedCurrentWaypoint()
        {
            return (Time.time - _timeStartedCurrentWaypoint);
        }

        protected virtual void move()
        {
            toMove.transform.localPosition = Vector3.Lerp(_positionWhenStartedCurrentWaypoint, waypoints[_currentWaypointIndex],
                Mathf.Min(timeElapsedCurrentWaypoint() / _timePerWaypoint, 1f));
        }

        private void setDestinationToNextWaypoint()
        {
            this._currentWaypointIndex++;
            this._timeStartedCurrentWaypoint = Time.time;
            _positionWhenStartedCurrentWaypoint = toMove.transform.localPosition;
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
            this._positionWhenStartedCurrentWaypoint = toMove.transform.position;
        }


        private bool journeyHasFinished()
        {
            return this._currentWaypointIndex >= this.waypoints.Length;
        }

        public virtual void execute(TriggerData? data)
        {
            this._active = true;
        }
    }
}
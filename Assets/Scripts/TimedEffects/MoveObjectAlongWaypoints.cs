using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObjectAlongWaypoints : MonoBehaviour
{
    public GameObject toMove;
    public bool loop = false;
    public Vector3[] waypoints;
    public float timeToTake = 10f;
    public bool active = true;
    private float _timePerWaypoint;
    private int _currentWaypointIndex = -1;
    private float _timeStartedCurrentWaypoint;

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
        if (active)
        {
            if (_currentWaypointIndex == -1 || _timePerWaypoint < timeElapsedCurrentWaypoint())
            {
                setDestinationToNextWaypoint();
            }

            move();
        }
    }

    private float timeElapsedCurrentWaypoint()
    {
        return (Time.time - _timeStartedCurrentWaypoint);
    }

    private void move()
    {
        toMove.transform.position = Vector3.Slerp(toMove.transform.position, waypoints[_currentWaypointIndex],
            Mathf.Min(timeElapsedCurrentWaypoint() / _timePerWaypoint, 1f));
    }

    private void setDestinationToNextWaypoint()
    {
        this._currentWaypointIndex++;
        this._timeStartedCurrentWaypoint = Time.time;
        if (journeyHasFinished())
        {
            this._currentWaypointIndex = 0;
            if(!loop)
            {
                this.active = false;
            }
        }
    }

    private bool journeyHasFinished()
    {
        return this._currentWaypointIndex >= this.waypoints.Length;
    }
}

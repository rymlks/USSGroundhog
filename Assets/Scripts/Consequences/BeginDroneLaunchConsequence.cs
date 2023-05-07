#nullable enable
using System.Collections;
using System.Collections.Generic;
using Consequences;
using Triggers;
using UnityEngine;

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
        //GameObject.Instantiate(, drone.transform)
    }

    public void execute(TriggerData? data)
    {
        this.BeginLaunch(drone);
        if (!this.arePrerequisitesMet())
        {
            ScheduleExplosion();
        }
    }

    private void ScheduleExplosion()
    {
        this.explosionTime = Time.time + 10f;
    }

    private void BeginLaunch(GameObject toLaunch)
    {
        this.started = true;
        MoveObjectAlongWaypointsConsequence movement = toLaunch.GetComponent<MoveObjectAlongWaypointsConsequence>();
        movement.execute(null);
    }
}

using System.Collections;
using System.Collections.Generic;
using Consequences;
using Triggers;
using UnityEngine;

public class MoveObjectInSquareConsequence : MonoBehaviour, IConsequence
{
    public GameObject toMove;
    public bool loop = false;
    public float width;
    public float height;
    public float timeToTake = 10f;

    public List<Vector3> createWaypoints()
    {
        List<Vector3> waypointsInRectangle = new List<Vector3>();
        waypointsInRectangle.Add(toMove.transform.position + new Vector3(0,0,height));
        waypointsInRectangle.Add(toMove.transform.position + new Vector3(width,0,height));
        waypointsInRectangle.Add(toMove.transform.position + new Vector3(width,0,0));
        waypointsInRectangle.Add(toMove.transform.position);
        return waypointsInRectangle;
    }

    public void execute(TriggerData? data)
    {
        MoveObjectAlongWaypoints moveAlongWaypoints = toMove.AddComponent<MoveObjectAlongWaypoints>();
        moveAlongWaypoints.loop = loop;
        moveAlongWaypoints.waypoints = createWaypoints().ToArray();
        moveAlongWaypoints.timeToTake = timeToTake;
        moveAlongWaypoints.toMove = toMove;
        moveAlongWaypoints.active = true;
    }
}

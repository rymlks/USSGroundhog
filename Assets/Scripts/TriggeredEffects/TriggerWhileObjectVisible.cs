using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerWhileObjectVisible : AbstractTrigger
{
    public string tagToWatchFor = "Player";
    public Transform viewpoint;
    public float maximumDistanceToSee = 100f;
    private GameObject _target;

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
        this.transform.LookAt(_target.transform);
    }

    protected bool CanSeeTarget()
    {
        return IsLineOfSightClear();
    }

    protected bool IsLineOfSightClear()
    {
        return Physics.Raycast(new Ray(this.viewpoint.position, _target.transform.position - this.viewpoint.position),
            out RaycastHit hitInfo, maximumDistanceToSee) 
               && hitInfo.transform.CompareTag(tagToWatchFor);
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerWhileObjectVisible : AbstractTrigger
{
    public string tagToWatchFor = "Player";
    public Transform viewpoint;
    public float maximumDistanceToSee = 100f;
    private GameObject _target;
    public GameObject turretLeftRightPivot;
    public GameObject turretUpDownPivot;

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
        //turretLeftRightPivot.transform.LookAt(_target.transform.position);
        // Change RotationX of turretUpDownPivot
        // Change RotationY of turretLeftRightPivot
        //this.transform.LookAt(_target.transform.position + new Vector3(0, 1.5f, 0));
        turretUpDownPivot.transform.LookAt(_target.transform.position + new Vector3(0, 1.5f, 0));
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


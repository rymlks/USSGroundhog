using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UIElements;

public class TriggerWhileObjectVisible : AbstractTrigger
{
    public string tagToWatchFor = "Player";
    public Transform viewpoint;
    public float maximumDistanceToSee = 100f;
    private GameObject _target;
    public GameObject turretLeftRightPivot;
    public GameObject turretUpDownPivot;
    public float speed;

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
        // Explore Quaternion.Slerp as a way to control the speed at which the turret locks onto player
        //turretUpDownPivot.transform.Rotate(turretLock().transform.position, 0, 0);
        turretUpDownPivot.transform.LookAt(_target.transform.position + new Vector3(0, 1.5f, 0));
        turretLeftRightPivot.transform.Rotate(0, turretUpDownPivot.transform.localRotation.eulerAngles.y, 0);
      
    }

    protected bool CanSeeTarget()
    {
        var hitInfo = turretLock();

        if (hitInfo.transform.CompareTag(tagToWatchFor))
        {
            Debug.Log(hitInfo.transform.position);
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


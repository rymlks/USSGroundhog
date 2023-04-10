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
    public Transform Followpos = null;
    public float speed = 20f;

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
                
        Quaternion rotTarget = Quaternion.LookRotation(_target.transform.position - this.transform.position);

        this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, rotTarget, speed * Time.deltaTime);

        // Make turret bracket match rotation of turret head

        //turretLeftRightPivot.transform.Rotate(0, turretUpDownPivot.transform.localRotation.eulerAngles.y, 0);

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


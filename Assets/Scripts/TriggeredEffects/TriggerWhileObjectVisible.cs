using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerWhileObjectVisible : MonoBehaviour
{
    public string tagToWatchFor = "Player";
    public Transform viewpoint;
    public float maximumDistanceToSee = 100f;
    private GameObject _target;

    void Start()
    {
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
            this.transform.LookAt(_target.transform);
            GetComponent<AutomaticFire>().execute();
        }
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

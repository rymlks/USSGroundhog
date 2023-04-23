using System;
using System.Collections;
using System.Collections.Generic;
using Consequences;
using JetBrains.Annotations;
using Triggers;
using UnityEngine;

public class VacuumPullZone : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private SlideToDestinationConsequence _Consequence;
    [SerializeField] private ParticleCollisionWithTagTrigger _particleCollisionWithTagTrigger;
    [SerializeField] private VacuumPullZoneModel _pullZoneModel;


    private void OnEnable()
    {
        //this.initialize
        Debug.Log($"<color=blue>{this.name} enabled</color>");
        _pullZoneModel = new VacuumPullZoneModel();
        _pullZoneModel.name = this.name +"<color=yellow>_Model</color>";
        
    }

    // Start is called before the first frame update
    void Start()
    {
        //Assignments
        _pullZoneModel.particleSystem = null != _particleSystem
            ? _particleSystem
            : transform != null
                ? transform.GetChild(0).transform.GetChild(2).GetComponent<ParticleSystem>()
                : null;
        _pullZoneModel.trigger = null != _particleCollisionWithTagTrigger
            ? _particleCollisionWithTagTrigger
            : gameObject.GetComponent<ParticleCollisionWithTagTrigger>();
        _pullZoneModel.trigger.particleSystem = _pullZoneModel.trigger != null && null != _particleSystem
            ? _particleSystem
            : null != _pullZoneModel.particleSystem
                ? _pullZoneModel.particleSystem
                : null;
        _pullZoneModel.consequence = _Consequence != null
            ? _Consequence
            : null;
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj) _pullZoneModel.consequence.objectToSlide = playerObj.transform;

        ApplyModelPropertiesToFields(_pullZoneModel);
    }

    private void ApplyModelPropertiesToFields(VacuumPullZoneModel pullZoneModel)
    {
        _particleSystem = pullZoneModel.particleSystem;
        _Consequence = pullZoneModel.consequence;
        _particleCollisionWithTagTrigger = pullZoneModel.trigger;
        _pullZoneModel = pullZoneModel;
    }

    // Update is called once per frame
    void Update()
    {
        if (_pullZoneModel.canReachPlayer)
        {
            //Pull player into point
            //Debug.LogWarning("<color=blue>KILL PLAYER</color>");
            _Consequence.execute(null);
        }
        DetermineIfCanReachPlayer();
    }

    private void DetermineIfCanReachPlayer()
    {
        _pullZoneModel.canReachPlayer = _particleCollisionWithTagTrigger.CollidedWithTag;
    }
}

public class VacuumPullZoneModel
{
    [CanBeNull] public ParticleSystem particleSystem;
    [CanBeNull] public string name;
    [CanBeNull] public ParticleCollisionWithTagTrigger trigger;
    [CanBeNull] public float decay;
    [CanBeNull] public float strength;
    public bool canReachPlayer;
    [CanBeNull] public SlideToDestinationConsequence consequence;
    [CanBeNull] public GameManager target;
}
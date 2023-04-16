using System;
using System.Collections;
using System.Collections.Generic;
using Consequences;
using JetBrains.Annotations;
using Triggers;
using Unity.VisualScripting;
using UnityEngine;

public class VaccumPullZone : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private MoveObjectInSquareConsequence _moveObjectInSquareConsequence;
    [SerializeField] private ParticleCollisionWithTagTrigger _particleCollisionWithTagTrigger;
    [SerializeField] VaccumPullZoneModel _pullZoneModel = new VaccumPullZoneModel();
    private void OnEnable()
    {
        //this.initialize
        Debug.Log($"<color=blue>{this.name} enabled</color>");
        this._pullZoneModel.name = this.name +"<color=yellow>Model</color>";
        
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log($"<color=blue>{this.name} created</color>\n {this._pullZoneModel.name}");
        /*this._pullZoneModel.particleSystem = null != this._particleSystem
            ? this._particleSystem
            : this.transform != null
                ? this.transform.GetChild(0).transform.GetChild(2).GetComponent<ParticleSystem>()
                : null;*/
        this._pullZoneModel.trigger = null != this._particleCollisionWithTagTrigger
            ? this._particleCollisionWithTagTrigger
            : this.gameObject.GetComponent<ParticleCollisionWithTagTrigger>();
        this._pullZoneModel.Consequence = _moveObjectInSquareConsequence
            ? this._moveObjectInSquareConsequence
            : this.gameObject.GetComponent<MoveObjectInSquareConsequence>();

        ApplyModelProperties(this._pullZoneModel);
    }

    public void ApplyModelProperties(VaccumPullZoneModel pullZoneModel)
    {
        //_particleSystem = pullZoneModel.particleSystem;
        _moveObjectInSquareConsequence = pullZoneModel.Consequence as MoveObjectInSquareConsequence;
        _particleCollisionWithTagTrigger = pullZoneModel.trigger as ParticleCollisionWithTagTrigger;
        _pullZoneModel = pullZoneModel;
    }

    // Update is called once per frame
    void Update()
    {
        if (_pullZoneModel.canReachPlayer)
        {
            //Pull player into point
            var playerObj = GameObject.FindWithTag("Player");
            Debug.LogError($"<color=red>I can reach player object {_pullZoneModel.canReachPlayer}</color>");
        }

        DetermineIfCanReachPlayer();
    }

    private void DetermineIfCanReachPlayer()
    {
        var playerObj = GameObject.FindWithTag("Player");
        if(playerObj == null) return;
    }
}

public class VaccumPullZoneModel
{
    [CanBeNull] public string name;
    [CanBeNull] public AbstractTrigger trigger;
    [CanBeNull] public float decay;
    [CanBeNull] public float strength;
    public bool canReachPlayer;
    [CanBeNull] public IConsequence Consequence;
    [CanBeNull] public GameManager target;
}
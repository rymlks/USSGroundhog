#nullable enable
using System;
using System.Collections.Generic;
using KinematicCharacterController;
using Managers;
using Player;
using StaticUtils;
using Triggers;
using UnityEngine;

namespace Consequences
{
    public class SwapWithElevatorConsequence : SwapTransformsConsequence
    {
        public GameObject otherElevator;
        public bool bringPlayerAlong = true;
        public bool bringEverythingAlong = true;
        public GameObject playerCharacter = null;
        
        protected KinematicCharacterMotor playerMotor = null;
        protected Camera playerCamera = null;
        protected SwapWithElevatorConsequence oppositeNumber = null;
        
        void Start()
        {
            if (otherElevator == null)
            {
                otherElevator = GameObject.FindObjectOfType<SwapWithElevatorConsequence>().gameObject;
            }

            oppositeNumber = otherElevator.GetComponentInChildren<SwapWithElevatorConsequence>();
            if (this.oppositeNumber == this)
            {
                Destroy(this);
            }

            if (bringPlayerAlong)
            {
                playerMotor = FindObjectOfType<KinematicCharacterMotor>();
                playerCamera = Camera.main != null ? Camera.main : FindObjectOfType<FinalCharacterCamera>().Camera;
                if(playerCharacter == null){
                    playerCharacter = GameObject.Find("Character");
                }
            }
        }

        public override void Execute(TriggerData? data)
        {
            this.toSwapEnd = otherElevator.transform;
            performSwap();

            // Pick which objects we want to bring with us
            List<string> physicsLayersToBring = new List<string>();
            if (bringPlayerAlong)
            {
                physicsLayersToBring.Add("Player");
            }
            if (bringEverythingAlong)
            {
                physicsLayersToBring.Add("Default");
                physicsLayersToBring.Add("Player");  // Yes, this duplication is OK
            }
            LayerMask objectFilter = LayerMask.GetMask(physicsLayersToBring.ToArray());

            BoxCollider hitbox = toSwapEnd.transform.Find("ContentCheckBox").GetComponent<BoxCollider>();
            Collider[] elevatorContents = Physics.OverlapBox(hitbox.center + hitbox.transform.position, hitbox.size * 0.5f, hitbox.transform.rotation, objectFilter.value);
            Debug.Log(elevatorContents);
            foreach (Collider col in elevatorContents)
            {
                Vector3 myPosition = col.transform.position;
                Vector3 positionRelativeToStartElevator = myPosition - toSwapEnd.position;
                Vector3 initialRotation = toSwapStart.transform.rotation.eulerAngles;
                Vector3 finalRotation = toSwapEnd.transform.rotation.eulerAngles;
                // Don't use Quaternion.Angles(), it can't be negative :((((
                Quaternion rotateBy = Quaternion.Euler(initialRotation - finalRotation);
                Vector3 positionRelativeToEndElevator = rotateBy * positionRelativeToStartElevator;
                Vector3 PositionInEndElevator = toSwapStart.transform.position + positionRelativeToEndElevator;
                Vector3 distanceToMove = PositionInEndElevator - myPosition;


                if (col.transform.CompareTag("Player"))
                {
                    UnityUtil.MoveAndRotatePlayer(distanceToMove, Quaternion.Euler(initialRotation - finalRotation), playerMotor, playerCamera);
                    continue;
                }
                Rigidbody rb = col.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.MovePosition(rb.position + distanceToMove);
                    rb.MoveRotation(rotateBy * rb.rotation);
                } else
                {
                    col.transform.position += distanceToMove;
                    col.transform.rotation = rotateBy * col.transform.rotation;
                }
            }
        }

        public override void Cancel(TriggerData? data)
        {
            throw new NotImplementedException();
        }
    }
}

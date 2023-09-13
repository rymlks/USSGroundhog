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
    public class MoveLobbiesConsequence : SwapTransformsConsequence
    {
        public Vector2Int destinationLobbyGridCoordinates;
        public bool bringPlayerAlong = true;
        public bool bringEverythingAlong = true;
        public GameObject playerCharacter = null;
        
        protected KinematicCharacterMotor playerMotor = null;
        protected Camera playerCamera = null;
        
        void Start()
        {
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
            chooseElevatorToSwapWith();
            performSwap();
            swapDestinations();

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
                }
            }
        }

        private void swapDestinations()
        {
            MoveLobbiesConsequence otherConsequence = toSwapEnd.Find("TeleportElevatorSwitch/TeleportElevatorUp")
                .GetComponent<MoveLobbiesConsequence>();
            if (otherConsequence == null)
                throw new ICantEvenRightNowException();
            Vector2Int temp = otherConsequence.destinationLobbyGridCoordinates;
            otherConsequence.destinationLobbyGridCoordinates = this.destinationLobbyGridCoordinates;
            this.destinationLobbyGridCoordinates = temp;
        }

        private void chooseElevatorToSwapWith()
        {
            string nameOfDestinationLobby = coordinatesToName(destinationLobbyGridCoordinates);
            this.toSwapEnd = UnityUtil.SelectRandomChild(GameObject.Find(nameOfDestinationLobby).transform.Find("ElevatorLobbyElevators"));
        }

        protected string coordinatesToName(Vector2Int coordinates)
        {
            return coordinates.Equals(Vector2Int.zero) ? "PuzzleStart" : 
                coordinates.Equals(new Vector2Int(999,999)) ? "PuzzleGoal" :
                (coordinates.x == 0 ? "" : coordinates.x.ToString() + "X") +
                (coordinates.y == 0 ? "" : coordinates.y.ToString() + "Z");
        }

        public override void Cancel(TriggerData? data)
        {
            throw new NotImplementedException();
        }
    }

    internal class ICantEvenRightNowException : Exception
    {
    }
}

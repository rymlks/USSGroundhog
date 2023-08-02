#nullable enable
using System;
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
            if (bringPlayerAlong)
            {
                Vector3 myPosition = playerCharacter.transform.position;
                Vector3 positionRelativeToStartElevator = myPosition - toSwapWith.position;
                Vector3 initialRotation = toSwap.transform.rotation.eulerAngles;
                Vector3 finalRotation = toSwapWith.transform.rotation.eulerAngles;
                // Don't use Quaternion.Angles(), it can't be negative :((((
                Quaternion rotateBy = Quaternion.Euler(initialRotation - finalRotation);
                Vector3 positionRelativeToEndElevator = rotateBy * positionRelativeToStartElevator;
                Vector3 PositionInEndElevator = toSwap.transform.position + positionRelativeToEndElevator;
                Vector3 distanceToMove = PositionInEndElevator - myPosition;

                UnityUtil.MoveAndRotatePlayer(distanceToMove, Quaternion.Euler(initialRotation - finalRotation), playerMotor, playerCamera);
            }
        }

        private void swapDestinations()
        {
            MoveLobbiesConsequence otherConsequence = toSwapWith.Find("TeleportElevatorSwitch/TeleportElevatorUp")
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
            this.toSwapWith = UnityUtil.SelectRandomChild(GameObject.Find(nameOfDestinationLobby).transform.Find("ElevatorLobbyElevators"));
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

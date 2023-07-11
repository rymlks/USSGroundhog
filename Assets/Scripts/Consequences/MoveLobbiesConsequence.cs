#nullable enable
using System;
using KinematicCharacterController;
using StaticUtils;
using Triggers;
using UnityEngine;

namespace Consequences
{
    public class MoveLobbiesConsequence : SwapTransformsConsequence
    {
        public Vector2Int destinationLobbyGridCoordinates;
        public bool bringPlayerAlong = true;
        protected KinematicCharacterMotor playerMotor = null;
        protected Camera playerCamera = null;
        
        void Start()
        {
            if (bringPlayerAlong)
            {
                playerMotor = FindObjectOfType<KinematicCharacterMotor>();
                playerCamera = Camera.main != null ? Camera.main : FindObjectOfType<FinalCharacterCamera>().Camera;
            }
        }

        public override void Execute(TriggerData? data)
        {
            chooseElevatorToSwapWith();
            performSwap();
            if (bringPlayerAlong)
            {
                Vector3 initialRotation = toSwap.transform.rotation.eulerAngles;
                Vector3 finalRotation = toSwapWith.transform.rotation.eulerAngles;
                Debug.Log("initial rotation of elevator: " + initialRotation);
                Debug.Log("final rotation of elevator: " + finalRotation);
                UnityUtil.MoveAndRotatePlayer(toSwap.transform.position - toSwapWith.transform.position, Quaternion.Euler(initialRotation - finalRotation), playerMotor, playerCamera);
            }
        }

        private void chooseElevatorToSwapWith()
        {
            this.toSwapWith = UnityUtil.SelectRandomChild(GameObject.Find(coordinatesToName(destinationLobbyGridCoordinates)).transform.Find("ElevatorLobbyElevators"));
        }

        protected string coordinatesToName(Vector2Int coordinates)
        {
            return coordinates.Equals(Vector2Int.zero) ? "PuzzleStart" : 
                (coordinates.x == 0 ? "" : coordinates.x.ToString() + "X") +
                (coordinates.y == 0 ? "" : coordinates.y.ToString() + "Z");
        }

        public override void Cancel(TriggerData? data)
        {
            throw new NotImplementedException();
        }
    }
}

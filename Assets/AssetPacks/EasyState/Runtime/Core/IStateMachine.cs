using EasyState.Models;

namespace EasyState.Core
{
    public interface IStateMachine
    {
        EasyStateMachine StateMachineComponent { get; }
        /// <summary>
        /// State Machine's current state
        /// </summary>
        State CurrentState { get; }
        /// <summary>
        /// Data instance this state machine is using
        /// </summary>
        DataTypeBase DataBaseInstance { get; } 

        event StateMachine.ExitDelayStartedCallback OnExitDelayStarted;       
        event StateMachine.EnterStateCallback OnStateEntered;
        event StateMachine.ExitStateCallback OnStateExited;
        event StateMachine.StateUpdatedCallback OnStateUpdated;
        event StateMachine.ActionExecuted OnActionExecuted;
        event System.Action OnUpdateCycleCompleted;
        /// <summary>
        /// Reset current state to initial state
        /// </summary>
        void Reset();
        /// <summary>
        /// Execute an update cycle on this state machine
        /// </summary>
        void Update(); 
    }
}
using EasyState.Models;
using UnityEngine;

namespace EasyState.Core
{
    /// <summary>
    /// Provides an easy way to react to changes in an <see cref="IStateMachine"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class EventHandler<T>: MonoBehaviour, IEventController where T : DataTypeBase
    {

        /// <summary>
        /// <inheritdoc cref="OnInitialize(EasyStateMachine, DataTypeBase)"/>
        /// </summary>
        /// <param name="stateMachine">State machine being initialized</param>
        /// <param name="data">Data instance state machine is using</param>
        public abstract void OnInitialize(EasyStateMachine stateMachine, T data);

        public void OnInitialize(EasyStateMachine stateMachine, DataTypeBase data)=> OnInitialize(stateMachine, data as T);

        /// <summary>
        /// <inheritdoc cref="OnPreUpdate(DataTypeBase)"/>
        /// </summary>
        /// <param name="data">
        /// Instance of data that the state machine is using
        /// </param>       
        public abstract void OnPreUpdate(T data);

        public void OnPreUpdate(DataTypeBase data)=> OnPreUpdate(data as T);

        /// <summary>
        /// Called once everytime a new state is entered
        /// </summary>
        /// <param name="stateEntered"></param>
        /// <param name="data">Instance of data that the state machine is using</param>
        public abstract void OnStateEntered(State stateEntered, T data);

        /// <summary>
        /// Called once everytime a state is updated
        /// </summary>
        /// <param name="stateUpdated">state being updated</param>
        /// <param name="data">Instance of data that the state machine is using</param>
        public abstract void OnStateUpdated(State stateUpdated, T data);

        /// <summary>
        /// Called once everytime a state is exited
        /// </summary>
        /// <param name="stateExited">state that was exited</param>
        /// <param name="data">Instance of data that the state machine is using</param>
        public abstract void OnStateExited(State stateExited, T data);

        /// <summary>
        /// Called once an exit delay is started
        /// </summary>
        /// <param name="stateMachine">state machine that has entered delay</param>
        /// <param name="delayAmount">length of exit delay</param>
        public abstract void OnExitDelayStarted(IStateMachine stateMachine, float delayAmount);

        /// <summary>
        /// <inheritdoc cref="OnPostUpdate(DataTypeBase)"/>
        /// </summary>
        /// <param name="data">Instance of data that the state machine is using</param>
        public abstract void OnPostUpdate(T data);

        public void OnPostUpdate(DataTypeBase data)=> OnPostUpdate(data as T);

        /// <summary>
        /// Called once when a state machine has been stopped
        /// </summary>
        /// <param name="stateMachineStopped">State machine that was stopped</param>
        public void OnStopped(EasyStateMachine stateMachineStopped)=> OnStopped(stateMachineStopped as EasyStateMachine<T>);
        public abstract void OnStopped(EasyStateMachine<T> stateMachine);

    }
}

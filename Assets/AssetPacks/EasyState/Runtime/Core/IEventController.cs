using EasyState.Models;

namespace EasyState.Core
{
    /// <summary>
    /// Abstracts what type of event handlers the user has chosen.
    /// </summary>
    public interface IEventController
    {
        /// <summary>
        /// Called once when a state machine is initialized.
        /// </summary>
        /// <param name="stateMachine">state machine that was initialized</param>
        /// <param name="data">instance data that state machine is using.</param>
        void OnInitialize(EasyStateMachine stateMachine, DataTypeBase data);

        /// <summary>
        /// Called prior to every update a state machine is given, useful for prepping data prior to consumption by the state machine.
        /// </summary>
        /// <param name="data">data instance from the state machine that is about to be updated.</param>

        void OnPreUpdate(DataTypeBase data);
        /// <summary>
        /// Called directly after a state machine has been updated, useful for cleaning up data post consumption by the state machine.
        /// </summary>
        /// <param name="data"> data instance from the state machine that was just updated.</param>
        void OnPostUpdate(DataTypeBase data);
        /// <summary>
        /// Called when state machine has been stopped
        /// </summary>
        /// <param name="stateMachine">State machine that was stopped</param>
        void OnStopped(EasyStateMachine stateMachine);

    }
}

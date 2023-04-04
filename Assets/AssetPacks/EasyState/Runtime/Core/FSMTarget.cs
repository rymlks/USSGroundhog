using EasyState.Models;
using UnityEngine;

namespace EasyState.Core
{
    /// <summary>
    /// Serves as an abstraction of the EasyStateMachine component exposing essential data.
    /// </summary>
    /// <remarks>
    /// This class is used extensively by the state machine event handlers. The class is meant to make it easy to
    /// track and sync data changes between individual machines and compare it to the host's data under the circumstance
    /// that they might diverge from each other.
    /// </remarks>
    public class FSMTarget : MonoBehaviour
    {
        /// <summary>
        /// The data that this machine is using to update it's state.
        /// </summary>
        public DataTypeBase Data;

        /// <summary>
        /// The data that the host machine is using to update the host machine's state.
        /// </summary>
        public FSMTarget Host;
    }
}
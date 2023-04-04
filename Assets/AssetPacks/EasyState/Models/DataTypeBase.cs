using UnityEngine;

namespace EasyState.Models
{
    /// <summary>
    /// Serves as a base type for all data types that are used in
    /// different state machines,actions,conditions..etc
    /// Do not modify this class instead create a sub data type by the Asset/Easy State menu or right clicking in
    /// the project window and selecting Easy State/New Data Type
    /// </summary>
    [EasyStateScript("4c1437a9-749f-4aea-b4ee-ead74c759f35")]
    [System.Serializable]
    public class DataTypeBase : ScriptableObject
    {
        public const string DATA_TYPE_BASE_ID = "4c1437a9-749f-4aea-b4ee-ead74c759f35";
        /// <summary>
        /// A generic data container used for passing raw data into and out of the state machine.
        /// </summary>
        [System.NonSerialized]
        public DataGrabBag DataGrabBag = new DataGrabBag();
        /// <summary>
        /// <see cref="GameObject"/> that this state machine is attached to.
        /// </summary>
        [System.NonSerialized]
        public GameObject FSM_GameObject;
        /// <summary>
        /// <see cref="Transform"/> that this state machine is attached to.
        /// </summary>
        [System.NonSerialized]
        public Transform FSM_Transform;
        //***** DO NOT MODIFY THIS CLASS ************* 
    }
}

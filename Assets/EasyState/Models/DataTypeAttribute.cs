using UnityEngine;

namespace EasyState.Models
{
    /// <summary>
    /// Draws the data that a state machine is using as well as providing a create
    /// button to make creating new data assets easier.
    /// </summary>
    public class DataTypeAttribute : PropertyAttribute
    {
        /// <summary>
        /// Setting this to true removes the create button
        /// as well as the ability to select a different type of data.
        /// </summary>
        public bool ReadOnly { get; }
        public DataTypeAttribute(bool readOnly = false)
        {
            ReadOnly = readOnly;
        }
    }
}
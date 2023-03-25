using System.Collections.Generic;
using System.Linq;

namespace EasyState.Models
{
    /// <summary>
    /// Represents an action that your state machine does
    /// </summary>
    /// <typeparam name="T">Data Type type</typeparam>
    public abstract class Action<T> : FunctionBase, IAction<T>, IAction where T : DataTypeBase
    {
        private readonly string _actionName;
        public Action()
        {
            _actionName = this.GetType().Name;
        }
        public abstract void Act(T data);
        public void BaseAct(DataTypeBase data) => Act((T)data);

        public string GetName() => _actionName;

    }
    /// <summary>
    /// Represents an action that your state machine does, with an additional parameter passed into the action. Currently supported parameter types are
    /// int, float, string, and bool 
    /// </summary>
    /// <typeparam name="T">Data Type type</typeparam>
    /// <typeparam name="T2">Parameter Type</typeparam>
    public abstract class Action<T, T2> : Action<T> where T : DataTypeBase
    {
        private T2 _value;
        private static System.Type[] _supportedTypes = new System.Type[]
        {
            typeof(int),
            typeof(float),
            typeof(string),
            typeof(bool),
        };
        public Action(T2 value)
        {
            _value = value;
            ValidateType();
        }
        private void ValidateType()
        {
            var paramType = typeof(T2);
            if (paramType.IsEnum)
            {
                return;
            }
            if (!_supportedTypes.Contains(paramType))
            {
                throw new System.InvalidOperationException("Type of " + paramType.Name + " not supported. T2 must be int, float, string, or bool.");
            }
        }
        public override void Act(T data)
        {
            Act(data, _value);
        }
        public abstract void Act(T data, T2 parameter);
    }
}
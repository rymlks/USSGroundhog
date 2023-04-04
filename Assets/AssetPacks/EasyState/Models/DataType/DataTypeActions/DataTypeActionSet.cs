using EasyState.Models.DataType;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EasyState.Models
{
    public class DataTypeActionSet<T> : IDataTypeFunctionSubset<T> where T : DataTypeBase
    {
        public IReadOnlyList<IDataTypeAction<T>> Actions => _actions;

        public IReadOnlyList<IDataTypeFunction<T>> GenericSet => _actions.Select(x=> x.AsFunction()).ToList();

        public IReadOnlyList<IDataTypeFunction> Set => this.GenericSet;

        private readonly List<IDataTypeAction<T>> _actions = new List<IDataTypeAction<T>>();

        public IDataTypeAction<T> AddAction(System.Action<T> action, string actionName)
        {
            var newAction = new CustomAction<T>(actionName, action);
            if (GenericSet.Any(x => x.Name == newAction.Name))
            {
                Debug.LogError($"Can not add action named {newAction.Name}, because an action of that name already exists in the set.");
                return null;
            }
            _actions.Add(newAction);
            return newAction;
        }
        public IDataTypeAction<T> AddLoggingMessage(System.Func<T, string> messageExp, string actionName)
        {
            var newAction = new LoggingAction<T>(actionName, messageExp);
            if (GenericSet.Any(x => x.Name == newAction.Name))
            {
                Debug.LogError($"Can not add action named {newAction.Name}, because an action of that name already exists in the set.");
                return null;
            }
            _actions.Add(newAction);
            return newAction;
        }
        public IDataTypeAction<T> AddLoggingMessage(string logMessage, string actionName)
        {
            var newAction = new LoggingAction<T>(actionName, logMessage);
            if (GenericSet.Any(x => x.Name == newAction.Name))
            {
                Debug.LogError($"Can not add action named {newAction.Name}, because an action of that name already exists in the set.");
                return null;
            }
            _actions.Add(newAction);
            return newAction;
        }
    }
}
using System;

namespace EasyState.Models.DataType
{
    public class CustomAction<T> : DataTypeFunction<T>, IDataTypeAction<T>, IAction where T : DataTypeBase
    {
        private readonly System.Action<T> _action;

        public CustomAction(string name, System.Action<T> action) : base(name)
        {
            _action = action ?? throw new ArgumentNullException(nameof(action));
        }

        public override string Id => $"{typeof(T).Name}-action-{Name}";

        public void Act(T context)
        {
            _action.Invoke(context);
        }
        public void BaseAct(DataTypeBase data) => Act((T)data);

        public string GetName() => Name;
    }
    public class LoggingAction<T> : DataTypeFunction<T>, IDataTypeAction<T>, IAction where T : DataTypeBase
    {
        private readonly Func<T, string> _logMessageFunc;
        private string _logMessage;
        public LoggingAction(string name, Func<T, string> messageFunc) : base(name)
        {
            _logMessageFunc = messageFunc ?? throw new ArgumentNullException(nameof(messageFunc));
        }
        public LoggingAction(string name, string logMessage) : base(name)
        {
            _logMessage = logMessage ?? throw new ArgumentNullException(nameof(logMessage));
        }
        public override string Id => $"{typeof(T).Name}-action-{Name}";

        public void Act(T context)
        {
            if (_logMessage == null)
            {
                UnityEngine.Debug.Log(_logMessageFunc.Invoke(context));
            }
            else
            {
                UnityEngine.Debug.Log(_logMessage);
            }
        }
        public void BaseAct(DataTypeBase data) => Act((T)data);

        public string GetName() => Name;
    }
}


using EasyState.DataModels;
using EasyState.Models.Decorators;

namespace EasyState.Models
{
    public class State : ModelData
    {
        public readonly StateType StateType;
        public bool HasConditionalActions { get; }
        public IAction[] OnEnterActions { get; set; }
        public IAction[] OnUpdateActions { get; set; }
        public IAction[] OnExitActions { get; set; }
        public string JumperMachineID { get; set; }
        public string JumperStateID { get; set; }
        public string DesignID { get; set; }
        public bool  IsRepeater { get; set; }
        public State(string stateID, string stateName, StateType stateType, bool hasConditionalActions, string designID)
        {
            HasConditionalActions = hasConditionalActions;
            StateType = stateType;
            Id = stateID;
            Name = stateName;
            StateType = stateType;
            DesignID = designID;
        }
    }
    public class State<T> : State where T : DataTypeBase
    {
        public IResolver<T> Resolver { get; set; }
        public bool HasExitDelay => GetExitDelay != null;
        public System.Func<T, float> GetExitDelay { get; set; }

        private System.Action<string> _onActionExecutedAction;
        public State(string stateID, string stateName, StateType stateType, System.Action<string> onActionExecutedCallback, bool hasConditionalActions, string designID) : base(stateID, stateName, stateType, hasConditionalActions, designID)
        {
            _onActionExecutedAction = onActionExecutedCallback;
        }

        /// <summary>
        /// Method is executed once when a state is entered
        /// </summary>
        /// <param name="data"></param>
        public void OnEnter(T data)
        {
            if (OnEnterActions != null)
            {
                for (int i = 0; i < OnEnterActions.Length; i++)
                {
                    OnEnterActions[i].BaseAct(data);
                    TriggerActionExecutionEvent(OnEnterActions[i]);
                }
            }

        }
        /// <summary>
        /// Method is executed every time a state is updated
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public void OnUpdate(T data)
        {
            if (OnUpdateActions != null)
            {
                for (int i = 0; i < OnUpdateActions.Length; i++)
                {
                    OnUpdateActions[i].BaseAct(data);
                    TriggerActionExecutionEvent(OnUpdateActions[i]);
                }
            }
        }
        /// <summary>
        /// Method is executed once when a state is exited
        /// </summary>
        /// <param name="data"></param>
        public void OnExit(T data)
        {
            if (OnExitActions != null)
            {
                for (int i = 0; i < OnExitActions.Length; i++)
                {
                    OnExitActions[i].BaseAct(data);
                    TriggerActionExecutionEvent(OnExitActions[i]);
                }
            }
        }
        public State<T> GetNext(T data)
        {
            var resolvedState = Resolver.Resolve(data);
            return resolvedState;
        }
        private void TriggerActionExecutionEvent(IAction action)
        {
            if (HasConditionalActions)
            {
                var decorator = action as ConditionalAction;
                if (decorator.ExecutedThisCycle)
                {
                    _onActionExecutedAction.Invoke(action.GetName());
                }
            }
            else
            {
                _onActionExecutedAction.Invoke(action.GetName());
            }
        }
        public override string ToString()
        {
            return $"State: {Name} Type: {StateType} Action Count: {OnEnterActions?.Length + OnExitActions?.Length + OnUpdateActions?.Length }";
        }
    }
}
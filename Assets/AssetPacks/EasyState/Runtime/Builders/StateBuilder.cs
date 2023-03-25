using EasyState.Data;
using EasyState.DataModels;
using EasyState.Models;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace EasyState.Builders
{
    public class StateBuilder<T> : Builder<State<T>> where T : DataTypeBase
    {
        private readonly State<T> _state;
        private readonly NodeData _nodeData;

        public StateBuilder(NodeData nodeData, System.Action<string> actionExecutedCallback, string designID)
        {
            _nodeData = nodeData;
            StateType stateType;
            if (_nodeData.IsEntryNode)
            {
                stateType = StateType.Entry;
            }
            else if (_nodeData.IsJumpNode)
            {
                stateType = StateType.Jumper;
            }
            else if (_nodeData.Connections.Count == 0)
            {
                stateType = StateType.Leaf;
            }
            else if (_nodeData.CycleType != NodeCycleType.PassThrough)
            {
                stateType = StateType.Cycle;
            }
            else
            {
                stateType = StateType.PassThrough;
            }
            _state = new State<T>(nodeData.Id, nodeData.Name, stateType, actionExecutedCallback, nodeData.ActionExecutionType == NodeActionExecutionType.ConditionalExecution, designID);
            if (nodeData.IsJumpNode)
            {
                _state.JumperMachineID = nodeData.JumpDesignID;
                _state.JumperStateID = nodeData.JumpNodeID;
            }
            if (nodeData.ExitDelay != 0)
            {
                _state.GetExitDelay = x => nodeData.ExitDelay;
            }
            else if (nodeData.ExitDelayField != null)
            {
                var fieldInfo = typeof(T).GetField(nodeData.ExitDelayField);
                if (fieldInfo != null)
                {
                    _state.GetExitDelay = ReflectionHelper.GetGetFieldDelegate<T,float>(fieldInfo);
                }
                else
                {
                    Debug.LogWarning($"Could not find field '{nodeData.ExitDelayField}' to base the exit delay off of");
                }
            }
            _state.IsRepeater = _nodeData.ConditionType == NodeConditionType.Repeat;
        }

        public StateBuilder<T> WithOnEnterActions(IEnumerable<IAction> actions)
        {
            _state.OnEnterActions = actions.ToArray();
            return this;
        }
        public StateBuilder<T> WithOnExitActions(IEnumerable<IAction> actions)
        {
            _state.OnExitActions = actions.ToArray();
            return this;
        }
        public StateBuilder<T> WithOnUpdateAction(IEnumerable<IAction> actions)
        {
            _state.OnUpdateActions = actions.ToArray();
            return this;
        }
        public StateBuilder<T> WithResolver(IResolver<T> resolver)
        {
            _state.Resolver = resolver;
            return this;
        }
        public State<T> BuildWithoutResolver()
        {
            return _state;
        }
        public override State<T> Build()
        {
            if (_state.Resolver == null)
            {
                throw new System.InvalidOperationException("Resolver can not be null");
            }
            return _state;
        }
    }

}
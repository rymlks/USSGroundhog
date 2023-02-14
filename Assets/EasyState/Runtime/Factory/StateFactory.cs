using EasyState.Builders;
using EasyState.Cache;
using EasyState.Core;
using EasyState.DataModels;
using EasyState.Models;
using System.Linq;

namespace EasyState.Factory
{
    public class StateFactory<T> where T : DataTypeBase
    {
        private readonly FunctionCacheSet<T> _cache;
        private readonly StateMachine<T> _stateMachine;
        public StateFactory(StateMachine<T> stateMachine)
        {
            _stateMachine = stateMachine;
            _cache = FunctionCache.DefaultCache.GetCache(_stateMachine.DataInstance);
        }
        public State<T> BuildStateWithoutResolver(NodeData data, string designID)
        {
            var stateBuilder = new StateBuilder<T>(data, _stateMachine.TriggerActionExecutedEvent, designID);

            stateBuilder.WithOnEnterActions(data.NodeActions.Where(x => x.ActionPhase == NodeActionExecutionPhase.OnEnter).Select(x => BuildAction(x)));
            stateBuilder.WithOnUpdateAction(data.NodeActions.Where(x => x.ActionPhase == NodeActionExecutionPhase.OnUpdate).Select(x => BuildAction(x)));
            stateBuilder.WithOnExitActions(data.NodeActions.Where(x => x.ActionPhase == NodeActionExecutionPhase.OnExit).Select(x => BuildAction(x)));

            return stateBuilder.BuildWithoutResolver();
        }

        private IAction BuildAction(NodeActionData data)
        {
            IAction actionFunction = _cache.GetAction(data.ActionID, data.ParameterDataString);

            var builder = new ActionBuilder(actionFunction);

            if (data.ConditionID != null)
            {
                var condition = _cache.GetCondition(data.ConditionID);
                builder = builder.WithConditional(condition).WithExpectedResult(!data.ExecuteWhenFalse);
            }
            return builder.Build();
        }
    }
}

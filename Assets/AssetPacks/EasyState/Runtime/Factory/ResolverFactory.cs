using EasyState.Cache;
using EasyState.Core;
using EasyState.Data;
using EasyState.DataModels;
using EasyState.Models;
using EasyState.Models.Resolvers;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EasyState.Factory
{
    public class ResolverFactory<T> where T : DataTypeBase
    {
        private readonly FunctionCacheSet<T> _functionCache;

        public ResolverFactory(T dataTypeInstance)
        {
            _functionCache = FunctionCache.DefaultCache.GetCache(dataTypeInstance);
        }

        public IResolver<T> BuildResolver(List<State<T>> states, NodeData node, StateMachine<T> stateMachine)
        {
            switch (node.ConditionType)
            {
                case NodeConditionType.Default:
                    return HandleDefault(states, node, stateMachine);
                case NodeConditionType.Repeat:
                    return HandleRepeaterNodes(states, node);
                case NodeConditionType.Utility:
                    return HandleEvalutor(states, node);

            }

            throw new NotImplementedException();
        }

        private IResolver<T> HandleEvalutor(List<State<T>> states, NodeData node)
        {
            List<IEvaluatorConnection<T>> evaluators = node.Connections.OrderBy(x => x.Priority).Select(x =>
             {
                 var evalFunc = _functionCache.GetEvaluator(x.EvaluatorID);

                 var destState = states.First(y => y.Id == x.DestNodeID);

                 return new EvaluatorConnection<T>(destState, evalFunc) as IEvaluatorConnection<T>;

             }).ToList();

            return new EvaluatorResolver<T>(evaluators);
        }

        private IResolver<T> HandleDefault(List<State<T>> states, NodeData node, StateMachine<T> stateMachine)
        {
            var state = states.First(x => x.Id == node.Id);
            if (state.StateType == StateType.Leaf)
            {
                var entryState = states.First(x => x.StateType == StateType.Entry && x.DesignID == state.DesignID);
                return new LeafResolver<T>(entryState);
            }
            else if (state.StateType == StateType.Jumper)
            {
                var existingState = states.FirstOrDefault(x => x.Id == node.JumpNodeID);
                if(existingState == null)
                {
                    throw new Exception("Could not find state to jump to :(");
                }
              
                return new AlwaysTrueResolver<T>(existingState); 
            }
            else
            {

                //this is an always true connection
                if (node.Connections.Count == 1 && node.ConditionType == NodeConditionType.Default)
                {
                    var destState = states.First(x => x.Id == node.Connections[0].DestNodeID);
                    return new AlwaysTrueResolver<T>(destState);
                }
                else
                {
                    List<IConditionConnection<T>> connections = BuildConnnections(states, node);
                    var fallbackConnection = connections.First(x => x is FallbackConnection<T>) as FallbackConnection<T>;
                    return new MultiConnectionResolver<T>(connections, fallbackConnection.DestState);
                }
            }
        }


        private IResolver<T> HandleRepeaterNodes(List<State<T>> states, NodeData node)
        {
            List<IConditionConnection<T>> connections = BuildConnnections(states, node);
            return new MultiConnectionResolver<T>(connections, states.First(x => x.Id == node.Id));
        }

        private List<IConditionConnection<T>> BuildConnnections(List<State<T>> states, NodeData node)
        {
            List<IConditionConnection<T>> connections = new List<IConditionConnection<T>>();

            foreach (var connection in node.Connections.OrderBy(x => x.Priority))
            {
                var destState = states.First(x => x.Id == connection.DestNodeID);

                if (node.Connections.Count == 1 && node.ConditionType == NodeConditionType.Default)
                {
                    connections.Add(new ConditionConnection<T>(destState, new AlwaysTrueCondition<T>()));
                    return connections;
                }
                if (connection.IsFallback)
                {
                    connections.Add(new FallbackConnection<T>(destState));
                    continue;
                }
                var initRow = BuildConditionExpresionFromRow(connection.ConditionalExpression.InitialConditionalRow);
                var additRows = new List<IConditionalExpression<T>>();
                if (connection.ConditionalExpression.AdditionalRows.Any())
                {
                    foreach (var row in connection.ConditionalExpression.AdditionalRows)
                    {
                        additRows.Add(BuildConditionExpresionFromRow(row));
                    }
                }

                var expression = new CompositeConditionExpression<T>(initRow, additRows);

                connections.Add(new ConditionConnection<T>(destState, expression));

            }

            return connections;
        }

        private IConditionalExpression<T> BuildConditionExpresionFromRow(ConditionalExpressionRowData data)
        {
            var initExp = BuildExpressionFromData(data.InitialExpressionData);

            var additionalExps = new List<IConditionalExpression<T>>();
            if (data.AdditionalExpressions.Any())
            {
                foreach (var condition in data.AdditionalExpressions)
                {
                    additionalExps.Add(BuildExpressionFromData(condition));
                }

            }
            return new CompositeConditionExpression<T>(initExp, additionalExps, data.ConditionalLogicType);

        }


        private ConditionExpression<T> BuildExpressionFromData(ConditionalExpressionData data)
        {
            return new ConditionExpression<T>(data.ExpectedResult, _functionCache.GetCondition(data.ConditionID), data.ConditionalLogicType);
        }
    }
}

using EasyState.DataModels;
using System.Collections.Generic;
using System.Linq;

namespace EasyState.Core.Validators
{
    public static class DesignValidator
    {
        public static DesignValidatorResult ValidateDesign(DesignData design)
        {
            var errors = new List<string>();
            var connections = design.Nodes.SelectMany(x => x.Connections);
            var additionalDesignList = new List<string>();
            if (design.Nodes.Count <= 1)
            {
                errors.Add("Design requires at least two nodes to be present.");
                return new DesignValidatorResult(errors);
            }
            foreach (var node in design.Nodes)
            {
                if (!node.IsEntryNode)
                {
                    if (!connections.Any(x => x.DestNodeID == node.Id))
                    {
                        errors.Add(node.AddNodeError("is disconnected from the rest of the design."));
                    }
                }
                if (node.ConditionType != NodeConditionType.Default)
                {
                    if (!node.Connections.Any())
                    {
                        errors.Add(node.AddNodeError($"with connection type {node.ConditionType} require at least one connection."));
                    }
                }

                if (!node.IsJumpNode)
                {
                    ValidateStateNode(errors, node);
                }
                else
                {
                    if (node.JumpDesignID is null)
                    {
                        errors.Add(node.AddNodeError("is missing a destination design."));
                    }
                    else
                    {
                        additionalDesignList.Add(node.JumpDesignID);
                    }
                    if (node.JumpNodeID is null)
                    {
                        errors.Add(node.AddNodeError("is missing a destination design node."));
                    }
                }

            }
            return new DesignValidatorResult(errors, additionalDesignList.Distinct().ToList());
        }

        private static void ValidateStateNode(List<string> errors, NodeData node)
        {
            if(node.CycleType == NodeCycleType.YieldSeconds && node.ExitDelay <= 0)
            {
                errors.Add(node.AddNodeError("Exit Delay must be greater than 0"));
            }
            if(node.CycleType == NodeCycleType.YieldVariableDelay && string.IsNullOrEmpty(node.ExitDelayField))
            {
                errors.Add(node.AddNodeError("has an unset Delay Variable under the Settings foldout."));
            }
            foreach (var action in node.NodeActions)
            {
                if (action.ActionID is null)
                {
                    errors.Add(node.AddNodeError("has an unset action(s)."));
                    break;
                }
                if (action.IsConditional && action.ConditionID is null)
                {
                    errors.Add(node.AddNodeError("has an unset action condition(s)."));
                    break;
                }
            }

            foreach (var connection in node.Connections)
            {
                if (connection.DestNodeID is null)
                {
                    errors.Add(node.AddNodeError($"has a {(connection.IsFallback ? "'Else Go to'" : string.Empty)} connection without a destination."));
                    break;
                }
                if (connection.ConnectionType is NodeConditionType.Utility)
                {
                    if (connection.EvaluatorID is null)
                    {
                        errors.Add(node.AddNodeError("has a connection that is missing an evaluator(s)"));
                    }
                }
                else
                {
                    if (connection.ConditionalExpression is null && !connection.IsFallback)
                    {
                        errors.Add(node.AddNodeError("has a connection missing a conditional expression"));
                        break;
                    }
                    if (!connection.IsFallback)
                    {
                        var initRow = connection.ConditionalExpression.InitialConditionalRow;
                        if (!ValidateConditionalRow(node, initRow, errors))
                        {
                            break;
                        }
                        foreach (var additionalRow in connection.ConditionalExpression.AdditionalRows)
                        {
                            if (!ValidateConditionalRow(node, additionalRow, errors))
                            {
                                break;
                            }
                        }
                    }
                }
            }
        }

        private static bool ValidateConditionalRow(NodeData node, ConditionalExpressionRowData rowData, List<string> errors)
        {
            if (rowData is null)
            {
                errors.Add(node.AddNodeError("has a connection with an empty conditional row."));
                return false;
            }
            if (rowData.InitialExpressionData is null)
            {
                errors.Add(node.AddNodeError("has a connection that is missing a row's expression data."));
                return false;
            }
            if (rowData.InitialExpressionData?.ConditionID is null)
            {
                errors.Add(node.AddNodeError("has a connection that is missing a condition on a conditional expression row."));
                return false;
            }
            foreach (var expression in rowData.AdditionalExpressions)
            {
                if (expression is null)
                {
                    errors.Add(node.AddNodeError("has a connection with an expression row with an invalid additional expression value"));
                    return false;
                }
                if (expression.ConditionID is null)
                {
                    errors.Add(node.AddNodeError("has a connection with an additional expression that is missing a condition."));
                }
            }
            return true;
        }
        private static string AddNodeError(this NodeData node, string error)
        {
            return $"Node '{node.Name}' {error}";
        }
    }
}

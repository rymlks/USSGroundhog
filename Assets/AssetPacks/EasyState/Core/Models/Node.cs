using EasyState.DataModels;
using EasyState.Models;
using EasyState.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EasyState.Core.Models
{
    public class Node : MoveableModel, IGroupable, IDuplicable, IDataModel<NodeData>
    {
        public event System.Action<Connection> ConnectionAdded;

        public event System.Action<Connection> ConnectionDeleted;

        public NodeActionExecutionType ActionExecutionType { get; set; }
        public List<NodeAction> Actions { get; set; } = new List<NodeAction>();
        public NodeConditionType ConditionType { get; set; }
        public IReadOnlyList<Connection> Connections => _connections;
        public NodeCycleType CycleType { get; set; }
        public Group Group { get; set; }
        public bool HasConnections => Connections.Any();
        public bool HasFallbackConnection => Connections.Any(x => x.SourceNode == this && x.IsFallback);
        public bool IsEntryNode { get; }
        public bool IsJumpNode { get; }
        public float ExitDelay { get; set; }
        public string ExitDelayField { get; set; }
        public Color NodeColor { get; set; }
        public DesignDataShort JumpDesign { get; set; }
        public NodeDataShort JumpNode { get; set; }
        public string NodeSummary { get; set; }
        public bool IsSummaryVisible { get; set; }
        public Rect Rect
        {
            get
            {
                _rect.position = Position;
                return _rect;
            }
            set
            {
                if (_rect != value)
                {
                    _rect.width = value.width;
                    _rect.height = value.height;
                }
            }
        }

        public Color SelectedNodeColor { get; set; }
        public bool ShouldHaveFallback
        {
            get
            {

                bool result = ConditionType == NodeConditionType.Default && Connections.Count > 0;
                if (Connections.Count == 1 && ConditionType == NodeConditionType.Default)
                {
                    return !IsAlwaysTrueCondition(Connections[0].ConditionalExpression);
                }
                else if (Connections.Count == 2 && ConditionType == NodeConditionType.Default)
                {
                    if (Connections[1].IsFallback)
                    {
                        return !IsAlwaysTrueCondition(Connections[0].ConditionalExpression);
                    }
                }
                return result;
            }
        }

        public bool CanDuplicate => (Group == null || !(Group?.Selected).GetValueOrDefault()) && !IsEntryNode;

        private List<Connection> _connections = new List<Connection>();
        private Rect _rect;

        public Node(NodeData nodeData) : base(nodeData)
        {
            Rect = new Rect(nodeData.Position, nodeData.Size);
            CycleType = nodeData.CycleType;
            ConditionType = nodeData.ConditionType;
            ActionExecutionType = nodeData.ActionExecutionType;
            Actions = nodeData.NodeActions.Select(x => new NodeAction(x)).ToList();
            NodeColor = nodeData.NodeColor;
            SelectedNodeColor = nodeData.SelectedNodeColor;
            IsEntryNode = nodeData.Name == NodePresetCollection.ENTRY_NODE;
            IsJumpNode = nodeData.IsJumpNode;
            ExitDelay = nodeData.ExitDelay;
            ExitDelayField = nodeData.ExitDelayField;
            NodeSummary = nodeData.NodeSummary;
            IsSummaryVisible = nodeData.IsSummaryVisible;

        }

        public Node(Vector2? position = null, Rect? rect = null, bool isEntryNode = false, bool isJumpNode = false) : base(position)
        {
            Rect = rect ?? Rect.zero;
            IsEntryNode = isEntryNode;
            IsJumpNode = isJumpNode;
        }

        public void AddConnection(Connection connection, bool addSilently = false)
        {
            _connections.Add(connection);
            if (!addSilently)
            {
                ConnectionAdded?.Invoke(connection);
                Refresh();
            }
        }

        public virtual bool CanAttachTo(Node destNode)
        {
            if (destNode == this)
            {
                return false;
            }
            return true;
        }

        public Connection GetFallbackConnection() => _connections.FirstOrDefault(x => x.IsFallback);

        public void RefreshConnections()
        {
            foreach (var conn in _connections)
            {
                conn.Refresh();
            }
        }

        public void RemoveConnection(Connection connection)
        {
            if (_connections.Remove(connection))
            {
                connection.Dispose();
                ConnectionDeleted?.Invoke(connection);
                //Remove the unnecessary fallback connection
                if (HasFallbackConnection && _connections.Count == 1)
                {
                    var fallbackCon = _connections[0];
                    _connections.Clear();
                    ConnectionDeleted?.Invoke(fallbackCon);
                }

                Refresh();
            }
        }

        private void AddFallbackConnection()
        {
            var fallback = Connection.BuildFallbackConnection(this);
            _connections.Add(fallback);
            ConnectionAdded?.Invoke(fallback);
        }
        private bool IsAlwaysTrueCondition(ConditionalExpressionSet conditionalExpression)
        {
            if (conditionalExpression != null)
            {
                if (!conditionalExpression.HasAdditionalRows)
                {
                    if (conditionalExpression.InitialConditionalRow != null)
                    {
                        var initRow = conditionalExpression.InitialConditionalRow;
                        if (!initRow.HasAdditionalExpressions)
                        {
                            if (initRow.InitialExpression?.Condition?.Id == AlwaysTrue.Id && initRow.InitialExpression?.ExpectedResult == true)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }
        public override void Refresh()
        {
            if (!HasFallbackConnection && ShouldHaveFallback)
            {
                AddFallbackConnection();
            }
            else if (HasFallbackConnection && !ShouldHaveFallback)
            {
                var fallbackConn = _connections.First(x => x.IsFallback);
                RemoveConnection(fallbackConn);
            }
            base.Refresh();
        }
        public override void SetPosition(Vector2 position)
        {
            base.SetPosition(position);
            RefreshConnections();
        }

        public override string ToString()
        {
            return $"Name : {Name ?? "None"}, Id : {Id}";
        }

        public override void UpdatePosition(Vector2 newPoint)
        {
            base.UpdatePosition(newPoint);
            RefreshConnections();
        }
        public void OnDelete(Design design) {

            if (this.IsEntryNode)
            {
                design.OnShowToast("Can not delete entry node.", 2000);
            }
            else
            {
                design.Nodes.Remove(this);
            }
        }

        public void OnGroupRefresh()
        {
            Refresh();
            RefreshConnections();
        }

        public IDuplicable Duplicate(Design design)
        {

            var nodeData = Serialize();
            nodeData.Connections.Clear();
            nodeData.Position += new Vector2(30, 30);
            nodeData.Id = Guid.NewGuid().ToString();
            var duplicatedNode = new Node(nodeData);
            duplicatedNode.JumpDesign = JumpDesign;
            duplicatedNode.JumpNode = JumpNode;
            duplicatedNode.Group = Group;
            duplicatedNode.Actions = Actions.Select(x =>
            {

                var actionData = x.Serialize();
                actionData.Id = Guid.NewGuid().ToString();
                actionData.NodeID = duplicatedNode.Id;
                var duplicatedAction = new NodeAction(actionData);
                duplicatedAction.Condition = x.Condition;
                duplicatedAction.Action = x.Action;

                return duplicatedAction;
            }).ToList();

            duplicatedNode._connections = Connections.Select(x =>
            {
                var connData = x.Serialize();
                connData.Id = Guid.NewGuid().ToString();
                connData.SourceNodeID = duplicatedNode.Id;
                ConditionalExpressionSet set = null;
                if (x.ConditionalExpression != null)
                {
                    set = new ConditionalExpressionSet(x.ConditionalExpression);
                }
                var duplicatedConnection = new Connection(connData, set, duplicatedNode, x.DestNode, x.Evaluator);
                return duplicatedConnection;
            }).ToList();

            design.Nodes.Add(duplicatedNode);

            foreach (var conn in duplicatedNode.Connections)
            {
                duplicatedNode.ConnectionAdded?.Invoke(conn);
            }
            if(Group != null)
            {
                Group.AddChild(duplicatedNode);
            }
            return duplicatedNode;
        }

        public NodeData Serialize()
        {
            var data = new NodeData
            {
                NodeActions = Actions.Select(x => x.Serialize()).ToList(),
                ConditionType = ConditionType,
                ActionExecutionType = ActionExecutionType,
                CycleType = CycleType,
                Size = Rect.size,
                NodeColor = NodeColor,
                SelectedNodeColor = SelectedNodeColor,
                IsJumpNode = IsJumpNode,
                JumpDesignID = JumpDesign?.Id,
                JumpNodeID = JumpNode?.Id,
                Connections = Connections.Select(x => x.Serialize()).ToList(),
                IsEntryNode = IsEntryNode,
                ExitDelay = ExitDelay,
                ExitDelayField = ExitDelayField,
                IsSummaryVisible = IsSummaryVisible,
                NodeSummary = NodeSummary,

            };
            data.SetModelData(this);
            return data;
        }
    }
}
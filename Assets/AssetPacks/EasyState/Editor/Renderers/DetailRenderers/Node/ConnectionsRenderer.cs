using EasyState.Core.Models;
using EasyState.Data;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using EasyState.DataModels;

namespace EasyState.Editor.Renderers.DetailRenderers
{
    internal class ConnectionsRenderer : IDisposableView
    {
        private readonly Foldout _connectionsFoldout;
        private readonly DataTypeCollection _dataTypeCollection;
        private readonly Design _design;
        private readonly Node _node;
        private NodeConditionType _cachedConditionType;
        private List<FunctionModel> _conditions;
        private List<FunctionModel> _evaluators;
        private Node _nullNode = new Node() { Name = "Select..." };

        public ConnectionsRenderer(Node node, Foldout connectionsFoldout, Design design)
        {
            _node = node;
            _connectionsFoldout = connectionsFoldout;
            _cachedConditionType = _node.ConditionType;
            _design = design;

            var db = new DataTypeDatabase();
            _dataTypeCollection = db.Load();
            var dataType = _dataTypeCollection.DataTypes.First(x => x.Id == _design.DataTypeID);
            _conditions = dataType.GetConditions();
            // _conditions.AddRange(dataType.LoadDataTypeConditionSet());
            _conditions.Insert(0, FunctionModel.NullFunction);

            _evaluators = dataType.GetEvaluators();
            _evaluators.Insert(0, FunctionModel.NullFunction);
        }

        public void Dispose()
        {
        }

        public void Refresh()
        {
            if (_cachedConditionType != _node.ConditionType)
            {
                foreach (var conn in _node.Connections.ToList())
                {
                    _node.RemoveConnection(conn);
                }

                _cachedConditionType = _node.ConditionType;
            }
            _connectionsFoldout.Clear();
            var nodeConnections = _node.Connections.Where(x => !x.IsFallback).OrderBy(x => x.Priority);

            if (_node.ConditionType == NodeConditionType.Utility)
            {
                foreach (var conn in nodeConnections)
                {
                    _connectionsFoldout.Add(BuildEvaluatorRow(conn));
                }
            }
            else
            {
                HandleConditionConnections(nodeConnections);
            }
            _connectionsFoldout.Add(BuildAddButton());
        }

        private void AddConnection()
        {
            var newConn = new Connection(_node, null);
            _node.AddConnection(newConn);
        }

        private VisualElement BuildAddButton()
        {
            var row = new VisualElement();
            row.style.flexDirection = FlexDirection.RowReverse;
            row.style.marginTop = 10;
            var addBtn = new Label("Add Connection");
            addBtn.AddToClassList("btn");
            addBtn.AddManipulator(new Clickable(AddConnection));

            row.Add(addBtn);

            return row;
        }

        private VisualElement BuildConditionRow(Connection connection)
        {
            var row = new VisualElement();
            row.name = connection.Id;
            row.AddToClassList("action-row");
            if (connection.Priority != 1)
            {
                VisualElement priorityBtn = BuildPriorityButton(connection);
                row.Add(priorityBtn);
            }
            var nameLabel = new Label($"Name : {connection.Name}");
            nameLabel.style.alignSelf = Align.Center;
            row.Add(nameLabel);
            PopupField<Node> destNodes = BuildDestNodePopup(connection);

            row.Add(destNodes);
            VisualElement deleteBtn = BuildDeleteButton(connection);
            var endSpacer = new VisualElement();
            endSpacer.style.width = 40;
            row.Add(endSpacer);
            row.Add(deleteBtn);
            row.Add(BuildEditButton(connection));

            return row;
        }

        private VisualElement BuildDeleteButton(Connection connection)
        {
            var deleteBtn = new VisualElement();
            deleteBtn.AddToClassList("close-button");
            deleteBtn.AddManipulator(new Clickable(() => OnDeleteConnection(connection.Id)));
            deleteBtn.style.top = 14;
            deleteBtn.style.right = 4;
            deleteBtn.tooltip = "Delete connection";
            return deleteBtn;
        }

        private PopupField<Node> BuildDestNodePopup(Connection connection)
        {
            var destNodeOptions = _design.Nodes.Where(x => x.Id != _node.Id).ToList();
            destNodeOptions.Insert(0, _nullNode);
            PopupField<Node> destNodes = new PopupField<Node>(destNodeOptions,
                                                                _nullNode,
                                                                x => PopupFormatters.FormatDestName(x, destNodeOptions),
                                                                x => PopupFormatters.FormatDestName(x, destNodeOptions));
            destNodes.style.width = 200;
            if (connection.DestNode != null)
            {
                destNodes.SetValueWithoutNotify(connection.DestNode);
            }
            destNodes.RegisterValueChangedCallback(x => OnDestNodeChanged(x, connection));
            return destNodes;
        }

        

        private VisualElement BuildEditButton(Connection connection)
        {
            var editBtn = new VisualElement();
            editBtn.AddToClassList("edit-icon");
            editBtn.style.position = Position.Absolute;
            editBtn.AddManipulator(new Clickable(() =>
            {
                _design.OnDetailsPanelRequested(connection);
            }));
            editBtn.tooltip = "Edit connection";
            editBtn.style.top = 12;
            editBtn.style.right = 20;
            return editBtn;
        }

        private VisualElement BuildEvaluatorPopup(Connection connection)
        {
            var popup = new PopupField<FunctionModel>(_evaluators, 0, x => x.Name, x => x.Name);
            if (connection.Evaluator != null)
            {
                popup.SetValueWithoutNotify(connection.Evaluator);
            }

            popup.RegisterValueChangedCallback(x =>
            {
                if (x.newValue == FunctionModel.NullFunction)
                {
                    connection.Evaluator = null;
                }
                else
                {
                    connection.Evaluator = x.newValue;
                }
            });

            return popup;
        }

        private VisualElement BuildEvaluatorRow(Connection connection)
        {
            var row = new VisualElement();
            row.name = connection.Id;
            row.AddToClassList("action-row");
            if (connection.Priority != 1)
            {
                VisualElement priorityBtn = BuildPriorityButton(connection);
                row.Add(priorityBtn);
            }
            var nameLabel = new Label($"If");
            nameLabel.style.alignSelf = Align.Center;
            row.Add(nameLabel);
            row.Add(BuildEvaluatorPopup(connection));
            var gotoLabel = new Label($"picked, go to");
            gotoLabel.style.alignSelf = Align.Center;
            row.Add(gotoLabel);

            PopupField<Node> destNodes = BuildDestNodePopup(connection);
            row.Add(destNodes);
            VisualElement deleteBtn = BuildDeleteButton(connection);
            var endSpacer = new VisualElement();
            endSpacer.style.width = 40;
            row.Add(endSpacer);
            row.Add(deleteBtn);
            row.Add(BuildEditButton(connection));
            return row;
        }

        private VisualElement BuildFallbackRow(Connection connection)
        {
            var row = new VisualElement();
            row.name = connection.Id;
            row.AddToClassList("action-row");
            var elseLabel = new Label("Else go to");
            elseLabel.style.alignSelf = Align.Center;
            row.Add(elseLabel);

            PopupField<Node> destNodes = BuildDestNodePopup(connection);
            row.Add(destNodes);

            return row;
        }

        private VisualElement BuildPriorityButton(Connection connection)
        {
            var priorityBtn = new VisualElement();
            priorityBtn.AddToClassList("priority-up-icon");
            priorityBtn.AddToClassList("hover-icon");
            priorityBtn.tooltip = "Move priority up";
            priorityBtn.AddManipulator(new Clickable(() => OnPriorityMoveRequest(connection.Id)));
            return priorityBtn;
        }

        private void HandleConditionConnections(IEnumerable<Connection> connections)
        {
            foreach (var _conn in connections)
            {
                _connectionsFoldout.Add(BuildConditionRow(_conn));
            }
            if (_node.HasFallbackConnection)
            {
                var fallBackConnection = _node.GetFallbackConnection();
                _connectionsFoldout.Add(BuildFallbackRow(fallBackConnection));
            }
        }

        private void OnDeleteConnection(string id)
        {
            var connectionToRemove = _node.Connections.First(x => x.Id == id);
            _node.RemoveConnection(connectionToRemove);
        }

        private void OnDestNodeChanged(ChangeEvent<Node> x, Connection connection)
        {
            if (x.newValue == _nullNode)
            {
                connection.SetDestNode(null);
                Refresh();
            }
            else
            {
                connection.SetDestNode(x.newValue);
            }
            _node.RefreshConnections();
        }
        private void CleanPriorities()
        {
            var orderedConnections = _node.Connections.OrderBy(x => x.Priority).ToList();
            for (int i = 0; i < orderedConnections.Count; i++)
            {
                var conn = orderedConnections[i];
                if (!conn.IsFallback)
                {
                    conn.Priority = i + 1;
                }
            }
        }
        private void OnPriorityMoveRequest(string id)
        {
            CleanPriorities();
            var priorityToMoveUp = _node.Connections.First(x => x.Id == id);
            int targetPriority = priorityToMoveUp.Priority - 1;

            var priorityToMoveDown = _node.Connections.First(x => x.Priority == targetPriority);
            priorityToMoveDown.Priority++;
            priorityToMoveUp.Priority--;
            Refresh();
        }
    }
}
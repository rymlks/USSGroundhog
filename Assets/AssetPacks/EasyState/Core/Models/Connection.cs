using EasyState.DataModels;
using EasyState.Core.Utility;
using EasyState.Settings;
using System;
using System.Linq;
using UnityEngine;

namespace EasyState.Core.Models
{
    public enum ConnectionDirection { Left, Right, Up, Down }

    public class Connection : MoveableModel, IDataModel<ConnectionData>
    {
        public bool AutoPosition { get; set; } = true;
        public bool CanDraw => SourceNode != null && DestNode != null;
        public ConditionalExpressionSet ConditionalExpression { get; private set; }
        public NodeConditionType ConnectionType { get; }
        public Node DestNode { get; private set; }
        public FunctionModel Evaluator { get; set; }
        public bool IsFallback { get; set; }
        public int Priority { get; set; }

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
        public Node SourceNode { get; }

        private readonly EasyStateSettings _settings = EasyStateSettings.Instance;
        private bool _initialRender = true;
        private Rect _rect = Rect.zero;

        public Connection(ConnectionData data, ConditionalExpressionSet conditionalExpressionSet, Node sourceNode, Node destNode, FunctionModel evaluator) : base(data)
        {
            SourceNode = sourceNode;
            DestNode = destNode;
            Priority = data.Priority;
            IsFallback = data.IsFallback;
            AutoPosition = data.AutoPosition;
            ConnectionType = data.ConnectionType;
            ConditionalExpression = conditionalExpressionSet;
            Evaluator = evaluator;
            if (destNode != null)
            {
                destNode.Moving += OnDestNodeMoved;
            }
        }

        public Connection(Node sourceNode, Node destNode) : base((Vector2?)null)
        {
            if (sourceNode is null)
            {
                throw new ArgumentNullException("Source node");
            }
            SourceNode = sourceNode;
            DestNode = destNode;
            ConditionalExpression = new ConditionalExpressionSet();
            bool shouldAddAlwaysTrueCondition = SourceNode.Connections.Count == 0 || _settings.ConnectionSettings.DefaultToAlwaysTrueCondition;
            if (shouldAddAlwaysTrueCondition)
            {
                ConditionalExpression.InitialConditionalRow = new ConditionalExpressionRow()
                {
                    InitialExpression = new ConditionalExpression
                    {
                        Condition = FunctionModel.AlwaysTrue
                    }
                };
            }
            Priority = SourceNode.HasConnections ? SourceNode.Connections.Where(x => !x.IsFallback).Max(x => x.Priority) + 1 : 1;
            if (destNode != null)
            {
                destNode.Moving += OnDestNodeMoved;
            }
            ConnectionType = sourceNode.ConditionType;
        }

        public static Connection BuildFallbackConnection(Node sourceNode)
        {
            var connection = new Connection(sourceNode, null);
            connection.Priority = 99;
            connection.IsFallback = true;
            connection.ConditionalExpression = null;
            return connection;
        }

        public void Dispose()
        {
            if (DestNode != null)
            {
                DestNode.Moving -= OnDestNodeMoved;
            }
        }

        public override void Refresh()
        {
            if (!CanDraw)
            {
                return;
            }
            if (AutoPosition)
            {
                var offset = _settings.ConnectionSettings.InputOutputOffset;
                if (IsFallback)
                {
                    offset += _settings.ConnectionSettings.FallbackConnectionOffset;
                }
                Vector2 a = ConnectionPointUtility.CalculateOutputPosition(SourceNode.Rect, Rect, offset);
                Vector2 b = ConnectionPointUtility.CalculateInputPosition(DestNode.Rect, Rect, offset);
                Position = ((a + b) / 2) - (Rect.size / 2);
                if (_initialRender)
                {
                    _initialRender = false;
                    Refresh();
                }
            }
            base.Refresh();
        }

        public void SetDestNode(Node destNode)
        {
            if (destNode == SourceNode || destNode == DestNode)
            {
                return;
            }
            if (DestNode != null)
            {
                DestNode?.RemoveConnection(this);
                DestNode.Moving -= OnDestNodeMoved;
            }

            var old = DestNode;
            DestNode = destNode;
            if (DestNode != null)
            {
                DestNode.Moving += OnDestNodeMoved;
            }

            base.Refresh();
        }

        public override void SetPosition(Vector2 position)
        {
            AutoPosition = false;
            base.SetPosition(position);
            Refresh();
        }

        public override void UpdatePosition(Vector2 delta)
        {
            base.UpdatePosition(delta);
            AutoPosition = false;
            Refresh();
        }

        private void OnDestNodeMoved(MoveableModel destNode) => Refresh();

        public ConnectionData Serialize()
        {
            var data = new ConnectionData()
            {
                AutoPosition = AutoPosition,
                SourceNodeID = SourceNode.Id,
                DestNodeID = DestNode?.Id,
                Size = Rect.size,
                Priority = Priority,
                ConditionalExpression =  ConditionalExpression?.Serialize(),
                IsFallback = IsFallback,
                ConnectionType = ConnectionType,
                EvaluatorID = Evaluator?.Id
            };

            data.SetModelData(this);

            return data;
        }
    }
}
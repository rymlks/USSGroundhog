using EasyState.Core.Models;
using EasyState.Data;
using EasyState.Editor.Renderers.DetailRenderers.Connections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using EasyState.DataModels;

namespace EasyState.Editor.Renderers.DetailRenderers
{
    public class ConnectionDetailRenderer : DetailRendererBase<Connection>
    {
        protected override string Title => $"{(Model.Name is null ? "Connection" : Model.Name)}({Model.SourceNode.Name} - {(Model.DestNode is null ? "Unset" : Model.DestNode.Name)}) Priority {Model.Priority}";
        private readonly DataTypeCollection _dataTypeCollection;
        private VisualElement _conditionContainer;
        private List<FunctionModel> _conditions;
        private VisualElement _connectionDataContainer;
        private List<Node> _destNodes;
        private List<FunctionModel> _evaluators;
        private VisualElement _leftConditionColumn;
        private VisualElement _rightConditionColumn;
        private Toggle _autoPosToggle;

        public ConnectionDetailRenderer(VisualElement container, Connection model, Design design) : base(container, model, design)
        {
            if (model.IsFallback)
            {
                var dataContainer = container.Q<VisualElement>("content");
                dataContainer.Clear();
                dataContainer.Add(new Label("Fallback connection can not have any conditions."));
            }
            else
            {
                _conditionContainer = container.Q<VisualElement>("conditions-container");
                _leftConditionColumn = _conditionContainer.Q<VisualElement>("left");
                _rightConditionColumn = _conditionContainer.Q<VisualElement>("right");
                _connectionDataContainer = container.Q<VisualElement>("connection-data");
                var db = new DataTypeDatabase();
                _dataTypeCollection = db.Load();
                var dataType = _dataTypeCollection.DataTypes.First(x => x.Id == design.DataTypeID);

                _conditions = dataType.GetConditions();
                //  _conditions.AddRange(dataType.LoadDataTypeConditionSet());
                _conditions.Insert(0, FunctionModel.NullFunction);

                _evaluators = dataType.GetEvaluators();
                _evaluators.Insert(0, FunctionModel.NullFunction);

                BuildPanel();
                BuildConditionContainer();
            }
            _autoPosToggle = new Toggle("Auto Position");
            _autoPosToggle.name = "auto-position";
            _autoPosToggle.SetValueWithoutNotify(Model.AutoPosition);
            _autoPosToggle.tooltip = "automatically calculate handle position.";
            Container.Insert(0, _autoPosToggle);
        }

        protected override void AddPropertyFields()
        {
            if (!Model.IsFallback)
            {
                PropertyCollection.AddTextProperty(x => x.Name, "name-input");
            }
            PropertyCollection.AddCheckProperty(x => x.AutoPosition, "auto-position");
            Model.Changed += Connection_Changed;
        }

        protected override void OnWindowClosing()
        {
            base.OnWindowClosing();
            Model.Changed -= Connection_Changed;
        }

        private void AddExpression()
        {
            Model.ConditionalExpression.AdditionalRows.Add(new ConditionalExpressionRow());
            Model.Refresh();
        }

        private VisualElement BuildAddExpressionButton()
        {
            var row = new VisualElement();
            row.style.flexDirection = FlexDirection.RowReverse;
            row.style.marginTop = 10;
            var addBtn = new Label("Add Expression");
            addBtn.AddToClassList("btn");
            addBtn.AddManipulator(new Clickable(AddExpression));

            row.Add(addBtn);

            return row;
        }

        private void BuildConditionContainer()
        {
            if (Model.IsFallback)
            {
                return;
            }
            if (Model.SourceNode.ConditionType != NodeConditionType.Utility)
            {
                _leftConditionColumn.Clear();
                _rightConditionColumn.Clear();
                _rightConditionColumn.Add(BuildExpressionRow(Model.ConditionalExpression.InitialConditionalRow));

                foreach (var addition in Model.ConditionalExpression.AdditionalRows)
                {
                    _leftConditionColumn.Add(BuildLogicPopup(addition));
                    _rightConditionColumn.Add(BuildExpressionRow(addition));
                }

                _rightConditionColumn.Add(BuildAddExpressionButton());
            }
            else
            {
                _connectionDataContainer.Clear();
                _connectionDataContainer.Add(BuildEvaluatorOptions());
            }
        }

        private PopupField<FunctionModel> BuildEvaluatorOptions()
        {
            var evaluatorOptions = new List<FunctionModel>(_evaluators);

            var conditionPopup = new PopupField<FunctionModel>("Evaluator", 
                                                                evaluatorOptions, 
                                                                FunctionModel.NullFunction,
                                                                x => PopupFormatters.FormatFunctionName(x, evaluatorOptions),
                                                                x => PopupFormatters.FormatFunctionName(x, evaluatorOptions));
            if (Model.Evaluator != null)
            {
                var conditionOption = evaluatorOptions.First(x => x.Id == Model.Evaluator.Id);
                conditionPopup.SetValueWithoutNotify(conditionOption);
            }
            conditionPopup.RegisterValueChangedCallback(e =>
            {
                if (e.newValue == FunctionModel.NullFunction)
                {
                    Model.Evaluator = null;
                }
                else
                {
                    Model.Evaluator = e.newValue;
                }
            });

            return conditionPopup;
        }

        private VisualElement BuildExpressionRow(ConditionalExpressionRow rowExp)
        {
            var row = new VisualElement();
            row.name = rowExp.Id;
            row.AddToClassList("action-row");
            var renderer = new ConditionalRowRenderer(row, rowExp, _conditions);
            renderer.Refresh();
            renderer.OnDeleteExpression += OnDeleteExpressionRow;
            return row;
        }

        private VisualElement BuildLogicPopup(ConditionalExpressionRow exp)
        {
            var popup = new EnumField(exp.ConditionalLogicType);
            popup.style.marginTop = 13;
            popup.RegisterValueChangedCallback(e =>
            {
                exp.ConditionalLogicType = (ConditionalLogicType)e.newValue;
            });
            return popup;
        }

        private void BuildPanel()
        {
            var destEle = Container.Q<VisualElement>("dest-input");
            _destNodes = Design.Nodes.Where(x => x != Model.SourceNode).ToList();

            var destPopup = new PopupField<Node>("Destination", 
                                                    _destNodes, 
                                                    Model.DestNode,
                                                    x => PopupFormatters.FormatDestName(x, _destNodes),
                                                    x => PopupFormatters.FormatDestName(x, _destNodes));
            destPopup.RegisterValueChangedCallback(DestChange);
            destEle.Add(destPopup);
        }

        private void Connection_Changed()
        {
            BuildConditionContainer();
            _autoPosToggle.SetValueWithoutNotify(Model.AutoPosition);
        }

        private void DestChange(ChangeEvent<Node> evt)
        {
            Model.SetDestNode(evt.newValue);
            Model.SourceNode.RefreshConnections();
        }

        private void OnDeleteExpressionRow(string id)
        {
            if (Model.ConditionalExpression.InitialConditionalRow.Id == id)
            {
                Model.ConditionalExpression.InitialConditionalRow = new ConditionalExpressionRow();
            }
            else
            {
                var rowToRemove = Model.ConditionalExpression.AdditionalRows.First(x => x.Id == id);
                Model.ConditionalExpression.AdditionalRows.Remove(rowToRemove);
            }
            Connection_Changed();
        }
    }
}
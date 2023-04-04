using EasyState.Core.Models;
using EasyState.Data;
using EasyState.DataModels;
using EasyState.Editor.Templates;
using EasyState.Settings;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace EasyState.Editor.Renderers.DetailRenderers
{
    public class NodeDetailRenderer : DetailRendererBase<Node>
    {
        protected override string Title => $"Edit Node";
        private Label _description;
        private TextField _summaryInput;
        private List<IDisposableView> _disposables = new List<IDisposableView>();

        private FloatField _exitDelayField;
        private PopupField<string> _delayVariableField;
        private const string NULL_DELAY_FIELD = "Select variable";
        public NodeDetailRenderer(VisualElement container, Node node, Design design) : base(container, node, design)
        {
            _description = new Label();
            _description.style.fontSize = 12;
            _description.style.whiteSpace = WhiteSpace.Normal;
            _description.style.flexWrap = Wrap.Wrap;
            _description.style.maxWidth = 315;
            var settingsFoldout = container.Q<Foldout>("settings-foldout");
            var settingsTemplate = TemplateFactory.CreateNodeSettings();
            settingsFoldout.Add(settingsTemplate);
            var descriptFoldout = container.Q<Foldout>("description-foldout");
            descriptFoldout.Add(_description);
            _disposables.Add(new ActionsRenderer(container.Q<Foldout>("actions-foldout"), node, design));
            _disposables.Add(new ConnectionsRenderer(node, container.Q<Foldout>("connections-foldout"), design));
            _summaryInput = container.Q<TextField>("summary-input");
            _summaryInput.SetValueWithoutNotify(Model.NodeSummary);
            _summaryInput.RegisterValueChangedCallback(x =>
            {
                Model.NodeSummary = x.newValue;
                Model.Refresh();
            });
            InitializeDelayVariableField(container);
            InitializeExitDelayField(container);
        }

        private void InitializeExitDelayField(VisualElement container)
        {
            _exitDelayField = container.Q<FloatField>("exit-delay");
            _exitDelayField.SetValueWithoutNotify(Model.ExitDelay);
            _exitDelayField.RegisterValueChangedCallback(x =>
            {
                Model.ExitDelay = x.newValue;
                GenerateDescription();
            });
        }

        private void InitializeDelayVariableField(VisualElement container)
        {
           var variableContainer = container.Q<VisualElement>("delay-variable");
            var choices = new List<string> { NULL_DELAY_FIELD};
            choices.AddRange(Design.DataType.GetFloatFields());
            _delayVariableField = new PopupField<string>("Delay Variable", choices, 0);
            variableContainer.Add(_delayVariableField);
            if (Model.ExitDelayField != null)
            {
                var fieldExists = choices.FirstOrDefault(x => x == Model.ExitDelayField) != null;
                if (fieldExists)
                {
                    _delayVariableField.SetValueWithoutNotify(Model.ExitDelayField);
                }
                else
                {
                    _delayVariableField.SetValueWithoutNotify(NULL_DELAY_FIELD);
                }
            }
            else
            {
                _delayVariableField.SetValueWithoutNotify(NULL_DELAY_FIELD);
            }
            _delayVariableField.RegisterValueChangedCallback(x =>
            {

               if(x.newValue == NULL_DELAY_FIELD)
                {
                    Model.ExitDelayField = null;
                }
                else
                {
                    Model.ExitDelayField = x.newValue;
                }
                UpdateDescription();
            });
        }

        protected override void AddPropertyFields()
        {
            PropertyCollection.AddTextProperty(x => x.Name, "name-input");
            if (Model.Name == NodePresetCollection.ENTRY_NODE)
            {
                var nameField = Container.Q<TextField>("name-input");
                nameField.SetEnabled(false);
            }
            //BuildCycleTypePopup();
            PropertyCollection.AddEnumProperty(x => x.ConditionType, "condition-type", refreshFunc: UpdateDescription);
            PropertyCollection.AddEnumProperty(x => x.ActionExecutionType, "action-type", refreshFunc: UpdateDescription);
            PropertyCollection.AddEnumProperty(x => x.CycleType, "cycle-type", refreshFunc: OnCycleTypeChanged);
            PropertyCollection.AddColorProperty(x => x.NodeColor, "node-color");
            PropertyCollection.AddColorProperty(x => x.SelectedNodeColor, "selected-color");
            OnCycleTypeChanged();
            OnNodeChanged();
            Model.Changed += OnNodeChanged;
        }

        private void OnCycleTypeChanged()
        {
            if (Model.CycleType == NodeCycleType.YieldSeconds)
            {
                _exitDelayField.style.display = DisplayStyle.Flex;
            }
            else
            {
                Model.ExitDelay = 0;
                _exitDelayField.style.display = DisplayStyle.None;
            }

            if (Model.CycleType == NodeCycleType.YieldVariableDelay)
            {
                _delayVariableField.style.display = DisplayStyle.Flex;
            }
            else
            {
                _delayVariableField.SetValueWithoutNotify(NULL_DELAY_FIELD);
                Model.ExitDelayField = null;
                _delayVariableField.style.display = DisplayStyle.None;
            }

            UpdateDescription();
        }

        protected override void OnWindowClosing()
        {
            Model.Changed -= OnNodeChanged;
            foreach (var item in _disposables)
            {
                item.Dispose();
            }
        }

        private string GenerateDescription()
        {
            string conditionString = null;
            string actionString = null;
            string cycleString = null;
            switch (Model.ConditionType)
            {
                case NodeConditionType.Default:
                    conditionString = "check connections from top to bottom and exit on first match";
                    break;

                case NodeConditionType.Repeat:
                    conditionString = "check connections from top to bottom and will stay in state until a connection is matched";
                    break;

                case NodeConditionType.Utility:
                    conditionString = "check connections from top to bottom and exit on highest scoring evaluation";
                    break;

                default:
                    break;
            }
            switch (Model.ActionExecutionType)
            {
                case NodeActionExecutionType.Default:
                    actionString = "execute action(s) based on their associated phase from top to bottom";
                    break;

                case NodeActionExecutionType.ConditionalExecution:
                    actionString = "execute action(s) only if attached condition is met";
                    break;

                default:
                    break;
            }
            switch (Model.CycleType)
            {
                case NodeCycleType.PassThrough:
                    cycleString = "exit state without yielding an update cycle";
                    break;

                case NodeCycleType.YieldCycle:
                    cycleString = "yield an update cycle after being updated";
                    break;

                case NodeCycleType.YieldSeconds:
                    cycleString = $"have a delay of {Model.ExitDelay} seconds upon exiting state";
                    break;
                case NodeCycleType.YieldVariableDelay:
                    if(Model.ExitDelayField == null)
                    {
                        cycleString = $"use a float variable from {Design.DataType.Name} as an exit delay length";
                    }
                    else
                    {
                        cycleString = $"use the value in {Design.DataType.Name}.{Model.ExitDelayField} as an exit delay length";
                    }
                    break;
                default:
                    break;
            }
            if (Model.Actions.Count == 0)
            {
                actionString = "not execute any actions";
            }
            if (IsLeafNode())
            {
                if (Model.CycleType == NodeCycleType.PassThrough)
                {
                    cycleString = "yield an update before entering next state";
                }
                conditionString = "loop back to the entry state node";
            }
            else if (Model.Connections.Count == 0)
            {
                conditionString += " This node has no connections";
            }
            if (Model.ConditionType == NodeConditionType.Repeat && Model.CycleType == NodeCycleType.PassThrough)
            {
                cycleString = "not yield an update cycle before either trying again to match a connection or exiting state";
            }

            string description = $"{Model.Name} will {cycleString}. Will {conditionString}. Will {actionString}.";

            if (!IsConnectedToDesign())
            {
                description += " This node currently can not be reached.";
            }

            return description;
        }
        private bool IsLeafNode()
        {
            if (Model.IsEntryNode)
            {
                return false;
            }
            if (Model.Connections.Count > 0)
            {
                return false;
            }
            if (Design.Nodes.Any(x => x.Connections.Any(y => y.DestNode == Model)))
            {
                return true;
            }
            return false;
        }
        private bool IsConnectedToDesign() => Design.Nodes.Any(x => x.Connections.Any(y => y.DestNode == Model)) || Model.IsEntryNode;
        private void OnNodeChanged()
        {
            UpdateDescription();
            foreach (var item in _disposables)
            {
                item.Refresh();
            }
        }
        private void UpdateDescription()
        {
            _description.text = GenerateDescription();
        }
    }
}
using EasyState.Core.Models;
using EasyState.Data;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using EasyState.DataModels;
using System;
using EasyState.Editor.Utility;

namespace EasyState.Editor.Renderers.DetailRenderers
{
    public class ActionsRenderer : IDisposableView
    {
        private readonly VisualElement _actionsContainer;
        private readonly Foldout _actionsFoldout;
        private readonly Design _design;
        private readonly Node _node;
        private List<FunctionModel> _actionChoices;
        private NodeActionExecutionType _cachedType;
        private List<string> _conditionalOptions = new List<string>() { "is true", "is false" };
        private List<FunctionModel> _conditionChoices;
        private DataTypeCollection _dataTypeCollection;

        public ActionsRenderer(Foldout actionsFoldout, Node model, Design design)
        {
            _actionsFoldout = actionsFoldout;
            _actionsContainer = new VisualElement();
            _actionsFoldout.Add(_actionsContainer);
            _actionsFoldout.Add(_actionsContainer);
            _node = model;
            _design = design;
            var db = new DataTypeDatabase();
            _dataTypeCollection = db.Load();
            _cachedType = _node.ActionExecutionType;
            var dataType = _dataTypeCollection.DataTypes.First(x => x.Id == _design.DataTypeID);
            _actionChoices = dataType.GetActions();
            _actionChoices.Insert(0, FunctionModel.NullFunction);

            _conditionChoices = dataType.GetConditions();
            _conditionChoices.Insert(0, FunctionModel.NullFunction);

            Refresh();
        }

        public void Dispose()
        {
        }

        public void Refresh()
        {
            if (_cachedType != _node.ActionExecutionType)
            {
                _node.Actions.Clear();
                _cachedType = _node.ActionExecutionType;
            }
            _actionsContainer.Clear();
            foreach (var action in _node.Actions.OrderBy(x => x.Priority))
            {
                if (action.IsConditional)
                {
                    _actionsContainer.Add(BuildConditionalActionRow(action));
                }
                else
                {
                    _actionsContainer.Add(BuildActionRow(action));
                }
            }
            _actionsContainer.Add(BuildAddButton());
        }

        private void AddAction()
        {
            var newAction = new NodeAction(_node.Id)
            {
                IsConditional = _node.ActionExecutionType == NodeActionExecutionType.ConditionalExecution,
                Priority = _node.Actions.Count,
                ActionPhase = NodeActionExecutionPhase.OnUpdate
            };
            _node.Actions.Add(newAction);
            _node.Refresh();
        }

        private PopupField<FunctionModel> BuildActionPopup(NodeAction action)
        {
            var popUp = new PopupField<FunctionModel>(_actionChoices,
                                                        FunctionModel.NullFunction,
                                                        x => PopupFormatters.FormatFunctionName(x, _actionChoices),
                                                        x => PopupFormatters.FormatFunctionName(x, _actionChoices));
            if (action.Action is null)
            {
                popUp.SetValueWithoutNotify(FunctionModel.NullFunction);
            }
            else
            {
                popUp.SetValueWithoutNotify(action.Action);
            }
            popUp.RegisterValueChangedCallback(x => OnActionChanged(x, action));
            popUp.style.width = 200;
            return popUp;
        }



        private VisualElement BuildActionRow(NodeAction action)
        {
            //row wrapper
            var row = new VisualElement();
            row.name = action.Id;
            row.AddToClassList("action-row");
            //priority button
            if (action.Priority != 0)
            {
                VisualElement priorityBtn = BuildPriorityButton(action);
                row.Add(priorityBtn);
            }
            //action phase
            BuildActionPhase(action, row);
            // action selection
            PopupField<FunctionModel> popUp = BuildActionPopup(action);
            row.Add(popUp);

            if (action.Action != null)
            {
                if (action.Action.HasParameter)
                {
                    switch (action.Action.ParameterType)
                    {
                        case nameof(String):
                            var textinput = new TextField();
                            textinput.SetValueWithoutNotify(action.ParameterDataString);
                            textinput.style.width = 150;
                            textinput.RegisterValueChangedCallback(x => action.ParameterDataString = x.newValue);
                            if (string.IsNullOrEmpty(action.ParameterDataString))
                            {
                                textinput.SetPlaceholderText("enter text parameter");
                            }
                            row.Add(textinput);
                            break;
                        case nameof(Single):
                            var floatInput = new FloatField();
                            if (!string.IsNullOrEmpty(action.ParameterDataString))
                            {
                                if (float.TryParse(action.ParameterDataString, out float input))
                                {
                                    floatInput.SetValueWithoutNotify(input);
                                }
                                else
                                {
                                    action.ParameterDataString = null;
                                }
                            }
                            floatInput.style.width = 150;
                            floatInput.RegisterValueChangedCallback(x => action.ParameterDataString = x.newValue.ToString());
                            row.Add(floatInput);
                            break;
                        case nameof(Int32):
                            var intInput = new IntegerField();
                            if (!string.IsNullOrEmpty(action.ParameterDataString))
                            {
                                if (int.TryParse(action.ParameterDataString, out int input))
                                {
                                    intInput.SetValueWithoutNotify(input);
                                }
                                else
                                {
                                    action.ParameterDataString = null;
                                }
                            }
                            intInput.style.width = 150;
                            intInput.RegisterValueChangedCallback(x => action.ParameterDataString = x.newValue.ToString());
                            row.Add(intInput);
                            break;
                        case nameof(Boolean):
                            var boolInput = new Toggle();
                            if (!string.IsNullOrEmpty(action.ParameterDataString))
                            {
                                if (bool.TryParse(action.ParameterDataString, out bool input))
                                {
                                    boolInput.SetValueWithoutNotify(input);
                                }
                                else
                                {
                                    action.ParameterDataString = null;
                                }
                            }
                            boolInput.RegisterValueChangedCallback(x => action.ParameterDataString = x.newValue.ToString());
                            row.Add(boolInput);
                            break;
                        case nameof(Enum):
                            Enum val;
                            var enumType = Type.GetType(action.Action.EnumType);
                            if (string.IsNullOrEmpty(action.ParameterDataString))
                            {
                                val = (Enum)Activator.CreateInstance(enumType);
                            }
                            else
                            {
                                val = (Enum)Enum.Parse( enumType, action.ParameterDataString);
                            }
                            var enumInput = new EnumField(val);
                            enumInput.RegisterValueChangedCallback(x => action.ParameterDataString = (x.newValue).ToString());
                            row.Add(enumInput);
                            break;
                        default:
                            break;
                    }
                }
            }
            // end spacer
            var endSpacer = new VisualElement();
            endSpacer.style.width = 15;
            row.Add(endSpacer);
            // delete button
            VisualElement deleteBtn = BuildDeleteButton(action);
            row.Add(deleteBtn);

            return row;
        }


        private VisualElement BuildConditionalActionRow(NodeAction action)
        {
            var row = new VisualElement();
            row.name = action.Id;
            row.AddToClassList("action-row");
            //priority button
            if (action.Priority != 0)
            {
                VisualElement priorityBtn = BuildPriorityButton(action);
                row.Add(priorityBtn);
            }
            //action phase
            BuildActionPhase(action, row);
            //if label
            var ifLabel = new Label("If");
            ifLabel.style.alignSelf = Align.Center;
            row.Add(ifLabel);
            //condition selection
            PopupField<FunctionModel> conditionPopup = BuildCondition(action);
            row.Add(conditionPopup);
            // is true/false selection
            PopupField<string> isPopup = new PopupField<string>(_conditionalOptions, action.ExecuteWhenFalse ? "is false" : "is true");
            isPopup.RegisterValueChangedCallback(x => OnIsPopupChanged(x, action));
            row.Add(isPopup);
            // then do label
            var thenLabel = new Label("then do");
            thenLabel.style.alignSelf = Align.Center;
            row.Add(thenLabel);
            // action selection
            PopupField<FunctionModel> actionPopUp = BuildActionPopup(action);
            row.Add(actionPopUp);
            // end spacer
            var endSpacer = new VisualElement();
            endSpacer.style.width = 15;
            row.Add(endSpacer);
            // delete button
            VisualElement deleteBtn = BuildDeleteButton(action);
            row.Add(deleteBtn);

            return row;
        }

        private VisualElement BuildAddButton()
        {
            var row = new VisualElement();
            row.style.flexDirection = FlexDirection.RowReverse;
            row.style.marginTop = 10;
            var addBtn = new Label("Add Action");
            addBtn.AddToClassList("btn");
            addBtn.AddManipulator(new Clickable(AddAction));

            row.Add(addBtn);

            return row;
        }

        private PopupField<FunctionModel> BuildCondition(NodeAction action)
        {
            var conditionPopup = new PopupField<FunctionModel>(_conditionChoices, FunctionModel.NullFunction, x => x.Name, x => x.Name);
            if (action.Condition is null)
            {
                conditionPopup.SetValueWithoutNotify(FunctionModel.NullFunction);
            }
            else
            {
                conditionPopup.SetValueWithoutNotify(action.Condition);
            }
            conditionPopup.RegisterValueChangedCallback(x => OnConditionChanged(x, action));
            conditionPopup.style.width = 200;
            return conditionPopup;
        }
        private static void BuildActionPhase(NodeAction action, VisualElement row)
        {
            var onLabel = new Label("On");
            onLabel.style.alignSelf = Align.Center;
            row.Add(onLabel);
            EnumField phase = new EnumField(action.ActionPhase);
            phase.RegisterValueChangedCallback<Enum>((x) => action.ActionPhase = (NodeActionExecutionPhase)x.newValue);
            row.Add(phase);
        }

        private VisualElement BuildDeleteButton(NodeAction action)
        {
            var deleteBtn = new VisualElement();
            deleteBtn.AddToClassList("close-button");
            deleteBtn.AddManipulator(new Clickable(() => OnDeleteAction(action.Id)));
            deleteBtn.style.top = 14;
            deleteBtn.style.right = 4;
            deleteBtn.tooltip = "Delete action";
            return deleteBtn;
        }

        private VisualElement BuildPriorityButton(NodeAction action)
        {
            var priorityBtn = new VisualElement();
            priorityBtn.AddToClassList("priority-up-icon");
            priorityBtn.AddToClassList("hover-icon");
            priorityBtn.tooltip = "Move priority up";
            priorityBtn.AddManipulator(new Clickable(() => OnPriorityMoveRequest(action.Id)));
            return priorityBtn;
        }

        private void OnActionChanged(ChangeEvent<FunctionModel> x, NodeAction action)
        {
            action.ParameterDataString = null;
            if (x.newValue == FunctionModel.NullFunction)
            {
                action.Action = null;
            }
            else
            {
                action.Action = x.newValue;
            }
            Refresh();
        }

        private void OnConditionChanged(ChangeEvent<FunctionModel> x, NodeAction action)
        {
            if (x.newValue == FunctionModel.NullFunction)
            {
                action.Condition = null;
            }
            else
            {
                action.Condition = x.newValue;
            }
        }

        private void OnDeleteAction(string id)
        {
            var actionToRemove = _node.Actions.First(x => x.Id == id);
            _node.Actions.Remove(actionToRemove);
            var rowToRemove = _actionsContainer.Q(id);
            _actionsContainer.Remove(rowToRemove);
        }

        private void OnIsPopupChanged(ChangeEvent<string> x, NodeAction action)
        {
            if (x.newValue == "is true")
            {
                action.ExecuteWhenFalse = false;
            }
            else
            {
                action.ExecuteWhenFalse = true;
            }
        }

        private void OnPriorityMoveRequest(string id)
        {
            var priorityToMoveUp = _node.Actions.First(x => x.Id == id);
            int targetPriority = priorityToMoveUp.Priority - 1;

            var priorityToMoveDown = _node.Actions.First(x => x.Priority == targetPriority);
            priorityToMoveDown.Priority++;
            priorityToMoveUp.Priority--;
            Refresh();
        }
    }
}
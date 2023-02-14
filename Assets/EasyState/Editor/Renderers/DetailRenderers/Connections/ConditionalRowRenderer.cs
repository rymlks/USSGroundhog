using EasyState.Core.Models;
using EasyState.Editor.Utility;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using EasyState.DataModels;

namespace EasyState.Editor.Renderers.DetailRenderers.Connections
{
    public class ConditionalRowRenderer : IDisposableView
    {
        public event System.Action<string> OnDeleteExpression;

        private readonly ConditionalExpressionRow _row;
        private readonly VisualElement _rowElement;
        private List<string> _conditionalOptions = new List<string>() { "is true", "is false" };
        private List<FunctionModel> _conditions;

        public ConditionalRowRenderer(VisualElement rowElement, ConditionalExpressionRow row, List<FunctionModel> conditions)
        {
            _rowElement = rowElement;
            _row = row;
            _conditions = conditions;
        }

        public void Dispose()
        {
        }

        public void Refresh()
        {
            _rowElement.Clear();
            var ifLabel = new Label("If");
            ifLabel.style.alignSelf = Align.Center;
            _rowElement.Add(ifLabel);
            PopupField<FunctionModel> conditionPopup = BuildCondition(_row.InitialExpression);
            _rowElement.Add(conditionPopup);
            _rowElement.Add(BuildIsPopup(_row.InitialExpression));

            foreach (var addition in _row.AdditionalExpressions)
            {
                _rowElement.Add(BuildAdditionalCondition(addition));
            }

            VisualElement deleteBtn = BuildDeleteButton(_row);
            var endSpacer = new VisualElement();
            endSpacer.style.width = 30;
            endSpacer.pickingMode = PickingMode.Ignore;
            _rowElement.Add(endSpacer);
            //TODO consider allow more expressions
            if (!_row.HasAdditionalExpressions)
            {
                _rowElement.Add(BuildAddConditionButton(_row));
            }
            _rowElement.Add(deleteBtn);
        }

        private VisualElement BuildAddConditionButton(ConditionalExpressionRow expRow)
        {
            var wrapper = new VisualElement();
            wrapper.style.position = Position.Absolute;
            wrapper.tooltip = "Add condition";
            wrapper.style.top = 14;
            wrapper.style.right = 20;
            wrapper.SetWidthAndHeight(10);
            var icon = new VisualElement();
            icon.AddToClassList("add-icon");
            wrapper.Add(icon);

            wrapper.AddManipulator(new Clickable(() =>
            {
                expRow.AdditionalExpressions.Add(new ConditionalExpression());

                Refresh();
            }));
            return wrapper;
        }

        private VisualElement BuildAdditionalCondition(ConditionalExpression exp)
        {
            var wrapper = new VisualElement();
            wrapper.style.flexDirection = FlexDirection.Row;
            wrapper.Add(BuildLogicPopup(exp));
            wrapper.Add(BuildCondition(exp));
            wrapper.Add(BuildIsPopup(exp));
            return wrapper;
        }

        private PopupField<FunctionModel> BuildCondition(ConditionalExpression exp)
        {
            var conditionOptions = new List<FunctionModel>(_conditions); 

            var conditionPopup = new PopupField<FunctionModel>(conditionOptions, 
                                                                FunctionModel.NullFunction,
                                                                x => PopupFormatters.FormatFunctionName(x,conditionOptions),
                                                                x => PopupFormatters.FormatFunctionName(x, conditionOptions));
            if (exp.Condition != null)
            {
                var conditionOption = conditionOptions.First(x => x.Id == exp.Condition.Id);
                conditionPopup.SetValueWithoutNotify(conditionOption);
            }
            conditionPopup.style.width = 100;
            conditionPopup.RegisterValueChangedCallback(e =>
            {
                if (e.newValue == FunctionModel.NullFunction)
                {
                    exp.Condition = null;
                }
                else
                {
                    exp.Condition = e.newValue;
                }
            });

            return conditionPopup;
        }

        private VisualElement BuildDeleteButton(ConditionalExpressionRow rowExp)
        {
            var deleteBtn = new VisualElement();
            deleteBtn.AddToClassList("close-button");
            deleteBtn.AddManipulator(new Clickable(() => OnDeleteExpression?.Invoke(rowExp.Id)));
            deleteBtn.style.top = 14;
            deleteBtn.style.right = 4;
            deleteBtn.tooltip = "Delete row";
            return deleteBtn;
        }

        private VisualElement BuildIsPopup(ConditionalExpression exp)
        {
            var isPopup = new PopupField<string>(_conditionalOptions, "is true");
            if (!exp.ExpectedResult)
            {
                isPopup.SetValueWithoutNotify("is false");
            }
            isPopup.RegisterValueChangedCallback(e =>
            {
                if (e.newValue == "is true" && isPopup != null || isPopup == null)
                {
                    exp.ExpectedResult = true;
                }
                else
                {
                    exp.ExpectedResult = false;
                }
            });
            return isPopup;
        }

        private VisualElement BuildLogicPopup(ConditionalExpression exp)
        {
            var popup = new EnumField(exp.ConditionalLogicType);

            popup.RegisterValueChangedCallback(e =>
            {
                exp.ConditionalLogicType = (ConditionalLogicType)e.newValue;
            });
            return popup;
        }
    }
}
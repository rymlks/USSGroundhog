using System;
using UnityEngine.UIElements;

namespace EasyState.Editor.Renderers.DetailRenderers.FieldRenderers
{
    public class PropertyField<TFieldType, TValueType> : IDisposable where TFieldType : BaseField<TValueType>
    {
        private readonly TFieldType _input;
        private readonly Action<TValueType> _setterExpression;

        public PropertyField(Action<TValueType> setterExprssion, string elementID, VisualElement parentElement, TValueType initialValue)
        {
            _input = parentElement.Q<TFieldType>(elementID);
            _input.RegisterValueChangedCallback(OnElementChanged);
            _setterExpression = setterExprssion;
            _input.SetValueWithoutNotify(initialValue);
        }

        public void Dispose()
        {
            _input.UnregisterValueChangedCallback(OnElementChanged);
        }

        private void OnElementChanged(ChangeEvent<TValueType> evt)
        {
            _setterExpression.Invoke(evt.newValue);
        }
    }
}
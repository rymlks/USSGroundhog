using System;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace EasyState.Editor.Renderers.DetailRenderers.FieldRenderers
{
    public class EnumPropertyField<TValueType> : IDisposable where TValueType : Enum
    {
        private readonly EnumField _input;
        private readonly Action<TValueType> _setterExpression;
        private readonly Action _refreshFunc;
        public EnumPropertyField(Action<TValueType> setterExprssion, string elementID, VisualElement parentElement, TValueType initialValue, bool isReadonly = false, Action refreshFunc = null)
        {
            _input = parentElement.Q<EnumField>(elementID);
            _input.RegisterValueChangedCallback(OnElementChanged);
            _setterExpression = setterExprssion;
            _input.SetValueWithoutNotify(initialValue);
            if (isReadonly)
            {
                _input.SetEnabled(false);
            }
            _refreshFunc = refreshFunc;
        }

        public void Dispose()
        {
            _input.UnregisterValueChangedCallback(OnElementChanged);
        }

        private void OnElementChanged(ChangeEvent<Enum> evt)
        {
            _setterExpression.Invoke((TValueType)evt.newValue);
            _refreshFunc?.Invoke();
        }
    }
}